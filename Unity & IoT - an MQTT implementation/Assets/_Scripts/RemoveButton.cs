using UnityEngine;

public class RemoveButton : MonoBehaviour {

    public TopicsButton tb;
    //public GameObject topicsButton;

    public void Remove() {
        tb.UnsubscribeAndDelete();
    }
}
