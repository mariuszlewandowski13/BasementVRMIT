using UnityEngine;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections.Generic;
using System.Text;

public enum ClientType {
    streamer,
    listener
}


public class Server : MonoBehaviour
{
    Socket SeverSocket = null;
    Thread Socket_Thread = null;

    Thread StreamerThread = null;


    private List<Socket> clients;
    private Socket streamer;

    private object clientsLock = new object();

    private Queue<string> infos;
    private object infosLock = new object();

    void Awake()
    {
        clients = new List<Socket>();
        infos = new Queue<string>();

        Socket_Thread = new Thread(ListenNewClients);
        Socket_Thread.Start();

    }

    private void ListenNewClients()
    {
        SeverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9999);
        SeverSocket.Bind(ipep);
        SeverSocket.Listen(30);

        while (true)
        {
            AddNewInfo("Socket Standby....");
            Socket client = SeverSocket.Accept();
            AddNewInfo("Socket Connected.");

            IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;

            byte[] bytes = new byte[4];

            try
            {
                if (client.Receive(bytes) != 0)
                {
                    int userType;
                    if (int.TryParse(Encoding.Default.GetString(bytes), out userType))
                    {
                        if ((ClientType)userType == ClientType.listener)
                        {
                            AddNewInfo("new listener connected");
                            AddScoketToClients(client);

                        }
                        else if ((ClientType)userType == ClientType.streamer)
                        {
                            AddNewInfo("new streamer connected");
                            InitStreaming(client);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                client.Close();
                SeverSocket.Close();
            }

        }
    }

    private void AddNewInfo(string info)
    {
        lock (infosLock)
        {
            infos.Enqueue(info);
        }
    }

    private void ShowInfo()
    {
        string info = "";
        lock (infosLock)
        {
             info = infos.Dequeue();

        }
        Debug.Log(info);
    }

    private void Update()
    {
        if (infos.Count > 0)
        {
            ShowInfo();
        }
    }

    private void InitStreaming(Socket newStreamer)
    {
        if (streamer != null)
        {
            if (StreamerThread.ThreadState == ThreadState.Running)
            {
                StreamerThread.Abort();
            }
            streamer.Close();
        }
        streamer = newStreamer;

        StreamerThread = new Thread(Stream);
        StreamerThread.Start();

    }

    private void Stream()
    {
        if (streamer != null && streamer.Connected)
        {
            NetworkStream recvStm = new NetworkStream(streamer);
            while (true)
            {
                byte[] bytes = new byte[1024 * 1024];
                if (streamer != null && streamer.Connected)
                {
                    int bufferLengthInt = 0;
                    if ((bufferLengthInt = streamer.Receive(bytes)) != 0)
                    {
                                AddNewInfo("Streaming buffer length: " + bufferLengthInt.ToString());
                                SendBufferToAll(bytes, bufferLengthInt);
                    }
                }
                else if (streamer != null && !streamer.Connected)
                {
                    AddNewInfo("Streamer not connected");
                    streamer = null;
                    
                    break;
                }
            }

        }
    }

    private void SendBufferToAll(byte[] buffer, int length)
    {
        CheckConnectedListeners();
        lock (clientsLock)
        {
            foreach (Socket client in clients)
            {
                client.Send(buffer, length, SocketFlags.None);
            }
        }
    }

    private void CheckConnectedListeners()
    {
        lock (clientsLock)
        {
            foreach (Socket client in clients)
            {
                if (!client.Connected)
                {
                    client.Close();
                    clients.Remove(client);
                }
            }
        }

    }

    private void AddScoketToClients(Socket newClient)
    {
        lock (clientsLock)
        {
            clients.Add(newClient);
        }
    }

    private void RemoveScoketFromClients(Socket clientToRemove)
    {
        lock (clientsLock)
        {
            clients.Remove(clientToRemove);
        }
    }

    private void CloseAllClients()
    {
        foreach (Socket client in clients)
        {
            client.Close();
        }
    }

    void OnApplicationQuit()
    {
        try
        {
            Socket_Thread.Abort();
            SeverSocket.Shutdown(SocketShutdown.Both);
            SeverSocket.Close();

            StreamerThread.Abort();


            Debug.Log("Bye~~");
        }
        catch
        {
            Debug.Log("Error when finished...");
        }
    }


}