using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputTopicField : MonoBehaviour
{

    private BrokerConnection IPconfigScript;
    private InputField TopicInputField;
    private Text InputTopicText; //never assigned in Start

    public Dropdown QoSlevelDD;
    [HideInInspector]public byte QosLevel { get; private set; }

    void Start()
    {
        IPconfigScript = FindObjectOfType<BrokerConnection>();
        TopicInputField = GetComponent<InputField>();
    }

    public void CreateNewTopic(string NewTopic)
    {
        if (NewTopic.Equals(""))
        {
            return;
        }
        else
        {
            IPconfigScript.Subscribe(NewTopic, QosLevel);
            Debug.Log("Quality of service level: " + QosLevel);
            TopicInputField.text = "";
        }
    }

    public void OnButtonPress()
    {
        IPconfigScript.Subscribe(InputTopicText.text, QosLevel);
        Debug.Log(QosLevel);
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
