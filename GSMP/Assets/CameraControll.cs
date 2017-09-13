using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraControll : NetworkBehaviour {

    /*
     * Code contains Camera Movement, Ready Function, Ship Placement and Shooting functions.
     */

    #region Variables
    private int mouseX = 0;
    private int mouseY = 0;
    private int oldmouseX;
    private int oldmouseY;
    public float sensivity;

    private float pitch = 0;
    private float yaw = 0;
    private float distance = 16;
    public float scrollSensitivity = 10;
    private float xTranslate = 0;
    private float yTranslate = 0;
    public int movementRange = 70;

    private GameObject GameManager;
    private GameObject InfoText;
    public GameObject ship;
    private GameObject shiptracker;
    private GameObject[] Ships;
    private GameObject Bullet;
    public GameObject bulletPrefab;
    public float shootSpeed = 10;
    private GameObject existingBullet;
    private bool inPlacement = false;
    private float placementDistance = 10;
    private int placedShips = 0;
    public int placeShipMax;

    //private int readyPlayers;
    private bool isReady = false;
    private float readyDelayTime = 4.5f;
    private NetworkSending sender;

    [SyncVar]
    public int playercount;
    #endregion

    private void Start()
    {
        CmdCheckPlayers();
        InfoText = GameObject.FindGameObjectWithTag("InfoText");
        sender = gameObject.GetComponent<NetworkSending>();
        //readyPlayers = 0;
        GameManager = GameObject.FindGameObjectWithTag("GameController");
        transform.position = new Vector3(0, 5, -16);
        if (isLocalPlayer)
        {
            gameObject.GetComponent<Camera>().enabled = true;
        }
        if (playercount == 2)
        {
            sender.ChainStart("All players connected","Placement-Phase beginns","Use [Middle Click] to place your ships",2.5f,2);
        }
    }

    void Update () //Detects Mouse Input, starts needed Methods and manages "Ready"-state
    {
        if (isLocalPlayer)
        {
            oldmouseX = mouseX; 
            oldmouseY = mouseY; //Detect change in Mouse coordinates
            mouseX = (int)Input.mousePosition.x;
            mouseY = (int)Input.mousePosition.y;
            if (Input.GetAxis("Mouse ScrollWheel") != 0 && !inPlacement) //Only change distance if not in placement
            {
                distance -= scrollSensitivity * Input.GetAxis("Mouse ScrollWheel");
                distance = Mathf.Clamp(distance, 7, 25);
                MoveCamera();
            }

            if (Input.GetMouseButtonDown(0)) //Detect Input "Left Mouseclick"
            {
                StartCoroutine("CameraMove");
            }
            if (Input.GetMouseButtonDown(1)) //Detect Input "Right Mouseclick"
            {
                StartCoroutine("CameraRotate");
            }
            if (Input.GetMouseButtonDown(2)) //Detect Input "Middle Mouseclick"
            {
                if (ManagerControlls.gameStarted) //If game has started shoot
                    Shoot();
                else
                    StartCoroutine("Place"); //If game hasn't started start placement
            }
            if (Input.GetKeyDown(KeyCode.Space) && placedShips>=placeShipMax) //Ready Player
            {
                if (!isReady) 
                {
                    sender.Send("You are ready", 1.5f, 0);
                    sender.Send("Enemy is ready", 1.5f, 1);
                    StartCoroutine(ReadyDelay());
                    isReady = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space) && placedShips < placeShipMax) //Ready Player
            {
                sender.Send("You have not finished placing your ships", 2f, 0);
            }
        }
	}

    private void Shoot() //Bullet Spawn and Gravity-instantiate
    {
        existingBullet = GameObject.FindGameObjectWithTag("Bullet");
        if (existingBullet != null) //Only shoot if there is no bullet
            return;
        Ships = GameObject.FindGameObjectsWithTag("Ship"); //Find all Ships before shooting
        Bullet = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.identity); //Spawn Bullet
        Bullet.GetComponent<Rigidbody>().AddForce(transform.forward * shootSpeed, ForceMode.Impulse); //Propell Bullet

        foreach (GameObject ship in Ships) //Start gravitytional effect on all ships
        {
            ship.GetComponent<ShipGravityControll>().FindBullet();
        }
    }

    #region ReadyPlayer Methods
    IEnumerator ReadyDelay() //Wait before Starting game
    {
        yield return new WaitForSeconds(readyDelayTime);
        CmdReady();
    }

    [Command]
    void CmdReady() //Order Server to start "ReadyPlayer()" on each client
    {
        RpcStart();
    }

    [ClientRpc]
    public void RpcStart() //Call "ReadyPlayer()" on each client
    {
        ReadyPlayer(); 
    } 

    private void ReadyPlayer() //Ready Player method and Status Renderer
    {
        GameManager.GetComponent<ManagerControlls>().readyplayers++;
        if (GameManager.GetComponent<ManagerControlls>().readyplayers > 1)
        {
            GameManager.GetComponent<ManagerControlls>().LoadField();
            sender.Send("Game starts", 2.5f, 0);
        }
    }
    #endregion

    #region CameraMovement Methods
    IEnumerator CameraMove() //Sets yTranslate and xTranslate
    {
        if (isLocalPlayer) {
            while (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
            {

                xTranslate += oldmouseX - mouseX;
                yTranslate += oldmouseY - mouseY;
                xTranslate = Mathf.Clamp(xTranslate, -movementRange, movementRange);
                yTranslate = Mathf.Clamp(yTranslate, -movementRange, movementRange);

                MoveCamera();
                yield return null;
            }
        }
    }

    IEnumerator CameraRotate() //Sets yaw and pitch
    {
        while (Input.GetMouseButton(1) || Input.GetMouseButtonDown(1))
        {
            pitch += oldmouseY - mouseY;
            yaw += oldmouseX - mouseX;

            pitch = Mathf.Clamp(pitch, -90, 90);

            MoveCamera();
            yield return null;
        }
    }

    private void MoveCamera() //Renders Movement
    {
        transform.position = new Vector3(0, 5, -distance);
        transform.eulerAngles = new Vector3(0, 0, 0);

        transform.RotateAround(new Vector3(0, 5, 0), Vector3.left, -pitch);
        transform.RotateAround(new Vector3(0, 5, 0), Vector3.up, -yaw);

        transform.Translate(sensivity * new Vector3(xTranslate, yTranslate, 0));
    }
    #endregion  

    #region Placement Methods
    IEnumerator Place() //Ship Placement Method
    {
        if (inPlacement) yield break; //If already in placement, quit

        if (placedShips >= placeShipMax) //If reached ship maximum
        {
            sender.Send("Maximum ship amount reached", 2.5f, 0);
            yield break;
        }

        CmdCheckPlayers();
        if (playercount == 1) //If no one connected, quit
            {
                sender.Send("Waiting for players", 0.5f, 0);
                yield break;
            }

        inPlacement = true;
        while (!Input.GetMouseButtonUp(2)) //Wait for button release
        {
            yield return null;
        }
        
        shiptracker = (GameObject)Instantiate(ship, transform.position + transform.forward * placementDistance, Quaternion.identity); //Instantiate Dummy Ship

        while (!Input.GetMouseButtonDown(2)) //Until button is pressed again render ship at cursor
        {
            placementDistance += scrollSensitivity * Input.GetAxis("Mouse ScrollWheel");
            shiptracker.transform.position = transform.position + transform.forward * placementDistance;
            yield return null;
        }

        if (Mathf.Abs(shiptracker.transform.position.x)>5 ||Mathf.Abs(shiptracker.transform.position.z)>5 ||shiptracker.transform.position.y<0 || shiptracker.transform.position.y > 10)
        {
            Destroy(shiptracker);
            inPlacement = false;
            yield break;
        } //If faulty placement, break

        if (isServer)
        {
            GameManager.GetComponent<ManagerControlls>().field.Add(shiptracker.transform.position); //Add ship to own ship list
            RpcSpawn(shiptracker.transform.position); //Spawn future Ship everywhere excluding oneself because of List
        }
        if (isClient) CmdSpawn(shiptracker.transform.position); //Spawn future Ship on server
        placedShips++;
        inPlacement = false;

        if (placedShips == placeShipMax)
        {
            sender.ChainStart("You finished placing your ships", "Press [Space] when ready", 2f, 0);
        }
    }

    [Command] 
    void CmdSpawn (Vector3 pos) //Spawn Ship on Server
    {
        gameObject.GetComponent<NetworkConnector>().Spawn(pos);
    }

    [ClientRpc]
    void RpcSpawn (Vector3 pos) //Spawn Ship everywhere
    {
        gameObject.GetComponent<NetworkConnector>().Spawn(pos);
    }

    [Command]
    void CmdCheckPlayers() //Set "playercount" to current connection count
    {
        playercount = NetworkServer.connections.Count;
    }  
    #endregion

}