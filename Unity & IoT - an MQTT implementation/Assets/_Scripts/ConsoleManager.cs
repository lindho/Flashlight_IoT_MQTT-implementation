using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleManager : MonoBehaviour
{
    public static ConsoleManager Instance = null;

    [Header("Connected to label")]
    public Text topicsLabel;

    [Header("Input Field")]
    public InputField console;

    [Header("Panel object")]
    public Image headerPanel;

    public TopicButtonSpawner topicButtonSpawner;

    private int backgroundAlphaValue = 80;
    private string connectedToText;
    private string noConnectionText = "\"Not connected\"";

    private void Awake()
    {
        SingletonCheck();
    }

    private void SingletonCheck()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        //SetBgColorAndText();
    }


    //public void SetBgColorAndText()
    //{
    //    string text = "";
    //    bool atLeastOneIsActive = false;
    //    foreach (TopicsButton topicsButton in topicButtonSpawner.GetTopicButtonsList())
    //    {
    //        if (!topicsButton.Active)
    //        {
    //            atLeastOneIsActive = true;
    //            headerPanel.color = new Color(0, 255, 0, backgroundAlphaValue);
    //            text += topicsButton.GetButtonText() + ", ";
    //        }
    //        else
    //        {
    //            headerPanel.color = new Color(255, 0, 0, backgroundAlphaValue);
    //            topicsLabel.text = "Connected To: " + noConnectionText;
    //        }
    //    }
    //    if (atLeastOneIsActive)
    //    {
    //        topicsLabel.text = (connectedToText += text);
    //        atLeastOneIsActive = false;
    //    }
    //}
}
