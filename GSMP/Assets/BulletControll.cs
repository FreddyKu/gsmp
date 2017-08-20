using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControll : MonoBehaviour {

    public int unloadDistance = 40;
    private List<Vector3> path = new List<Vector3>();
    
    public GameObject PathRender;
    public GameObject Ship;
    private GameObject pathRend;
    private LineRenderer line;
	
	void Update () {
        if (Vector3.Distance(transform.position, new Vector3(0,5,0))>unloadDistance)
        {
            Debug.Log("Drawing");
            DrawPath();
            Destroy(gameObject);
        } else
        {
            path.Add(transform.position);
        }
	}

    void DrawPath ()
    {
        pathRend = (GameObject)Instantiate(Resources.Load("PathRenderer"));
        line = pathRend.GetComponent<LineRenderer>();
        line.positionCount = path.ToArray().Length;
        line.SetPositions(path.ToArray());
    }
}
