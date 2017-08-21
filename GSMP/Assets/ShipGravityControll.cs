using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGravityControll : MonoBehaviour {

    private GameObject bullet;
    public float gravityIntensity = 5;
    public GameObject brokenShip;
    private GameObject textbox;
    private GameObject gameManager;

    public void FindBullet ()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        bullet = GameObject.FindGameObjectWithTag("Bullet");
        textbox = GameObject.FindGameObjectWithTag("ShipCount");
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
        gameManager.GetComponent<ManagerControlls>().enemyfield.Remove(transform.position);
        textbox.GetComponent<GUIManager>().RenderShipsLeft();
        Destroy(gameObject);
    }
}
