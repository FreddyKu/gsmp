using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour {

    private int mouseX = 0;
    private int mouseY = 0;
    private int oldmouseX;
    private int oldmouseY;
    public float sensivity;

    private float pitch = 0;
    private float yaw = 0;
    private float distance = 16;
    public float scrollSensitivity = 10;
    private float xTranslate = 0;
    private float yTranslate = 0;
    public int movementRange = 70;

    private GameObject[] Ships;
    private GameObject Bullet;
    public GameObject bulletPrefab;
    public float shootSpeed = 10;
    private GameObject existingBullet;

	void Update () {
        oldmouseX = mouseX;
        oldmouseY = mouseY;
        mouseX = (int)Input.mousePosition.x;
        mouseY = (int)Input.mousePosition.y;
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            distance -= scrollSensitivity * Input.GetAxis("Mouse ScrollWheel");
            distance = Mathf.Clamp(distance, 5, 25);
            MoveCamera();
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine("CameraMove");
        }
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine("CameraRotate");
        }
        if (Input.GetMouseButtonDown(2))
        {
            Shoot();
        }

        
	}

    IEnumerator CameraMove()
    {
        while (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            
            xTranslate += oldmouseX - mouseX;
            yTranslate += oldmouseY - mouseY;
            xTranslate = Mathf.Clamp(xTranslate, -movementRange, movementRange);
            yTranslate = Mathf.Clamp(yTranslate, -movementRange, movementRange);

            MoveCamera();
            yield return null;

        }
    }

    IEnumerator CameraRotate()
    {
        while (Input.GetMouseButton(1) || Input.GetMouseButtonDown(1))
        {
            pitch += oldmouseY - mouseY;
            yaw += oldmouseX - mouseX;

            pitch = Mathf.Clamp(pitch, -90, 90);

            MoveCamera();
            yield return null;
        }
    }

    void MoveCamera()
    {
        transform.position = new Vector3(0, 5, -distance);
        transform.eulerAngles = new Vector3(0, 0, 0);

        transform.RotateAround(new Vector3(0, 5, 0), Vector3.left, -pitch);
        transform.RotateAround(new Vector3(0, 5, 0), Vector3.up, -yaw);

        transform.Translate(sensivity * new Vector3(xTranslate, yTranslate, 0));
    }

    void Shoot()
    {
        existingBullet = GameObject.FindGameObjectWithTag("Bullet");
        if (existingBullet != null)
            return;
        Ships = GameObject.FindGameObjectsWithTag("Ship");
        Bullet = (GameObject) Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet.GetComponent<Rigidbody>().AddForce(transform.forward * shootSpeed, ForceMode.Impulse);

        foreach (GameObject ship in Ships)
        {
            ship.GetComponent<ShipGravityControll>().FindBullet();
        }
    }
}
