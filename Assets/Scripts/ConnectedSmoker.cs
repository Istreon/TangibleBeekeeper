using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net.Sockets;

public class ConnectedSmoker : MonoBehaviour
{

    internal Boolean socketReady = false;

    TcpClient mySocket;
    NetworkStream theStream;
    StreamWriter theWriter;
    StreamReader theReader;
    String Host = "192.168.0.27";
    Int32 Port = 80;

    // Start is called before the first frame update
    void Start()
    {
        setupSocket();
        readSocket();   
    }

    // Update is called once per frame
    void Update()
    {
        string msg=readSocket();
        if(msg!="") Debug.Log(msg.ToString());
    }

    public void setupSocket() { 
        try {
            mySocket = new TcpClient(Host, Port);
            theStream = mySocket.GetStream(); 
            theStream.ReadTimeout = 1;
            theWriter = new StreamWriter(theStream);
            theReader = new StreamReader(theStream);
            socketReady = true;         
        }
        catch (Exception e) {
            Debug.Log("Socket error: " + e);
        }
    }
    public String readSocket() { 
        if (!socketReady)
            return "";
        try {
            return theReader.ReadLine();
        } catch (Exception e) {
            return "";
        }

    }
    public void closeSocket() {
        if (!socketReady)
            return;
        theWriter.Close();
        theReader.Close();
        mySocket.Close();
        socketReady = false;
    }
}
