using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRendererAge : MonoBehaviour {

    public int id;

	void Start () {
        id = GameObject.FindGameObjectsWithTag("PathRenderer").Length;
        Debug.Log("PathRendererAge: " + id);
	}
	
}
