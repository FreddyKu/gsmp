using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkSending : NetworkBehaviour {

    #region Variables
    private InfoTextAlpha infoText;
    private List<string> queue = new List<string>();
    #endregion

    public void Start() //Instantiates
    {
        infoText = GameObject.FindGameObjectWithTag("InfoText").GetComponent<InfoTextAlpha>();
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Send("J is pressed at your pc", 3, 0);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Send("K is pressed at enemys pc", 3, 1);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Send("L is pressed at all pcs", 3, 2);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(Chain("Test1", "Test2", "Test3", 2f, 0));
        }
    }*/

    public void Send(string message, float duration, int target) //Sends message for duration to target (0 = self; 1 = enemy; 2 = all)
    {
        if (target == 0)
        {
            infoText.Render(message, duration);
        } else if (target == 1)
        {
            queue.Add(message);
            CmdSend(message, duration);
        } else if (target == 2)
        {
            CmdSend(message, duration);
        }

    }
    public void ChainStart(string message1, string message2, float duration, int target)
    {
        StartCoroutine(Chain(message1, message2, duration, target));
    }
    private IEnumerator Chain(string message1, string message2, float duration, int target)
    {
        Send(message1, duration, target);
        yield return new WaitForSeconds(duration+1f);
        Send(message2, duration, target);
    }

    public void ChainStart(string message1, string message2, string message3, float duration, int target)
    {
        StartCoroutine(Chain(message1, message2, message3, duration, target));
    }
    private IEnumerator Chain(string message1, string message2, string message3, float duration, int target)
    {
        Send(message1, duration, target);
        yield return new WaitForSeconds(duration+1f);
        Send(message2, duration, target);
        yield return new WaitForSeconds(duration+1f);
        Send(message3, duration, target);
    }

    [Command]
    public void CmdSend(string message, float duration) 
    {
        RpcSend(message, duration);
    }

    [ClientRpc]
    public void RpcSend(string message, float duration) 
    {
        Receive(message, duration);
    }

    public void Receive(string message, float duration) //Receives Message and sends it to self
    {
        if (queue.Contains(message))
        {
            queue.Remove(message);
        } else
        {
            Send(message, duration, 0);
        }
    }
}