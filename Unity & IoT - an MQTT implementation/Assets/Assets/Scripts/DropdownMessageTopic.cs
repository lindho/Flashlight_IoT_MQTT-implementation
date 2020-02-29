using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// <descrpition>
/// This script is used to select a topic from a dropdown if any topics are added in the broker connection 'topics[]' 
/// (<see cref="BrokerConnection.topics"/>)
/// </descrpition>
/// </summary>
[RequireComponent(typeof(Dropdown))]
public class DropdownMessageTopic : MonoBehaviour {

    [Header("Object that holds the broker connection script")]
	public BrokerConnection brokerConnectionScript;

	private Dropdown topic;
	private string currentSelectedTopic;

    /// <summary>
    /// <description>Gets the attached dropdown component for this object 
    /// when this script instance is being loaded</description>
    /// </summary>
	void Awake () {
		topic = GetComponent<Dropdown>();
    }

    /// <summary>
    /// <desrpition>When this game object is enabled and active, topics from the broker connection script 
    /// gets added to the dropdown. The first topic in the list are selected automically. 
    /// If the topic list is empty no connection to the broker is available</desrpition>  
    /// </summary>
	public void OnEnable(){
		topic.ClearOptions ();
		topic.AddOptions(brokerConnectionScript.client.GetTopicList());
		if (topic.options.Count == 1)
			SelectTopic (0);
		else if (topic.options.Count == 0)
			currentSelectedTopic = null;
	}

    /// <summary>
    /// <description>Sets the current topic by index</description>
    /// </summary>
    /// <param name="index"> is refering to the index for which a topic is located in the list of topics</param>
	public void SelectTopic(int index){
		currentSelectedTopic = brokerConnectionScript.client.GetTopicList()[index];
	}
		
    /// <summary>
    /// <description>Gets the current selected topic</description>
    /// </summary>
    /// <returns>currently selected topic from the topic list in broker connection script</returns>
	public string GetSelectedTopic(){
		return currentSelectedTopic;
	}
}
