using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGravityControll : MonoBehaviour {

    /*
     * Code finds the Bullet and applies gravitational force until it disappears
     * Also handles Collision/Hits
     */

    #region Variables
    private GameObject bullet;
    public float gravityIntensity = 5;
    public GameObject brokenShip;
    private GameObject textbox;
    private GameObject gameManager;
    #endregion

    public void FindBullet () //Gets called when bullet spawns and calls "Act()"
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        bullet = GameObject.FindGameObjectWithTag("Bullet");
        textbox = GameObject.FindGameObjectWithTag("ShipCount");
        StartCoroutine(Act());
    }

    IEnumerator Act() //Applies Force until bullet disappears
    {
        while (bullet != null)
        {
            bullet.GetComponent<Rigidbody>().AddForce(gravityIntensity * (transform.position - bullet.transform.position).normalized / Mathf.Pow(Vector3.Distance(transform.position, bullet.transform.position), 2));
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision) //Bullet Hit handeler
    {
        Instantiate(brokenShip, transform.position, Quaternion.identity);
        gameManager.GetComponent<ManagerControlls>().enemyfield.Remove(transform.position);
        textbox.GetComponent<GUIManager>().RenderShipsLeft();
        Destroy(gameObject);
    }
}
