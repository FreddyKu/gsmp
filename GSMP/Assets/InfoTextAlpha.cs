using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoTextAlpha : MonoBehaviour {

    /*
     * Code is used to visualize text and messages on the screen
     */

    #region Variables
    Text tx;
    private int count = 0;
    #endregion

    private void Start() //Instantiates
    {
        tx = gameObject.GetComponent<Text>();
    }

    public void Render(string message, float duration) //Calls "TextViualize()" 
    {
        StartCoroutine(TextVisualize(message, duration));
    }

    public IEnumerator TextVisualize(string message, float duration) //Renders message for duration
    {
        count++;
        tx.text = message;
        tx.color = new Color(tx.color.r, tx.color.g, tx.color.b, 0);
        while (tx.color.a < 1)
        {
            tx.color = new Color(tx.color.r, tx.color.g, tx.color.b, tx.color.a + Time.deltaTime*2);
            yield return null;
        }
        yield return new WaitForSeconds(duration);
        while (tx.color.a > 0 && count == 1)
        {
            tx.color = new Color(tx.color.r, tx.color.g, tx.color.b, tx.color.a - Time.deltaTime*2);
            yield return null;
        }
        count--;
    }
}
