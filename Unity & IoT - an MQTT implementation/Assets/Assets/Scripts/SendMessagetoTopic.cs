using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendMessagetoTopic : MonoBehaviour
{

    public InputField Message;
    public Dropdown Topic;

    public Dropdown QoSlevelDD;
    private byte QosLevel;

    private List<string> messages /*= new List<string>() { "flashlight on", "flashlight off" }*/;
    public Dropdown messageChoice;

    private BrokerConnection IPconfigScript;
    private DropdownMessageTopic TopicScript;


    void Start()
    {
        IPconfigScript = FindObjectOfType<BrokerConnection>();
        TopicScript = FindObjectOfType<DropdownMessageTopic>();
        messages = new List<string>();
        //PopulateDropDownList();
    }

    private void PopulateDropDownList()
    {
        messageChoice.AddOptions(messages);
    }

    public void SendMessage()
    {
        if (!(Message.text.Equals("")) && TopicScript.GetSelectedTopic() != null)
        {
            IPconfigScript.SendMessagetoTopic(Message.text, TopicScript.GetSelectedTopic(), QosLevel);
            AddMessageToList(Message.text);
            messageChoice.ClearOptions();
            messageChoice.AddOptions(messages);
            Message.text = "";
        }
        else
        {
            Debug.Log("tomt meddelande eller null topic");
        }
    }

    public void AddMessageToList(string message)
    {
        if (messages.Count > 0)
        {
            for (int i = 0; i < messages.Count; i++)
            {
                if ((message == messages[i]))
                    return;
            }
        }
        messages.Add(message);
    }

    public void SelectMessage(int index)
    {
        Message.text = messages[index];
    }

    public void SelectQoSLevel(int index)
    {
        switch (index)
        {
            case 0:
                QosLevel = (byte)global::QosLevel.AT_LEAST_ONCE;
                break;
            case 1:
                QosLevel = (byte)global::QosLevel.AT_MOST_ONCE;
                break;
            case 2:
                QosLevel = (byte)global::QosLevel.EXACTLY_ONCE;
                break;
        }
        //QosLevel = (byte)index;
    }
}
