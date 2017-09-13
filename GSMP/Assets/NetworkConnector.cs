using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkConnector : MonoBehaviour {

    /* 
     * Accepts shipplacement over the Network and readies the player individually on own device
     */

    #region Variables
    private GameObject GameManager;
    public GameObject ship;
    private GameObject shiptracker;
    public int readyplayers = 0;
    #endregion

    void Start () //Instantiates
    {
        GameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    public void Spawn(Vector3 pos) //Adds argument to list of future ships if it isn't on the GameManager's list
    {
        if (!GameManager.GetComponent<ManagerControlls>().field.Contains(pos))
        GameManager.GetComponent<ManagerControlls>().enemyfield.Add(pos);
    }

    public void ReadyPlayer() //Calls "RpcStart()" on CameraControll on server
    {
        gameObject.GetComponent<CameraControll>().RpcStart();  
    }
}
