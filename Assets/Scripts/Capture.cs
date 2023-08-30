using System;
using System.IO;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using DoubleComputation;

public class Capture : MonoBehaviour
{

    private Texture2D camTexture;

    [SerializeField] public Camera _camera;

    [SerializeField] public ARSessionOrigin arSessionOrigin;

    [SerializeField] public Text log;

    public ARCameraManager cameraManager;

    private int count;

    private string time_name = System.DateTime.Now.ToString("s");

    private string ARKitFilePath;
    private string GPSFilePath;
    private string MagFilePath;
    private string GyroFilePath;
    private string AcceFilePath;

    bool locationIsReady = false;
    bool locationGrantedAndroid = false;
    public Vector3d gps_device;
    private float angleToNorth;
    private Gyroscope gyro;

    void Start()
    {
        cameraManager = GameObject.Find("AR Camera").GetComponent<ARCameraManager>();

        Application.lowMemory += OnLowMemory;

        time_name = System.DateTime.Now.ToString("s").Replace(":", "_");
        Directory.CreateDirectory(Application.persistentDataPath + "/" + time_name);
        //Debug.Log(time_name);

        ARKitFilePath = Application.persistentDataPath + "/" + time_name + "/" + "ARkitPose" + ".txt";
        if (!File.Exists(ARKitFilePath))
        {
            File.CreateText(ARKitFilePath);
        }
        GPSFilePath = Application.persistentDataPath + "/" + time_name + "/" + "GPSData" + ".txt";
        if (!File.Exists(GPSFilePath))
        {
            File.CreateText(GPSFilePath);
        }
        MagFilePath = Application.persistentDataPath + "/" + time_name + "/" + "MagnetometerData" + ".txt";
        if (!File.Exists(MagFilePath))
        {
            File.CreateText(MagFilePath);
        }
        GyroFilePath = Application.persistentDataPath + "/" + time_name + "/" + "GyroscopeData" + ".txt";
        if (!File.Exists(GyroFilePath))
        {
            File.CreateText(GyroFilePath);
        }
        AcceFilePath = Application.persistentDataPath + "/" + time_name + "/" + "AccelerometerData" + ".txt";
        if (!File.Exists(AcceFilePath))
        {
            File.CreateText(AcceFilePath);
        }

#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        else
        {
            locationGrantedAndroid = true;
            locationIsReady = NativeGPSPlugin.StartLocation();
        }

#elif PLATFORM_IOS

        locationIsReady = NativeGPSPlugin.StartLocation();

#endif

        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }

        Input.compass.enabled = true;
    }

    private void OnLowMemory()
    {
        // release all cached textures
        Resources.UnloadUnusedAssets();
    }

    unsafe void FixedUpdate()
    {
        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            return;

        float pos_x = _camera.transform.position.x;
        float pos_y = _camera.transform.position.y;
        float pos_z = _camera.transform.position.z;
        float ori_w = _camera.transform.rotation.w;
        float ori_x = _camera.transform.rotation.x;
        float ori_y = _camera.transform.rotation.y;
        float ori_z = _camera.transform.rotation.z;

        var conversionParams = new XRCpuImage.ConversionParams
        {
            // Get the entire image.
            inputRect = new RectInt(0, 0, image.width, image.height),

            outputDimensions = new Vector2Int(image.width, image.height),

            // Choose RGBA format.
            outputFormat = TextureFormat.RGBA32,

            transformation = XRCpuImage.Transformation.MirrorY

        };

        // See how many bytes you need to store the final image.
        int size = image.GetConvertedDataSize(conversionParams);

        // Allocate a buffer to store the image.
        var buffer = new NativeArray<byte>(size, Allocator.Temp);

        // Extract the image data
        image.Convert(conversionParams, new IntPtr(buffer.GetUnsafePtr()), buffer.Length);

        // The image was converted to RGBA32 format and written into the provided buffer
        // so you can dispose of the XRCpuImage. You must do this or it will leak resources.
        image.Dispose();


        // At this point, you can process the image, pass it to a computer vision algorithm, etc.
        // In this example, you apply it to a texture to visualize it.

        // You've got the data; let's put it into a texture so you can visualize it.
        camTexture = new Texture2D(
            conversionParams.outputDimensions.x,
            conversionParams.outputDimensions.y,
            conversionParams.outputFormat,
            false);

        camTexture.LoadRawTextureData(buffer);
        camTexture.Apply();
        var bytes = camTexture.EncodeToJPG();

        string path = String.Format(Application.persistentDataPath + "/" + time_name + "/" + "image_{0}" + ".jpg", count.ToString().PadLeft(4,'0'));
        File.WriteAllBytes(path, bytes);

        // orgTexture = rotateTexture(orgTexture,false);

        using (StreamWriter sr = File.AppendText(ARKitFilePath))
        {
            sr.Write("image_{0}" + ".jpg ", count.ToString().PadLeft(4, '0'));
            sr.WriteLine("{0} {1} {2} {3} {4} {5} {6}", pos_x, pos_y, pos_z, ori_w, ori_x, ori_y, ori_z);
        }

        gps_device = new Vector3d(NativeGPSPlugin.GetLatitude(), NativeGPSPlugin.GetLongitude(), NativeGPSPlugin.GetAltitude());
        using (StreamWriter sa = File.AppendText(GPSFilePath))
        {
            sa.Write("image_{0}" + ".jpg ", count.ToString().PadLeft(4, '0'));
            sa.WriteLine("{0} {1} {2}", gps_device.x, gps_device.y, gps_device.z);
        }

        angleToNorth = Input.compass.trueHeading;
        using (StreamWriter sb = File.AppendText(MagFilePath))
        {
            sb.Write("image_{0}" + ".jpg ", count.ToString().PadLeft(4, '0'));
            sb.WriteLine("{0}", angleToNorth);
        }

        Quaternion gyroAttitude = Input.gyro.attitude;
        using (StreamWriter sc = File.AppendText(GyroFilePath))
        {
            sc.Write("image_{0}" + ".jpg ", count.ToString().PadLeft(4, '0'));
            sc.WriteLine("{0} {1} {2} {3}", gyroAttitude.w, gyroAttitude.x, gyroAttitude.y, gyroAttitude.z);
        }

        using (StreamWriter sd = File.AppendText(AcceFilePath))
        {
            sd.Write("image_{0}" + ".jpg ", count.ToString().PadLeft(4, '0'));
            sd.WriteLine("{0} {1} {2}", Input.acceleration.x, Input.acceleration.y, Input.acceleration.z);
        }

        count = count + 1;
        log.text = "Time: " + count.ToString() + "\n";
    }



}
