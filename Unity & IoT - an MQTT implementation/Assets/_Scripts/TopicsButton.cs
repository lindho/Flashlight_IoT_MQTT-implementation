using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopicsButton : MonoBehaviour
{

    private BrokerConnection IPconfigScript;
    private InputTopicField inputTopicField;
    private Text ButtonText;
    private bool Active;

    void Start()
    {
        inputTopicField = FindObjectOfType<InputTopicField>();
        Active = true;
        IPconfigScript = FindObjectOfType<BrokerConnection>();
        ButtonText = this.gameObject.GetComponentInChildren<Text>();
    }

    public void OnButtonPress()
    {
        if (Active)
        {
            IPconfigScript.Unsubscribe(ButtonText.text);
            ButtonText.color = Color.red;
        }
        else
        {
            //TODO fixa detta på något sätt
            IPconfigScript.Subscribe(ButtonText.text, inputTopicField.QosLevel);
            ButtonText.color = Color.green;
        }
        Active = !Active;
    }

    public void UnsubscribeAndDelete()
    {
        IPconfigScript.RemoveTopicFromList(ButtonText.text);
        IPconfigScript.Unsubscribe(ButtonText.text);
        StartCoroutine(DelayDelete());
    }

    private IEnumerator DelayDelete()
    {
        yield return new WaitForSeconds(.3f);
        Destroy(gameObject);
    }
}
