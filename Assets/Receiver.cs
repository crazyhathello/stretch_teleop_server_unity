using NetMQ;
using NetMQ.Sockets;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using AsyncIO;
public class Receiver : MonoBehaviour
{
    private PullSocket socket;
    public RawImage displayImage;

    private Texture2D texture;

    private void Start()
    {
        ForceDotNet.Force();
        socket = new PullSocket();
        socket.Bind("tcp://*:12346");

        texture = new Texture2D(360, 640);
    }

    private void Update()
    {
        byte[] incomingImg = socket.ReceiveFrameBytes();
        if (incomingImg != null && incomingImg.Length > 0)
        {
            // Texture2D texture = new Texture2D(720, 1280);
            texture.LoadImage(incomingImg);
            texture.Apply();

            // Update the UI
            displayImage.texture = texture;

        }
    }

    private void OnDestroy()
    {
        socket.Close();
        NetMQConfig.Cleanup();
    }
    // private void OnApplicationPause()
    // {
    //     // var terminationString = "terminate";
    //     // for (int i = 0; i < 10; i++)
    //     // {
    //     //     socket.SendFrame(terminationString);
    //     // }
    //     // socket.Close();
    //     // NetMQConfig.Cleanup();
    // }
}
