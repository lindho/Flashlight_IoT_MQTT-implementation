using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopicsButton : MonoBehaviour
{
    private InputTopicField inputTopicField;
    private Text ButtonText;
    public bool Active { get; private set; }
    private TopicButtonSpawner topicButtonSpawner;

    void Start()
    {
        inputTopicField = FindObjectOfType<InputTopicField>();
        Active = true;
        ButtonText = gameObject.GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        topicButtonSpawner = GetComponentInParent<TopicButtonSpawner>();
    }

    public void OnButtonPress()
    {
        if (Active)
        {
            BrokerConnection.Instance.Unsubscribe(ButtonText.text);
            ButtonText.color = Color.red;
        }
        else
        {
            BrokerConnection.Instance.Subscribe(ButtonText.text, inputTopicField.QosLevel);
            ButtonText.color = Color.green;
        }
        Active = !Active;
    }

    public void UnsubscribeAndDelete()
    {
        BrokerConnection.Instance.RemoveTopicFromList(ButtonText.text);
        BrokerConnection.Instance.Unsubscribe(ButtonText.text);
        topicButtonSpawner.RemoveTopicButtonFromList(this);
        StartCoroutine(DelayDelete());
    }

    private IEnumerator DelayDelete()
    {
        yield return new WaitForSeconds(.3f);
        Destroy(gameObject);
    }

    public string GetButtonText()
    {
        return ButtonText.text;
    }
}
