using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionInfo : MonoBehaviour
{
    private Text ConnectionText;
    void Start()
    {
        ConnectionText = GetComponent<Text>();

        StartCoroutine(PingHost(10f));
    }

    private IEnumerator PingHost(float waitTime)
    {
        float counter = 0f;

        while (counter < waitTime)
        {
            counter += Time.deltaTime;
            BrokerConnection.Instance.UpdateIPAddress();
            ConnectionText.text = "Connected to host: " + BrokerConnection.Instance.GetHostByName();
            yield return null;
        }
    }



}
