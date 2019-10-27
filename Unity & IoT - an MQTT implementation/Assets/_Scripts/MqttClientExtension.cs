using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;


public class MqttClientExtension : MqttClient {


    private readonly MqttClient client;
    private List<string> topicList = new List<string>();

    /// <summary>
    /// <description>Default constructor for the mqtt client</description>
    /// </summary>
    /// <param name="brokerIpAddress">the ip address for the broker</param>
    /// <param name="brokerPort">MQTT port, default to 1883</param>
    /// <param name="secure">Use ssl or not</param>
    /// <param name="caCert">Provides methods that help you use X.509 v.3 certificates.</param>
    public MqttClientExtension(IPAddress brokerIpAddress, int brokerPort, bool secure, X509Certificate caCert) : base(brokerIpAddress, brokerPort, secure, caCert)
    {

    }


    /// <summary>
    /// <description>Subscribes a client to defined topic</description>
    /// </summary>
    /// <param name="topics">the topic the client subscribes to</param>
    /// <param name="qosLevels">The level of quality service the message is sent with</param>
    /// <returns></returns>
    public override ushort Subscribe(string [] topics, byte [] qosLevels) {
        MqttMsgSubscribe subscribe = new MqttMsgSubscribe(topics, qosLevels);
        subscribe.MessageId = this.GetMessageId();

        // enqueue subscribe request into the inflight queue
        this.EnqueueInflight(subscribe, MqttMsgFlow.ToPublish);

        if (topicList.Count == 0)
        {
            for (int i = 0; i < topics.Length; i++)
                AddButton(topics, i);
        }
        else
        {
            for (int j = 0; j < topics.Length; j++)
            {
                for (int i = 0; i < topicList.Count; i++)
                {
                    if (topicList[i].Equals(topics[j]))
                    {
                        return subscribe.MessageId;
                    }
                }
                //Debug.Log (topicList [i]);
                Debug.Log(topics[j]);
                AddButton(topics, j);
            }
        }
        return subscribe.MessageId;
    }

    /// <summary>
    /// <description>The list of topics</description>
    /// </summary>
    /// <returns>A list of topics</returns>
    public List<string> GetTopicList()
    {
        return topicList;
    }


    /// <summary>
    /// <description>Adds a topic to the dropdown and creates a button</description>
    /// </summary>
    /// <param name="topics">The topics array</param>
    /// <param name="number">The index in the array</param>
    private void AddButton(string[] topics, int number)
    {
        topicList.Add(topics[number]);
        var buttonSpawner = MonoBehaviour.FindObjectOfType<TopicButtonSpawner>();
        buttonSpawner.NewTopicButton(topics[number]);

    }

    /// <summary>
    /// <description>Function to remove topic from the topic list</description>
    /// </summary>
    /// <param name="topic">The topic to be removed from the array</param>
    public void RemoveButton(string topic)
    {
        if (topicList.Count == 0)
        {
            return;
        }

        for (int i = 0; i < topicList.Count; i++)
        {
            if (topicList.Contains(topic))
            {
                topicList.Remove(topic);
            }
        }
    }
}
