using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionInfo : MonoBehaviour
{
    public GameObject BrokerConnection;
    void Start()
    {
        Text text = GetComponent<Text>();
        text.text = "Connected to host: " + GetHostByName();
    }
    public static string GetHostByName()
    {
        try
        {
            IPHostEntry Hosts = Dns.GetHostEntry("mqtt-dashboard.com");
            return Hosts.AddressList[0].ToString();
        }

        catch (SocketException e)
        {
            Debug.LogError("SocketException caught!!!");
            Debug.LogError("Source : " + e.Source);
            Debug.LogError("Message : " + e.Message);
        }
        catch (ArgumentNullException e)
        {
            Debug.LogError("ArgumentNullException caught!!!");
            Debug.LogError("Source : " + e.Source);
            Debug.LogError("Message : " + e.Message);
        }
        catch (Exception e)
        {
            Debug.LogError("Exception caught!!!");
            Debug.LogError("Source : " + e.Source);
            Debug.LogError("Message : " + e.Message);
        }
        return "";
    }

    void Update()
    {
        
    }
}
