using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopicButtonSpawner : MonoBehaviour {

	public GameObject NewButtonPrefab;
	public Transform NewButtonParentPanel;
	private Text NewButtonText;

	public void NewTopicButton(string name){
		var NewButton = Instantiate (NewButtonPrefab, NewButtonParentPanel);
		NewButtonText = NewButton.GetComponentInChildren<Text>();
		NewButtonText.text = name;
		NewButtonText.color = Color.green;
	}
}