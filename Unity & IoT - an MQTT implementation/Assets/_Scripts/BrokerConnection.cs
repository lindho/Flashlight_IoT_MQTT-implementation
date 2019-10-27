using UnityEngine;
using System.Net;
using uPLibrary.Networking.M2Mqtt.Messages;
using System;
using System.Net.Sockets;


/// <summary>
/// Script to handle the connection to the free HiveMQ MQTT broker using the <see cref="MqttClientExtension.class"/>
/// Can only be attached to a single game object in the game scene
/// </summary>
public class BrokerConnection : MonoBehaviour
{

    public static BrokerConnection Instance = null;

    [Header("HIVE MQ BROKER")]
    public bool _useHiveMqBroker;
    [Header("Manual ip address input (can be empty if 'use HiveMq broker' is checked)")]
    public string brokerIpAddress;
    public MqttClientExtension client;

    [HideInInspector] public string[] topics;

    [Header("Animator located in 'BottomBarPanel' game object")]
    public Animator animator;

    //Member variables
    private string clientId;
    private bool Active;
    private AndroidJavaObject camera1;

    /// <summary>
    /// <description>
    /// Sets the client Identification to a globally unique identifier as a string.
    /// Sets broker IP address to http://www.mqtt-dashboard.com if <see cref="_useHiveMqBroker"/> is checked in inspector
    /// Populates the topic list
    /// Sets up the client
    /// </description>
    /// </summary>
    private void Awake()
    {
        SingletonCheck();
        clientId = Guid.NewGuid().ToString();
        if (_useHiveMqBroker)
            brokerIpAddress = getHostByName();
        topics = new string[] { "phone/myflashlight" };
        client = new MqttClientExtension(IPAddress.Parse(brokerIpAddress), 1883, false, null);
    }

    /// <summary>
    /// <description>Function that only allows one intance of this script to exist in the game scene</description>
    /// </summary>
    public void SingletonCheck()
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

    /// <summary>
    /// <description> Invokes <see cref="DelayedAttribute"/> after 2 seconds </description>
    /// </summary>
    void Start()
    {
        Invoke("DelayedSetup", 2);
    }

    /// <summary>
    /// <description>Connects the client to the broker and subscribes to the specified topics listed in the dropdown menu with Qos Level 2</description>       
    /// </summary>
    private void DelayedSetup()
    {
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
        client.Connect(clientId);
        client.Subscribe(new string[] { topics[0] }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        animator.SetTrigger("slide");
    }

    /// <summary>
    /// Funtion to handle message specific actions
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e">the message</param>
    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message));

        if (System.Text.Encoding.UTF8.GetString(e.Message) == "Hej")
        {
            client.Publish(topics[0], System.Text.Encoding.UTF8.GetBytes("Sending from Unity3D!!!"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }
#if (UNITY_ANDROID )
        else if (System.Text.Encoding.UTF8.GetString(e.Message) == "flashlight on" && Application.platform == RuntimePlatform.Android)
        {
            Flashlight_Start();

        }
        else if (System.Text.Encoding.UTF8.GetString(e.Message) == "flashlight off")
        {
            Flashlight_Stop();
        }
#endif

        else if (System.Text.Encoding.UTF8.GetString(e.Message) == "flashlight active" && Application.platform == RuntimePlatform.Android)
        {
            Active = true;
        }
        else if (System.Text.Encoding.UTF8.GetString(e.Message) == "flashlight not active" && Application.platform == RuntimePlatform.Android)
        {
            Active = false;
        }
    }

    void Flashlight_Start()
    {
        int camID = 0;
        AndroidJavaClass cameraClass = new AndroidJavaClass("android.hardware.Camera");
        WebCamDevice[] devices = WebCamTexture.devices;

        //New
        AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.hardware.Camera", camID);

        camera1 = cameraClass.CallStatic<AndroidJavaObject>("open", camID);


        if (camera1 != null)
        {
            AndroidJavaObject cameraParameters = camera1.Call<AndroidJavaObject>("getParameters");
            cameraParameters.Call("setFlashMode", "torch");
            camera1.Call("setParameters", cameraParameters);
            camera1.Call("startPreview");
            //Active = true;
            client.Publish(topics[0], System.Text.Encoding.UTF8.GetBytes("flashlight active"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        else
        {
            Debug.LogError("[CameraParametersAndroid] Camera not available");
        }
    }

    void Flashlight_Stop()
    {
        if (camera1 != null)
        {
            camera1.Call("stopPreview");
            camera1.Call("release");
            //Active = false;
            client.Publish(topics[0], System.Text.Encoding.UTF8.GetBytes("flashlight not active"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        else
        {
            Debug.LogError("[CameraParametersAndroid] Camera not available");
        }
    }

    void OnApplicationQuit()
    {
        client.Disconnect();
    }

    public void Subscribe(string topicName, byte qosLevel)
    {
        client.Subscribe(new string[] { topicName }, new byte[] { qosLevel });
    }

    public void Unsubscribe(string topicName)
    {
        client.Unsubscribe(new string[] { topicName });
    }

    public void RemoveTopicFromList(string topicName)
    {
        client.RemoveButton(topicName);
    }

    public void SendMessagetoTopic(string message, string messageTopic, byte qosLevel)
    {
        client.Publish(messageTopic, System.Text.Encoding.UTF8.GetBytes(message), qosLevel, false);
    }


    /// <summary>
    /// Automatically get the IP address of the broker
    /// </summary>
    /// <returns>return the ip address as a string</returns>
    public static string getHostByName()
    {
        try
        {
            IPHostEntry Hosts = Dns.GetHostEntry("mqtt-dashboard.com");
            return Hosts.AddressList[0].ToString();
        }

        catch (SocketException e)
        {
            Debug.LogError("SocketException caught!!!");
            Debug.LogError("Source : " + e.Source);
            Debug.LogError("Message : " + e.Message);
        }
        catch (ArgumentNullException e)
        {
            Debug.LogError("ArgumentNullException caught!!!");
            Debug.LogError("Source : " + e.Source);
            Debug.LogError("Message : " + e.Message);
        }
        catch (Exception e)
        {
            Debug.LogError("Exception caught!!!");
            Debug.LogError("Source : " + e.Source);
            Debug.LogError("Message : " + e.Message);
        }
        return "";
    }

    /// <summary>
    /// Function to refresh the ip address
    /// </summary>
    public void UpdateIPAddress() //Not currently used in this project
    {
        brokerIpAddress = getHostByName();
    }


    /// <summary>
    /// Static call to andriod camera
    /// </summary>
    /// <param name="javaObject"></param>
    /// <param name="methodName"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string safeCallStringMethod(AndroidJavaObject javaObject, string methodName, params object[] args)
    {
#if UNITY_2018_2_OR_NEWER
        if (args == null) args = new object[] { null };
        IntPtr methodID = AndroidJNIHelper.GetMethodID<string>(javaObject.GetRawClass(), methodName, args, false);
        jvalue[] jniArgs = AndroidJNIHelper.CreateJNIArgArray(args);

        try
        {
            IntPtr returnValue = AndroidJNI.CallObjectMethod(javaObject.GetRawObject(), methodID, jniArgs);
            if (IntPtr.Zero != returnValue)
            {
                var val = AndroidJNI.GetStringUTFChars(returnValue);
                AndroidJNI.DeleteLocalRef(returnValue);
                return val;
            }
        }
        finally
        {
            AndroidJNIHelper.DeleteJNIArgArray(args, jniArgs);
        }

        return null;
#else
            return  javaObject.Call<string>(methodName, args);
#endif
    }

}
