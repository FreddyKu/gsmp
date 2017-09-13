using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControll : MonoBehaviour {

    /*
     * Code removes Bullet and logs flighttrack
     * Then, renders the line
     */

    #region Variables
    public int unloadDistance = 40;
    public int unloadSpeed = 20000;
    private List<Vector3> path = new List<Vector3>();
    
    public GameObject PathRender;
    public GameObject Ship;
    private GameObject pathRend;
    private LineRenderer line;
    private Vector3 velocity;
    #endregion

    private void FixedUpdate () {
        velocity = GetComponent<Rigidbody>().velocity;
        if (Vector3.Distance(transform.position, new Vector3(0,5,0))>unloadDistance && Vector3.Dot(transform.position-new Vector3(0,5,0),velocity)>0 || velocity.magnitude < unloadSpeed && velocity.magnitude != 0)
        {
            DrawPath(); //Calls Linerenderer
            Destroy(gameObject);
        } else
        {
            path.Add(transform.position); //Adds position to path
        }
	}

    private void DrawPath ()
    {
        pathRend = (GameObject) Instantiate(Resources.Load("PathRenderer"));
        line = pathRend.GetComponent<LineRenderer>();
        line.positionCount = path.ToArray().Length;
        line.SetPositions(path.ToArray());
    }
}
