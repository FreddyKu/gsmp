using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ManagerControlls : NetworkBehaviour {

    #region Variables
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
    #endregion

    private void Start() //Instantiates
    {
        textbox = GameObject.FindGameObjectWithTag("ShipCount");
    }

    private void Update() //Checks for Buttonpresses
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Remove();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void LoadField() //Loads enemyfield while unloading current field
    {
        gameStarted = true;
        Unload();
        Load(enemyfield, true);                                                  //!!!!!!!!!!!!!!!!!!! TWO LOAD METHODS !!!!!!!!!!!!!!!!!!!!!! (Fixed but Bugs could appear)
        textbox.GetComponent<GUIManager>().RenderShipsLeft();
    }

    private void Unload() //Unloads current field
    {
        Ships = GameObject.FindGameObjectsWithTag("Ship");
        foreach (GameObject ship in Ships)
        {
            Destroy(ship);
        }
    }

    private void Load(List<Vector3> shiplist, bool invisible) //Instantiates a ship for each ship in given list
    {
        foreach (Vector3 pos in shiplist)
        {
            currentShip = Instantiate(ship, pos, Quaternion.identity);
            if (invisible) currentShip.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void Remove() //Removes oldest line
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
