using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkConnector : MonoBehaviour {

    private GameObject GameManager;
    public GameObject ship;
    private GameObject shiptracker;
    public int readyplayers = 0;

    void Start () {
        GameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    public void Spawn(Vector3 pos)
    {
        if (!GameManager.GetComponent<ManagerControlls>().field.Contains(pos))
        GameManager.GetComponent<ManagerControlls>().enemyfield.Add(pos);
    }

    public void ReadyPlayer()
    {
        
        gameObject.GetComponent<CameraControll>().RpcStart();
        
    }
}
