using UnityEngine;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections.Generic;
using System.Text;


public class Client : MonoBehaviour
{ 

    byte[] bytes = new byte[1024 * 1024];

    byte[] firstInfo;

    Socket sender;

    byte[] bytesToRec = new byte[1024];

    private bool streaming;

    private bool frameToSend;

    private bool frameReady;
    private void Start()
    {
        if (ApplicationStaticData.IsSuperUser())
        {
            firstInfo = Encoding.Default.GetBytes("0");
        }
        else {
            firstInfo = Encoding.Default.GetBytes("1");
        }

        IPAddress ipAddress = IPAddress.Parse("35.227.44.24");
        IPEndPoint remoteEP = new IPEndPoint(ipAddress, 9999);

        // Create a TCP/IP  socket.  
        sender = new Socket(ipAddress.AddressFamily,
         SocketType.Stream, ProtocolType.Tcp);

        // Connect the socket to the remote endpoint. Catch any errors.  
        try
        {
            sender.Connect(remoteEP);

            Debug.Log("Socket connected to" +sender.RemoteEndPoint.ToString());
            // Send the data through the socket.  
            int bytesSent = sender.Send(firstInfo);


            if (!ApplicationStaticData.IsSuperUser())
            {
                Thread th = new Thread(WaitForFrames);
                th.Start();
            }
            

        }
        catch (ArgumentNullException ane)
        {
            Debug.Log("ArgumentNullException : {0}"+ane.ToString());
        }
        catch (SocketException se)
        {
            Debug.Log("SocketException : {0}"+ se.ToString());
        }
        catch (Exception e)
        {
            Debug.Log("Unexpected exception : {0}"+ e.ToString());
        }

    }

    void WaitForFrames()
    {

    }

    void Update()
    {
        if (ApplicationStaticData.IsSuperUser())
        {
            SendFrame();
        }
        else if (frameReady)
        {
            LoadFrame();
        }
    }

    void SendFrame()
    {
        if (!streaming && frameToSend)
        {
            streaming = true;
            Thread th = new Thread(SendAsync);
            th.Start();
        }
        else {
            frameToSend = true;
        }
    }

    void SendAsync()
    {
        byte [] length = Encoding.Default.GetBytes(bytesToRec.Length.ToString());
        sender.Send(length);
        sender.Send(bytesToRec);
        streaming = false;
        frameToSend = false;
    }


    void LoadFrame()
    {

    }

    private void OnApplicationQuit()
    {
        sender.Shutdown(SocketShutdown.Both);
        sender.Close();
    }


}