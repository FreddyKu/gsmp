using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour {

    private int mouseX = 0;
    private int mouseY = 0;
    private int oldmouseX;
    private int oldmouseY;
    public float sensivity;
	
	void Update () {
        oldmouseX = mouseX;
        oldmouseY = mouseY;
        mouseX = (int)Input.mousePosition.x;
        mouseY = (int)Input.mousePosition.y;

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine("CameraMove");
        }
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine("CameraRotate");
        }

        
	}

    IEnumerator CameraMove()
    {
        while (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            transform.Translate(sensivity * new Vector3(oldmouseX-mouseX,oldmouseY-mouseY,0));
            Debug.Log("Curr "+mouseX+" Last "+oldmouseX);
            yield return null;

        }
    }

    IEnumerator CameraRotate()
    {
        while (Input.GetMouseButton(1) || Input.GetMouseButtonDown(1))
        {
            //transform.RotateAround(new Vector3(0, 5, 0), Vector3.up, oldmouseX - mouseX);
            if (transform.eulerAngles.x + oldmouseY-mouseY<90 || transform.eulerAngles.x + oldmouseY - mouseY>270)
            transform.RotateAround(new Vector3(0, 5, 0), Vector3.left, oldmouseY - mouseY);
            Debug.Log("Euler " + transform.eulerAngles.x + " move " + (oldmouseY - mouseY));
            //Debug.Log(transform.rotation.x);
            yield return null;
        }
    }
}
