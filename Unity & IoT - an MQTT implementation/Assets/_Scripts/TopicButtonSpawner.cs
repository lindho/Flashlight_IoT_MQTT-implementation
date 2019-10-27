using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopicButtonSpawner : MonoBehaviour {

	public GameObject NewButtonPrefab;
	public Transform NewButtonParentPanel;
	private Text NewButtonText;

    private List<TopicsButton> topicButtons = new List<TopicsButton>();

	public void NewTopicButton(string name){
		var NewButton = Instantiate (NewButtonPrefab, NewButtonParentPanel);
		NewButtonText = NewButton.GetComponentInChildren<Text>();
		NewButtonText.text = name;
		NewButtonText.color = Color.green;
        topicButtons.Add(NewButtonPrefab.GetComponent<TopicsButton>());
        print("Added " + NewButtonPrefab.GetComponent<TopicsButton>() + "Status: " + NewButtonPrefab.GetComponent<TopicsButton>().Active);
    }

    public List<TopicsButton> GetTopicButtonsList()
    {
        return topicButtons;
    }

    public void RemoveTopicButtonFromList(TopicsButton _topicButton)
    {
        foreach (TopicsButton item in topicButtons)
        {
            if (topicButtons.Contains(_topicButton) && item.Equals(_topicButton))
            {
                topicButtons.Remove(item);
            }
        }
    }
}