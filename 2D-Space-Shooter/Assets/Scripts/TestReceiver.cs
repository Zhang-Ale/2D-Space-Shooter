using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestReceiver : MonoBehaviour
{
    public OSC MyOsc;
    public Transform SyncSphere;

    void Start()
    {
        MyOsc.SetAddressHandler("/SyncSphereTransform", ReceiveSyncSphereTransformOSCMessage);
    }

    void Update()
    {
        
    }

    void ReceiveSyncSphereTransformOSCMessage(OscMessage inputMessage)
    {
        //print(inputMessage.GetString(0));
        SyncSphere.localPosition = new Vector3(inputMessage.GetFloat(0), inputMessage.GetFloat(1), inputMessage.GetFloat(2));
        SyncSphere.localRotation = new Quaternion(inputMessage.GetFloat(3), inputMessage.GetFloat(4), inputMessage.GetFloat(5), inputMessage.GetFloat(6));
    }
}
