using UnityEngine;
public class AndroidCamera : MonoBehaviour
{
    private AndroidJavaClass unityClass = null;
    private AndroidJavaObject unityActivity = null;
    private AndroidJavaObject unityContext = null;
    private AndroidJavaClass customClass = null;
    private readonly string CAMERA_ID = "0";

#if (UNITY_ANDROID && !UNITY_EDITOR)
    private void Start()
    {
        SendActivityReference("com.example.iotflashlightcontroller.Flashlight");
    }
#endif

    private void SendActivityReference(string packageName)
    {
        unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        unityContext = unityActivity.Call<AndroidJavaObject>("getApplicationContext");

        customClass = new AndroidJavaClass(packageName);
        customClass.CallStatic("receiveContextInstance", unityContext);
    }

    public bool EnableFlash()
    {
        return customClass.CallStatic<bool>("enableFlash", CAMERA_ID);
    }

    public bool StopFlash()
    {
        return customClass.CallStatic<bool>("stopFlash", CAMERA_ID);
    }
}