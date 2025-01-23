using NetMQ;
using NetMQ.Sockets;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using AsyncIO;
public class ReceiverWithWrist : MonoBehaviour
{
    private PullSocket headVideoSocket;

    private PullSocket wristVideoSocket;
    public RawImage HeadDisplayImage;
    public RawImage WristDisplayImage;

    private Texture2D headVideoTexture;
    private Texture2D wristVideoTexture;

    private void Start()
    {
        ForceDotNet.Force();
        headVideoSocket = new PullSocket();
        headVideoSocket.Bind("tcp://*:12346");

        wristVideoSocket = new PullSocket();
        wristVideoSocket.Bind("tcp://*:12347");

        headVideoTexture = new Texture2D(360, 640);
        wristVideoTexture = new Texture2D(480, 270);
    }

    private void Update()
    {
        byte[] headIncomingImg = headVideoSocket.ReceiveFrameBytes();
        byte[] wristIncomingImg = wristVideoSocket.ReceiveFrameBytes();

        if (headIncomingImg != null && headIncomingImg.Length > 0)
        {
            headVideoTexture.LoadImage(headIncomingImg);
            headVideoTexture.Apply();

            // Update the UI
            HeadDisplayImage.texture = headVideoTexture;

        }

        if (wristIncomingImg != null && wristIncomingImg.Length > 0)
        {
            wristVideoTexture.LoadImage(wristIncomingImg);
            wristVideoTexture.Apply();

            // Update the UI
            WristDisplayImage.texture = wristVideoTexture;
        }
    }

    private void OnDestroy()
    {
        headVideoSocket.Close();
        wristVideoSocket.Close();
        NetMQConfig.Cleanup();
    }
}
