﻿


using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OpenNI2
{

    public class OpenNI2Wrapper 
    {
        public static int ONI_MAX_STR = 256;
        [Flags]
        public enum OniStatus : uint
        {

            ONI_STATUS_OK = 0,
            ONI_STATUS_ERROR = 1,
            ONI_STATUS_NOT_IMPLEMENTED = 2,
            ONI_STATUS_NOT_SUPPORTED = 3,
            ONI_STATUS_BAD_PARAMETER = 4,
            ONI_STATUS_OUT_OF_FLOW = 5,
            ONI_STATUS_NO_DEVICE = 6,
            ONI_STATUS_TIME_OUT = 102,
        }
        public enum OniSensorType : uint            
        {
	        ONI_SENSOR_IR = 1,
	        ONI_SENSOR_COLOR = 2,
	        ONI_SENSOR_DEPTH = 3,
        }

    public enum OniPixelFormat : uint
    {
	    // Depth
	    ONI_PIXEL_FORMAT_DEPTH_1_MM = 100,
	    ONI_PIXEL_FORMAT_DEPTH_100_UM = 101,
	    ONI_PIXEL_FORMAT_SHIFT_9_2 = 102,
	    ONI_PIXEL_FORMAT_SHIFT_9_3 = 103,

	    // Color
	    ONI_PIXEL_FORMAT_RGB888 = 200,
	    ONI_PIXEL_FORMAT_YUV422 = 201,
	    ONI_PIXEL_FORMAT_GRAY8 = 202,
	    ONI_PIXEL_FORMAT_GRAY16 = 203,
	    ONI_PIXEL_FORMAT_JPEG = 204,
    } 

    public enum OniDeviceState : uint
    {
	    ONI_DEVICE_STATE_OK 		= 0,
	    ONI_DEVICE_STATE_ERROR		= 1,
	    ONI_DEVICE_STATE_NOT_READY 	= 2,
	    ONI_DEVICE_STATE_EOF 		= 3
    }

        
    public enum OniImageRegistrationMode : uint {
	    ONI_IMAGE_REGISTRATION_OFF				= 0,
	    ONI_IMAGE_REGISTRATION_DEPTH_TO_COLOR	= 1,
    }

    public struct OniVideoMode
    {
	    OniPixelFormat pixelFormat;
	    int resolutionX;
	    int resolutionY;
	    int fps;
    } 

    [StructLayout(LayoutKind.Sequential)]
    public struct OniSensorInfo
    {
    	OniSensorType sensorType;
    	int numSupportedVideoModes;        
        IntPtr pSupportedVideoModes; //OniVideoMode* pSupportedVideoModes;
    } 

    [StructLayout(LayoutKind.Sequential)]
    public struct OniDeviceInfo
    {        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	    public char [] uri;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	    public char [] vendor;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	    public char [] name;
	    UInt16 usbVendorId;
	    UInt16 usbProductId;
    }


    [StructLayout(LayoutKind.Sequential)] 
    public struct OniVersion
    {		      																			
																								
	    /** Major version number, incremented for major API restructuring. */						
	    public int major;																					
	    /** Minor version number, incremented when signficant new features added. */
        public int minor;																					
	    /** Mainenance build number, incremented for new releases that primarily provide minor bug fixes. */
        public int maintenance;																			
	    /** Build number. Incremented for each new API build. Generally not shown on the installer and download site. */
        public int build;																					
    }

    [StructLayout(LayoutKind.Sequential)] 
    public struct OniFrame  
    {
	    public int dataSize;
	    public IntPtr data;

        public OniSensorType sensorType;
        public UInt64 timestamp;
        public int frameIndex;

        public int width;
        public int height;

        public OniVideoMode videoMode;
        public bool croppingEnabled;
        public int cropOriginX;
        public int cropOriginY;

        public int stride;
    }

    public delegate void OniDeviceInfoCallback(ref OniDeviceInfo info, IntPtr pCookie);
    public delegate void OniDeviceStateCallback(ref OniDeviceInfo info, OniDeviceState deviceState, IntPtr pCookie);

    //[MarshalAs(UnmanagedType.FunctionPtr)]
    //public OniDeviceInfoCallback		deviceConnected;
    //[MarshalAs(UnmanagedType.FunctionPtr)]
    //public OniDeviceInfoCallback		deviceDisconnected;
    //[MarshalAs(UnmanagedType.FunctionPtr)]
    //public OniDeviceStateCallback		deviceStateChanged;
       
    [StructLayout(LayoutKind.Sequential)]
    public struct OniDeviceCallbacks
    {
        public IntPtr deviceConnected;
        public IntPtr deviceDisconnected;
        public IntPtr deviceStateChanged;
    } 
//Bindings to OpenNI2 based on OniCAPI.h

        /**  Initialize OpenNI2. Use ONI_API_VERSION as the version. */
        //ONI_C_API OniStatus oniInitialize(int apiVersion);
        [DllImport("OpenNI2.dll")]
        public static extern OniStatus oniInitialize(int apiVersion);
        
        /**  Shutdown OpenNI2 */
        //ONI_C_API void oniShutdown();
        [DllImport("OpenNI2.dll")]
        public static extern void oniShutdown();
   
         /**
         * Get the list of currently connected device.
         * Each device is represented by its OniDeviceInfo.
         * pDevices will be allocated inside.
         */
        //ONI_C_API OniStatus oniGetDeviceList(OniDeviceInfo** pDevices, int* pNumDevices);
        [DllImport("OpenNI2.dll")]
        public static extern OniStatus oniGetDeviceList(ref IntPtr pDevices, ref int pNumDevices);


        /** Release previously allocated device list */
        //ONI_C_API OniStatus oniReleaseDeviceList(OniDeviceInfo* pDevices);
        [DllImport("OpenNI2.dll")]
        public static extern OniStatus oniReleaseDeviceList(IntPtr pDevices);

        
        
        //ONI_C_API OniStatus oniRegisterDeviceCallbacks(OniDeviceCallbacks* pCallbacks, void* pCookie, OniCallbackHandle* pHandle);
        [DllImport("OpenNI2.dll")]
        public static extern OniStatus oniRegisterDeviceCallbacks(IntPtr callbacks, IntPtr pCookie, out IntPtr pHandle);

        //ONI_C_API void oniUnregisterDeviceCallbacks(OniCallbackHandle handle);
        [DllImport("OpenNI2.dll")]
        public static extern void oniUnregisterDeviceCallbacks(IntPtr pHandle);

        ///** Wait for any of the streams to have a new frame */
        //ONI_C_API OniStatus oniWaitForAnyStream(OniStreamHandle* pStreams, int numStreams, int* pStreamIndex, int timeout);
        [DllImport("OpenNI2.dll")]
        public static extern OniStatus oniWaitForAnyStream(ref IntPtr pHandle, int numStreams, ref int pStreamIndex, int timeout);

        ///** Get the current version of OpenNI2 */
        //ONI_C_API OniVersion oniGetVersion();
        [DllImport("OpenNI2.dll")]
        public static extern OniVersion oniGetVersion();

        ///** Translate from format to number of bytes per pixel. Will return 0 for formats in which the number of bytes per pixel isn't fixed. */
        //ONI_C_API int oniFormatBytesPerPixel(OniPixelFormat format);
        [DllImport("OpenNI2.dll")]
        public static extern int oniFormatBytesPerPixel(OniPixelFormat format);

        ///** Get internal error */
        //ONI_C_API const char* oniGetExtendedError();
        [DllImport("OpenNI2.dll")]
        [return : MarshalAs(UnmanagedType.LPStr)]
        public static extern string oniGetExtendedError();

        /** Open a device. Uri can be taken from the matching OniDeviceInfo. */
        [DllImport("OpenNI2.dll")]
        public static extern OniStatus oniDeviceOpen( string uri, ref IntPtr pDevice);

        /** Close a device */
        [DllImport("OpenNI2.dll")]
        public static extern OniStatus oniDeviceClose(IntPtr device);

        /** Get the possible configurations available for a specific source, or NULL if the source does not exist. */
        //ONI_C_API const OniSensorInfo* oniDeviceGetSensorInfo(OniDeviceHandle device, OniSensorType sensorType);
        [DllImport("OpenNI2.dll")]
        public static extern IntPtr oniDeviceGetSensorInfo(IntPtr device, OniSensorType sensorType);

        ///** Get the OniDeviceInfo of a certain device. */
        //ONI_C_API OniStatus oniDeviceGetInfo(OniDeviceHandle device, OniDeviceInfo* pInfo);
        [DllImport("OpenNI2.dll")]
        public static extern OniStatus oniDeviceGetInfo(IntPtr device, ref OniDeviceInfo pInfo);   

        /** Create a new stream in the device. The stream will originate from the source. */
        //ONI_C_API OniStatus oniDeviceCreateStream(OniDeviceHandle device, OniSensorType sensorType, OniStreamHandle* pStream);
        [DllImport("OpenNI2.dll")]
        public static extern OniStatus oniDeviceCreateStream(IntPtr device, OniSensorType sensorType, ref IntPtr pStream);
   
        //ONI_C_API OniStatus oniDeviceEnableDepthColorSync(OniDeviceHandle device);
        [DllImport("OpenNI2.dll")]
        public static extern OniStatus oniDeviceEnableDepthColorSync(IntPtr device);   
        //ONI_C_API void oniDeviceDisableDepthColorSync(OniDeviceHandle device);
        [DllImport("OpenNI2.dll")]
        public static extern void oniDeviceDisableDepthColorSync(IntPtr device);

/** Set property in the device. Use the properties listed in OniTypes.h: ONI_DEVICE_PROPERTY_..., or specific ones supplied by the device. */
//ONI_C_API OniStatus oniDeviceSetProperty(OniDeviceHandle device, int propertyId, const void* data, int dataSize);
///** Get property in the device. Use the properties listed in OniTypes.h: ONI_DEVICE_PROPERTY_..., or specific ones supplied by the device. */
//ONI_C_API OniStatus oniDeviceGetProperty(OniDeviceHandle device, int propertyId, void* data, int* pDataSize);
///** Check if the property is supported by the device. Use the properties listed in OniTypes.h: ONI_DEVICE_PROPERTY_..., or specific ones supplied by the device. */
//ONI_C_API OniBool oniDeviceIsPropertySupported(OniDeviceHandle device, int propertyId);
///** Invoke an internal functionality of the device. */
//ONI_C_API OniStatus oniDeviceInvoke(OniDeviceHandle device, int commandId, const void* data, int dataSize);
///** Check if a command is supported, for invoke */
//ONI_C_API OniBool oniDeviceIsCommandSupported(OniDeviceHandle device, int commandId);

//ONI_C_API OniBool oniDeviceIsImageRegistrationModeSupported(OniDeviceHandle device, OniImageRegistrationMode mode);
        [DllImport("OpenNI2.dll")]
        public static extern bool oniDeviceIsImageRegistrationModeSupported(IntPtr device, OniImageRegistrationMode mode);

        /** Destroy an existing stream */
        [DllImport("OpenNI2.dll")]
        public static extern void oniStreamDestroy(IntPtr stream);
        
        /** Get the OniSourceInfo of the certain stream. */
        //ONI_C_API const OniSensorInfo* oniStreamGetSensorInfo(OniStreamHandle stream);
        [DllImport("OpenNI2.dll")]
        public static extern IntPtr oniStreamGetSensorInfo(IntPtr stream);

        /** Start generating data from the stream. */
        //ONI_C_API OniStatus oniStreamStart(OniStreamHandle stream);
        [DllImport("OpenNI2.dll")]
        public static extern OniStatus oniStreamStart(IntPtr stream);

        /** Stop generating data from the stream. */
        //ONI_C_API void oniStreamStop(OniStreamHandle stream);        
        [DllImport("OpenNI2.dll")]
        public static extern void oniStreamStop(IntPtr stream);



        ///** Get the next frame from the stream. This function is blocking until there is a new frame from the stream. For timeout, use oniWaitForStreams() first */
        //ONI_C_API OniStatus oniStreamReadFrame(OniStreamHandle stream, OniFrame** pFrame);
        [DllImport("OpenNI2.dll")]
        public static extern OniStatus oniStreamReadFrame(IntPtr stream, ref IntPtr pFrame);

        ///** Register a callback to when the stream has a new frame. */
        //ONI_C_API OniStatus oniStreamRegisterNewFrameCallback(OniStreamHandle stream, OniNewFrameCallback handler, void* pCookie, OniCallbackHandle* pHandle);
        [DllImport("OpenNI2.dll")]
        public static extern OniStatus oniStreamRegisterNewFrameCallback(IntPtr stream, IntPtr handler, IntPtr pCookie, out IntPtr pHandle);

///** Unregister a previously registered callback to when the stream has a new frame. */
//ONI_C_API void oniStreamUnregisterNewFrameCallback(OniStreamHandle stream, OniCallbackHandle handle);
        [DllImport("OpenNI2.dll")]
        public static extern void oniStreamUnregisterNewFrameCallback(IntPtr stream, IntPtr handle);

///** Set property in the stream. Use the properties listed in OniTypes.h: ONI_STREAM_PROPERTY_..., or specific ones supplied by the device for its streams. */
//ONI_C_API OniStatus oniStreamSetProperty(OniStreamHandle stream, int propertyId, const void* data, int dataSize);
///** Get property in the stream. Use the properties listed in OniTypes.h: ONI_STREAM_PROPERTY_..., or specific ones supplied by the device for its streams. */
//ONI_C_API OniStatus oniStreamGetProperty(OniStreamHandle stream, int propertyId, void* data, int* pDataSize);
///** Check if the property is supported the stream. Use the properties listed in OniTypes.h: ONI_STREAM_PROPERTY_..., or specific ones supplied by the device for its streams. */
//ONI_C_API OniBool oniStreamIsPropertySupported(OniStreamHandle stream, int propertyId);
///** Invoke an internal functionality of the stream. */
//ONI_C_API OniStatus oniStreamInvoke(OniStreamHandle stream, int commandId, const void* data, int dataSize);
///** Check if a command is supported, for invoke */
//ONI_C_API OniBool oniStreamIsCommandSupported(OniStreamHandle stream, int commandId);
//// handle registration of pixel

//////
///** Mark another user of the frame. */
//ONI_C_API void oniFrameAddRef(OniFrame* pFrame);
        [DllImport("OpenNI2.dll")]
        public static extern void oniFrameAddRef(ref OniFrame pFrame);

///** Mark that the frame is no longer needed.  */
//ONI_C_API void oniFrameRelease(OniFrame* pFrame);
        [DllImport("OpenNI2.dll")]
        public static extern void oniFrameRelease(ref OniFrame pFrame);



//// ONI_C_API OniStatus oniConvertRealWorldToProjective(OniStreamHandle stream, OniFloatPoint3D* pRealWorldPoint, OniFloatPoint3D* pProjectivePoint);
//// ONI_C_API OniStatus oniConvertProjectiveToRealWorld(OniStreamHandle stream, OniFloatPoint3D* pProjectivePoint, OniFloatPoint3D* pRealWorldPoint);

///**
// * Creates a recorder that records to a file.
// * @param	[in]	fileName	The name of the file that will contain the recording.
// * @param	[out]	pRecorder	Points to the handle to the newly created recorder.
// * @retval ONI_STATUS_OK Upon successful completion.
// * @retval ONI_STATUS_ERROR Upon any kind of failure.
// */
//ONI_C_API OniStatus oniCreateRecorder(const char* fileName, OniRecorderHandle* pRecorder);

///**
// * Attaches a stream to a recorder. The amount of attached streams is virtually
// * infinite. You cannot attach a stream after you have started a recording, if
// * you do: an error will be returned by oniRecorderAttachStream.
// * @param	[in]	recorder				The handle to the recorder.
// * @param	[in]	stream					The handle to the stream.
// * @param	[in]	allowLossyCompression	Allows/denies lossy compression
// * @retval ONI_STATUS_OK Upon successful completion.
// * @retval ONI_STATUS_ERROR Upon any kind of failure.
// */
//ONI_C_API OniStatus oniRecorderAttachStream(
//        OniRecorderHandle   recorder, 
//        OniStreamHandle     stream, 
//        OniBool             allowLossyCompression);

///**
// * Starts recording. There must be at least one stream attached to the recorder,
// * if not: oniRecorderStart will return an error.
// * @param[in] recorder The handle to the recorder.
// * @retval ONI_STATUS_OK Upon successful completion.
// * @retval ONI_STATUS_ERROR Upon any kind of failure.
// */
//ONI_C_API OniStatus oniRecorderStart(OniRecorderHandle recorder);

///**
// * Stops recording. You can resume recording via oniRecorderStart.
// * @param[in] recorder The handle to the recorder.
// * @retval ONI_STATUS_OK Upon successful completion.
// * @retval ONI_STATUS_ERROR Upon any kind of failure.
// */
//ONI_C_API void oniRecorderStop(OniRecorderHandle recorder);

///**
// * Stops recording if needed, and destroys a recorder.
// * @param	[in,out]	recorder	The handle to the recorder, the handle will be
// *									invalidated (nullified) when the function returns.
// * @retval ONI_STATUS_OK Upon successful completion.
// * @retval ONI_STATUS_ERROR Upon any kind of failure.
// */
//ONI_C_API OniStatus oniRecorderDestroy(OniRecorderHandle* pRecorder);

//ONI_C_API OniStatus oniCoordinateConverterDepthToWorld(OniStreamHandle depthStream, float depthX, float depthY, float depthZ, float* pWorldX, float* pWorldY, float* pWorldZ);

//ONI_C_API OniStatus oniCoordinateConverterWorldToDepth(OniStreamHandle depthStream, float worldX, float worldY, float worldZ, float* pDepthX, float* pDepthY, float* pDepthZ);

//ONI_C_API OniStatus oniCoordinateConverterDepthToColor(OniStreamHandle depthStream, OniStreamHandle colorStream, int depthX, int depthY, OniDepthPixel depthZ, int* pColorX, int* pColorY);




    }


    public class NITE2Wrapper
    {
        [Flags]
        public enum NiteStatus : uint
        {
	        NITE_STATUS_OK = 0,
	        NITE_STATUS_ERROR = 1,
	        NITE_STATUS_BAD_USER_ID = 2
        }

        [DllImport("NiTE2.dll")]
        public static extern NiteStatus niteInitialize();

        [DllImport("NiTE2.dll")]
        public static extern void niteShutdown();
    }





    class ZigInputOpenNI2 : IZigInputReader
    {

        IntPtr pDevice;
        IntPtr pDepthStream;
        IntPtr pDeviceCBHandle;
        OpenNI2.OpenNI2Wrapper.OniDeviceCallbacks callbacks;
        Delegate Connected;
        Delegate Disconnected;
        Delegate StateChanged;
        IntPtr pCallbacks;
        // init/update/shutdown
	    public void Init(ZigInputSettings settings)
        {
            Debug.Log("Init called for OpenNI2");
            OpenNI2.OpenNI2Wrapper.OniVersion v = OpenNI2.OpenNI2Wrapper.oniGetVersion();

            Debug.Log("OpenNI2 version: " + v.major + "." + v.minor + "." + v.maintenance + "." + v.build);

            OpenNI2.OpenNI2Wrapper.OniStatus s = OpenNI2.OpenNI2Wrapper.oniInitialize(2000);
            Debug.Log("OpenNI2 Wrapper Started : " + s);


            OpenNI2.NITE2Wrapper.NiteStatus ns = OpenNI2.NITE2Wrapper.niteInitialize();
            Debug.Log("NITE2 Wrapper Started : " + ns);

            IntPtr pDevices = IntPtr.Zero;
            int numDevices = 0;
            s = OpenNI2.OpenNI2Wrapper.oniGetDeviceList(ref pDevices, ref numDevices);
            Debug.Log("OpenNI2 Wrapper device list gotten : " + s + " num devices " + numDevices + " pDevices" + pDevices);
            OpenNI2.OpenNI2Wrapper.OniDeviceInfo info = new OpenNI2.OpenNI2Wrapper.OniDeviceInfo();
            if (numDevices == 0)
            {
                Debug.Log("OpenNI2 Error: No device connected");
            }
            else
            {
                for (int i = 0; i < numDevices; i++)
                {
                    IntPtr ptr = new IntPtr(pDevices.ToInt32() + i);
                    //IntPtr obj = Marshal.ReadIntPtr(pDevices);
                    info = (OpenNI2.OpenNI2Wrapper.OniDeviceInfo)Marshal.PtrToStructure(ptr, typeof(OpenNI2.OpenNI2Wrapper.OniDeviceInfo));
                    Debug.Log("uri:" + new String(info.uri));
                    Debug.Log("vendor:" + new String(info.vendor));
                    Debug.Log("name:" + new String(info.name));

                }

                string uri = new String(info.uri);
                Debug.Log("opening from uri:" + info.uri);
                s = OpenNI2.OpenNI2Wrapper.oniDeviceOpen(uri, ref pDevice);
                Debug.Log("OPENNI2 Wrapper device open: " + s + " pDevice: " + pDevice);
            }
            s = OpenNI2.OpenNI2Wrapper.oniReleaseDeviceList(pDevices);
            Debug.Log("OpenNI2 Wrapper device list released : " + s + " pDevices " + pDevices);




            //callbacks.deviceConnected = new OpenNI2.OpenNI2Wrapper.OniDeviceInfoCallback(deviceConnected_handler);
            Connected = new OpenNI2.OpenNI2Wrapper.OniDeviceInfoCallback(deviceConnected_handler);
            Disconnected = new OpenNI2.OpenNI2Wrapper.OniDeviceInfoCallback(deviceDisconnected_handler);
            StateChanged = new OpenNI2.OpenNI2Wrapper.OniDeviceStateCallback(deviceStateChanged_handler);
            callbacks.deviceConnected = Marshal.GetFunctionPointerForDelegate(Connected);
            callbacks.deviceDisconnected = Marshal.GetFunctionPointerForDelegate(Disconnected);
            callbacks.deviceStateChanged = Marshal.GetFunctionPointerForDelegate(StateChanged);

            //TODO: these callbacks have a weird behavior where they don't print the info the second time the callback occurs
            pCallbacks = Marshal.AllocHGlobal(Marshal.SizeOf(callbacks));
            Marshal.StructureToPtr(callbacks, pCallbacks, true);
            s = OpenNI2.OpenNI2Wrapper.oniRegisterDeviceCallbacks(pCallbacks, pDevice, out pDeviceCBHandle);
            Debug.Log("OpenNI2 Wrapper oniRegisterDeviceCallback called : " + s + "pDeviceCBHandle:" + pDeviceCBHandle);

            if (pDevice != IntPtr.Zero)
            {
                s = OpenNI2.OpenNI2Wrapper.oniDeviceCreateStream(pDevice, OpenNI2.OpenNI2Wrapper.OniSensorType.ONI_SENSOR_DEPTH, ref pDepthStream);
                Debug.Log("OpenNI2 Wrapper Stream created : " + s + " pDepthStream " + pDepthStream);

                s = OpenNI2.OpenNI2Wrapper.oniStreamStart(pDepthStream);
                Debug.Log("OpenNI2 Wrapper Stream started : " + s);
        
        //TODO: should get properties of the stream and set it to resolution
            }

        }
        void deviceConnected_handler(ref OpenNI2.OpenNI2Wrapper.OniDeviceInfo info, IntPtr pCookie)
        {
            Debug.Log("Device Connection Callback");
            //Debug.Log("pInfo: " + pInfo);

            //OpenNI2.OpenNI2Wrapper.OniDeviceInfo info = new OpenNI2.OpenNI2Wrapper.OniDeviceInfo();
            //Marshal.PtrToStructure(pInfo, info);

            try
            {//TODO: fails to print the second time a connected or disconnectec callback is called
                Debug.Log("printing info in Connection handler");
                Debug.Log("uri:" + new String(info.uri));
                Debug.Log("vendor:" + new String(info.vendor));
                Debug.Log("name:" + new String(info.name));
            }
            catch (Exception e)
            {
                Debug.LogWarning("info Connection Exception : " + e.Message);
            }
        }
        void deviceDisconnected_handler(ref OpenNI2.OpenNI2Wrapper.OniDeviceInfo info, IntPtr pCookie)
        {
            Debug.Log("Device Disconnection Callback");
            //Debug.Log("pInfo: " + pInfo);
            //OpenNI2.OpenNI2Wrapper.OniDeviceInfo info = new OpenNI2.OpenNI2Wrapper.OniDeviceInfo();
            //Marshal.PtrToStructure(pInfo, info);

            try
            {
                Debug.Log("uri:" + new String(info.uri));
                Debug.Log("vendor:" + new String(info.vendor));
                Debug.Log("name:" + new String(info.name));
            }
            catch (Exception e)
            {
                Debug.LogWarning("info Disconnection Exception : " + e.Message);
            }
            if (pDepthStream != IntPtr.Zero)
            {
                OpenNI2.OpenNI2Wrapper.oniStreamStop(pDepthStream);
                Debug.Log("OpenNI2 stream stopped");

                OpenNI2.OpenNI2Wrapper.oniStreamDestroy(pDepthStream);
                Debug.Log("OpenNI2 stream destroyed" + pDepthStream);

                pDepthStream = IntPtr.Zero;
                Debug.Log("OpenNI2 stream zeroed");
            }

            if (pDevice != IntPtr.Zero)
            {

                Debug.Log("Non-zero pDevice in disconnected " + pDevice);
                OpenNI2.OpenNI2Wrapper.oniDeviceClose(pDevice);
                Debug.Log("OpenNI2 device closed");
                pDevice = IntPtr.Zero;

            }
        }

        void deviceStateChanged_handler(ref OpenNI2.OpenNI2Wrapper.OniDeviceInfo info, OpenNI2.OpenNI2Wrapper.OniDeviceState deviceState, IntPtr pCookie)
        {
            Debug.Log("Device State Change Callback");

            //OpenNI2.OpenNI2Wrapper.OniDeviceInfo info = new OpenNI2.OpenNI2Wrapper.OniDeviceInfo();
            //Marshal.PtrToStructure(pInfo, info);
            /*
            OpenNI2.OpenNI2Wrapper.OniDeviceState deviceState = new OpenNI2.OpenNI2Wrapper.OniDeviceState();
            Marshal.PtrToStructure(pDeviceState, deviceState);
        
             */
            Debug.Log("uri:" + new String(info.uri));
            Debug.Log("vendor:" + new String(info.vendor));
            Debug.Log("name:" + new String(info.name));
            Debug.Log("State:" + deviceState);

        }
        public bool keepTrying = true;
	    public void Update()
        {
            if (keepTrying)
            {
                UpdateDepth();
            }
        }
	    public void Shutdown()
        {
            Debug.Log("Shutdown called for OpenNI2");
            if (pDepthStream != IntPtr.Zero)
            {
                OpenNI2.OpenNI2Wrapper.oniStreamStop(pDepthStream);
                Debug.Log("OpenNI2 stream stopped");

                OpenNI2.OpenNI2Wrapper.oniStreamDestroy(pDepthStream);
                Debug.Log("OpenNI2 stream destroyed");
            }
            if (pDeviceCBHandle != IntPtr.Zero)
            {
                OpenNI2.OpenNI2Wrapper.oniUnregisterDeviceCallbacks(pDeviceCBHandle);
                Debug.Log("OPENNI2 unregistered callbacks");
            }
            if (pDevice != IntPtr.Zero)
            {
                OpenNI2.OpenNI2Wrapper.oniDeviceClose(pDevice);
                Debug.Log("OpenNI2 Device Close");
            }
		    OpenNI2.OpenNI2Wrapper.oniShutdown();
		
		    Debug.Log("OpenNI2 Shutdown");
		    OpenNI2.NITE2Wrapper.niteShutdown();
		    Debug.Log("NITE2 Shutdown");
	    
        }
	
	    // users & hands
	    public event EventHandler<NewUsersFrameEventArgs> NewUsersFrame;
	
        // streams
        //bool UpdateDepth { get; set; }
        //bool UpdateImage { get; set; }
        //bool UpdateLabelMap { get; set; }

 
        void UpdateDepth()
        {
            try
            {
                IntPtr pFrame = new IntPtr();
                OpenNI2.OpenNI2Wrapper.OniStatus s = OpenNI2.OpenNI2Wrapper.oniStreamReadFrame(pDepthStream,  ref pFrame);
                Debug.Log("UpdateDepth: " + s);

                Debug.Log("pFrame: " + pFrame);

                IntPtr ptr = Marshal.ReadIntPtr(pFrame);
                Debug.Log("ReadIntPtr(pFrame): " + ptr);
                OpenNI2.OpenNI2Wrapper.OniFrame frame = new OpenNI2.OpenNI2Wrapper.OniFrame();

                keepTrying = false;
                /*

                frame = (OpenNI2.OpenNI2Wrapper.OniFrame)Marshal.PtrToStructure(pFrame, typeof(OpenNI2.OpenNI2Wrapper.OniFrame));
                Debug.Log("Marshalled to frame");
                Debug.Log("frame: " + frame.width + " " + frame.height);
            
                OpenNI2.OpenNI2Wrapper.oniFrameAddRef(ref frame);
                Debug.Log("frameAddRef");
                if (Depth == null)
                {
                    Depth = new ZigDepth(frame.width, frame.height);
                    Debug.Log("ZigDepth created");
                }
            
                Marshal.Copy(frame.data, Depth.data, 0, Depth.data.Length);
                Debug.Log("Marshal Copy complete " + Depth.data.Length);
                OpenNI2.OpenNI2Wrapper.oniFrameRelease(ref frame);
                Debug.Log("oniFrameRelease");
                 */
            }
            catch (Exception e)
            {
                Debug.LogWarning("Exception in UpdateDepth: " + e.Message);
                keepTrying = false;
            }
        }

        public ZigDepth Depth { get; private set; }
        public ZigImage Image { get; private set; }
        public ZigLabelMap LabelMap { get; private set; }

        // misc
        public Vector3 ConvertWorldToImageSpace(Vector3 worldPosition)
        { 
             return Vector3.zero;
        }
        public Vector3 ConvertImageToWorldSpace(Vector3 imagePosition)
        {
            return Vector3.zero;
        }
        public bool AlignDepthToRGB { get; set; }
    }
}