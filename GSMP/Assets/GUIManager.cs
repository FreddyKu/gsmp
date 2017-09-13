using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

    /*
     *  This Code is used to display the remaining amount of ships in the top right corner. 
     *  Takes the number from GameManager
     *  
     */

    int sum;
    GameObject GameManager;

	void Start () {
        GameManager = GameObject.FindGameObjectWithTag("GameController");
	}
	
	public void RenderShipsLeft () {
        gameObject.GetComponent<Text>().text = GameManager.GetComponent<ManagerControlls>().enemyfield.Count +"";
	}
}
