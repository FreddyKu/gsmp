using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoTextAlpha : MonoBehaviour {

    /*
     * Code is used to visualize text and messages on the screen
     * 
     */ 

    Text tx;
    private int count = 0;

    private void Start()
    {
        tx = gameObject.GetComponent<Text>();
    }

    public void Render(string s, float duration)
    {
        StartCoroutine(TextVisualize(s,duration));
    }

    public IEnumerator TextVisualize(string s, float duration)
    {
        count++;
        tx.text = s;
        tx.color = new Color(tx.color.r, tx.color.g, tx.color.b, 0);
        while (tx.color.a < 1)
        {
            tx.color = new Color(tx.color.r, tx.color.g, tx.color.b, tx.color.a + Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(duration);
        while (tx.color.a > 0 && count == 1)
        {
            tx.color = new Color(tx.color.r, tx.color.g, tx.color.b, tx.color.a - Time.deltaTime);
            yield return null;
        }
        count--;
    }
}
