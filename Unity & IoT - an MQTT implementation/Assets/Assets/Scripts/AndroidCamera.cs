using UnityEngine;

public class AndroidCamera 
{

    AndroidJavaObject camera = null;
    AndroidJavaObject cameraManager = null;


    public AndroidCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        Debug.Log("Camera Name:" + devices[0].name);

        open();
    }

    public void open()
    {
        if (camera == null)
        {
#if (UNITY_ANDROID)
			AndroidJavaClass cameraClass = new AndroidJavaClass("android.hardware.camera2");
			//camera = cameraClass.CallStatic<AndroidJavaObject>("open");
            cameraManager = cameraClass.CallStatic<AndroidJavaClass>("CameraManager");

#endif
        }
    }

    public void release()
    {
        if (camera != null)
        {
            LEDOff();

            camera.Call("release");
            camera = null;
        }
    }

    public void StartPreview()
    {
        if (camera != null)
        {
            cameraManager.CallStatic<AndroidJavaClass>("setTorchMode", "1", true);
            //Debug.Log("AndroidCamera::startPreview()");
            //LEDOn();
            //camera.Call("startPreview");
        }
    }

    public void StopPreview()
    {
        if (camera != null)
        {
            cameraManager.CallStatic<AndroidJavaClass>("setTorchMode", "1", false);
            //Debug.Log("AndroidCamera::stopPreview()");
            //LEDOff();
            //camera.Call("stopPreview");
        }
    }

    private void setFlashMode(string mode)
    {
        if (camera != null)
        {
            AndroidJavaObject cameraParameters = camera.Call<AndroidJavaObject>("getParameters");
            cameraParameters.Call("setFlashMode", mode);
            camera.Call("setParameters", cameraParameters);
        }
    }

    public void LEDOn()
    {
        setFlashMode("torch");
    }

    public void LEDOff()
    {
        setFlashMode("off");
    }
}