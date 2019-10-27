using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(BrokerConnection))]
public class CubeEditor : Editor
{

    private GameObject buttonPrefab;
    

    void Start()
    {
        buttonPrefab = (GameObject)Resources.Load("Assets/Resources/Test", typeof(GameObject));
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BrokerConnection connection = (BrokerConnection)target;

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Generate Button"))
        {
            Instantiate(buttonPrefab, connection.transform);
        }

        if (GUILayout.Button("Reset"))
        {
            //do something
        }

        GUILayout.EndHorizontal();
    }

}
