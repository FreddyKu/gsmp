using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkSending : NetworkBehaviour {

    private InfoTextAlpha infoText;
    private List<string> queue = new List<string>();

    public void Start()
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
    }*/

    public void Send(string s, float duration, int target)
    {
        if (target == 0)
        {
            infoText.Render(s, duration);
        } else if (target == 1)
        {
            queue.Add(s);
            CmdSend(s, duration);
        } else if (target == 2)
        {
            CmdSend(s, duration);
        }

    }

    [Command]
    public void CmdSend(string s, float d)
    {
        RpcSend(s,d);
    }

    [ClientRpc]
    public void RpcSend(string s, float d)
    {
        Receive(s,d);
    }

    public void Receive(string s, float d)
    {
        if (queue.Contains(s))
        {
            queue.Remove(s);
        } else
        {
            Send(s, d, 0);
        }
    }
}