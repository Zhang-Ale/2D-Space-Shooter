using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSender : MonoBehaviour
{
    public OSC MyOsc;
    public Transform SyncSphere;

    void Start()
    {
        
    }

    void Update()
    {
        OscMessage tempMessageToSend = new OscMessage();
        tempMessageToSend.address = "/SyncSphereTransform"; //this address "" should start with a /

        //tempMessageToSend.values.Add("hello World");
        tempMessageToSend.values.Add(SyncSphere.localPosition.x);
        tempMessageToSend.values.Add(SyncSphere.localPosition.y);
        tempMessageToSend.values.Add(SyncSphere.localPosition.z);

        tempMessageToSend.values.Add(SyncSphere.localRotation.x);
        tempMessageToSend.values.Add(SyncSphere.localRotation.y);
        tempMessageToSend.values.Add(SyncSphere.localRotation.z);
        tempMessageToSend.values.Add(SyncSphere.localRotation.w);

        MyOsc.Send(tempMessageToSend);
    }
}
