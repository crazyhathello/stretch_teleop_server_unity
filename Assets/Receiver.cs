using NetMQ;
using NetMQ.Sockets;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using AsyncIO;
public class Receiver : MonoBehaviour
{
    private PullSocket headVideoSocket;
    public RawImage HeadDisplayImage;
    private Texture2D headVideoTexture;

    private void Start()
    {
        ForceDotNet.Force();
        headVideoSocket = new PullSocket();
        headVideoSocket.Bind("tcp://*:12346");
        headVideoTexture = new Texture2D(360, 640);
    }

    private void Update()
    {
        byte[] headIncomingImg = headVideoSocket.ReceiveFrameBytes();

        if (headIncomingImg != null && headIncomingImg.Length > 0)
        {
            headVideoTexture.LoadImage(headIncomingImg);
            headVideoTexture.Apply();

            // Update the UI
            HeadDisplayImage.texture = headVideoTexture;
        }
    }

    private void OnDestroy()
    {
        headVideoSocket.Close();
        NetMQConfig.Cleanup();
    }
}
