using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGravityControll : MonoBehaviour {

    private GameObject bullet;
    public float gravityIntensity = 5;
    public GameObject brokenShip;


    public void FindBullet ()
    {
        bullet = GameObject.FindGameObjectWithTag("Bullet");
        StartCoroutine("Act");
    }

    IEnumerator Act()
    {
        while (bullet != null)
        {
            bullet.GetComponent<Rigidbody>().AddForce(gravityIntensity * (transform.position - bullet.transform.position).normalized / Mathf.Pow(Vector3.Distance(transform.position, bullet.transform.position), 2));
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(brokenShip, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
