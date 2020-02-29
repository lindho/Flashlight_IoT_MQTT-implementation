using UnityEngine;

public class AndroidCam : MonoBehaviour
{

    AndroidJavaObject camera1 = null;

    private void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        Debug.Log("Camera Name:" + devices[0].name);

        Open();
    }

    public void Open()
    {
        if (camera1 == null)
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
			AndroidJavaClass cameraClass = new AndroidJavaClass("android.hardware.Camera");
			AndriodCamera = cameraClass.CallStatic<AndroidJavaObject>("open");
#endif
        }
    }

    public void Release()
    {
        if (camera1 != null)
        {
            LEDOff();

            camera1.Call("release");
            camera1 = null;
        }
    }

    public void StartPreview()
    {
        if (camera1 != null)
        {
            Debug.Log("AndroidCamera::startPreview()");
            LEDOn();
            camera1.Call("startPreview");
        }
    }

    public void StopPreview()
    {
        if (camera1 != null)
        {
            Debug.Log("AndroidCamera::stopPreview()");
            LEDOff();
            camera1.Call("stopPreview");
        }
    }

    private void SetFlashMode(string mode)
    {
        if (camera1 != null)
        {
            AndroidJavaObject cameraParameters = camera1.Call<AndroidJavaObject>("getParameters");
            cameraParameters.Call("setFlashMode", mode);
            camera1.Call("setParameters", cameraParameters);
        }
    }

    public void LEDOn()
    {
        SetFlashMode("torch");
    }

    public void LEDOff()
    {
        SetFlashMode("off");
    }
}
