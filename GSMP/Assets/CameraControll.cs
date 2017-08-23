using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraControll : NetworkBehaviour {

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
    public GameObject ship;
    private GameObject shiptracker;
    private GameObject[] Ships;
    private GameObject Bullet;
    public GameObject bulletPrefab;
    public float shootSpeed = 10;
    private GameObject existingBullet;
    private bool inPlacement = false;
    private float placementDistance = 10;

    private int readyPlayers;
    private bool isReady = false;

    private void Start()
    {
        readyPlayers = 0;
        GameManager = GameObject.FindGameObjectWithTag("GameController");
        transform.position = new Vector3(0, 5, -16);
        if (isLocalPlayer)
        {
            gameObject.GetComponent<Camera>().enabled = true;
        }
    }

    void Update () {
        if (isLocalPlayer)
        {
            oldmouseX = mouseX;
            oldmouseY = mouseY;
            mouseX = (int)Input.mousePosition.x;
            mouseY = (int)Input.mousePosition.y;
            if (Input.GetAxis("Mouse ScrollWheel") != 0 && !inPlacement)
            {
                distance -= scrollSensitivity * Input.GetAxis("Mouse ScrollWheel");
                distance = Mathf.Clamp(distance, 5, 25);
                MoveCamera();
            }

            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine("CameraMove");
            }
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine("CameraRotate");
            }
            if (Input.GetMouseButtonDown(2))
            {
                if (ManagerControlls.gameStarted)
                    Shoot();
                else
                    StartCoroutine("Place");
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!isReady)
                {
                    Debug.Log("is Ready");
                    //Debug.Log(readyPlayers);
                    CmdReady();
                    isReady = true;

                }
            }
        }
	}

    [Command]
    void CmdReady()
    {
        RpcStart();
    }

    [ClientRpc]
    public void RpcStart()
    {
        ReadyPlayer(); 
    }

    private void ReadyPlayer()
    {
        GameManager.GetComponent<ManagerControlls>().readyplayers++;
        Debug.Log(GameManager.GetComponent<ManagerControlls>().readyplayers);
        if (GameManager.GetComponent<ManagerControlls>().readyplayers > 1)
        {
            GameManager.GetComponent<ManagerControlls>().Load();
            Debug.Log("Done");
        }
    }

    IEnumerator CameraMove()
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

    IEnumerator CameraRotate()
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

    private void MoveCamera()
    {
        transform.position = new Vector3(0, 5, -distance);
        transform.eulerAngles = new Vector3(0, 0, 0);

        transform.RotateAround(new Vector3(0, 5, 0), Vector3.left, -pitch);
        transform.RotateAround(new Vector3(0, 5, 0), Vector3.up, -yaw);

        transform.Translate(sensivity * new Vector3(xTranslate, yTranslate, 0));
    }

    private void Shoot()
    {
        existingBullet = GameObject.FindGameObjectWithTag("Bullet");
        if (existingBullet != null)
            return;
        Ships = GameObject.FindGameObjectsWithTag("Ship");
        Bullet = (GameObject) Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet.GetComponent<Rigidbody>().AddForce(transform.forward * shootSpeed, ForceMode.Impulse);

        foreach (GameObject ship in Ships)
        {
            ship.GetComponent<ShipGravityControll>().FindBullet();
        }
    }

    IEnumerator Place()
    {
        if (inPlacement) yield break;
        inPlacement = true;
        while (!Input.GetMouseButtonUp(2))
        {
            yield return null;
        }
        
        shiptracker = (GameObject)Instantiate(ship, transform.position + transform.forward * placementDistance, Quaternion.identity);

        while (!Input.GetMouseButtonDown(2))
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
        }

        //GameManager.GetComponent<ManagerControlls>().field.Add(shiptracker.transform.position);

        if (isServer)
        {
            GameManager.GetComponent<ManagerControlls>().field.Add(shiptracker.transform.position);
            RpcSpawn(shiptracker.transform.position);
        }
        if (isClient) CmdSpawn(shiptracker.transform.position);

        inPlacement = false;
    }

    [Command] 
    void CmdSpawn (Vector3 pos)
    {
        gameObject.GetComponent<NetworkConnector>().Spawn(pos);
    }

    [ClientRpc]
    void RpcSpawn (Vector3 pos)
    {
        gameObject.GetComponent<NetworkConnector>().Spawn(pos);
    }
}
