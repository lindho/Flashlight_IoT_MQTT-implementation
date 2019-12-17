using UnityEngine;

public class ExitClickHandler : MonoBehaviour {

    public void ExitProgram() {
#if !UNITY_EDITOR
        Application.Quit();
#else 
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}