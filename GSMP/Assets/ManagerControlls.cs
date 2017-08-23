using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ManagerControlls : NetworkBehaviour {

    public GameObject ship;
    private GameObject currentShip;
    private GameObject[] PathList;
    private GameObject[] Ships;
    private int iddeletion = 1;
    private float x, y, z;
    public List<Vector3> field;
    public List<Vector3> enemyfield;
    public static bool gameStarted = false;
    private GameObject textbox;
    public int readyplayers = 0;

    private void Start()
    {
        textbox = GameObject.FindGameObjectWithTag("ShipCount");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Spawn();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Remove();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Unload();
        }
    }


    public void Load()
    {
        gameStarted = true;
        Unload();
        Load(enemyfield, true);
        textbox.GetComponent<GUIManager>().RenderShipsLeft();
    }

    private void Spawn()
    {
        x = Random.Range(-5, 5);
        y = Random.Range(0, 10);
        z = Random.Range(-5, 5);

        field.Add(new Vector3(x, y, z));

        Instantiate(ship, new Vector3(x, y, z), Quaternion.identity);
    }

    private void Unload()
    {
        Ships = GameObject.FindGameObjectsWithTag("Ship");
        foreach (GameObject ship in Ships)
        {
            Destroy(ship);
        }
    }

    private void Load(List<Vector3> shiplist, bool invisible)
    {
        foreach (Vector3 pos in shiplist)
        {
            currentShip = Instantiate(ship, pos, Quaternion.identity);
            if (invisible) currentShip.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void Remove()
    {
        PathList = GameObject.FindGameObjectsWithTag("PathRenderer");
        foreach (GameObject path in PathList)
        {
            if (path.GetComponent<PathRendererAge>().id==iddeletion)
            {
                path.GetComponent<LineRenderer>().enabled = false;
                iddeletion++;
                break; 
            }
        }
    }


}
