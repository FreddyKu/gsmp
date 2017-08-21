using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

    int sum;
    GameObject GameManager;

	void Start () {
        GameManager = GameObject.FindGameObjectWithTag("GameController");
	}
	
	 public void RenderShipsLeft () {
        gameObject.GetComponent<Text>().text = GameManager.GetComponent<ManagerControlls>().enemyfield.Count +"";
	}
}
