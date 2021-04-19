using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net.Sockets;

public class ConnectedSmoker : MonoBehaviour
{

    internal Boolean socketReady = false;

    private TcpClient mySocket;
    private NetworkStream theStream;
    private StreamReader theReader;

    [SerializeField]
    private String Host = "10.29.238.39";
    private Int32 Port = 80;

    private Smoker smokerControlled;
    private bool active=false;

    // Start is called before the first frame update
    void Start()
    {
        setupSocket();
        readSocket();
        smokerControlled = this.GetComponent<Smoker>();
    }

    // Update is called once per frame
    void Update()
    {

 

        string msg=readSocket();
        if (msg != "")
        {
           //Debug.Log(msg.ToString());
            if (!active && msg.Equals("On") && smokerControlled!=null) {
                smokerControlled.ReleaseSmoke();
                active=true;
            }
            if (active && msg.Equals("Off")) {
                active=false;
            }

        }
    }

    public void setupSocket() { 
        try {
            mySocket = new TcpClient(Host, Port);
            theStream = mySocket.GetStream(); 
            theStream.ReadTimeout = 1;
            theReader = new StreamReader(theStream);
            socketReady = true;         
        }
        catch (Exception e) {
            Debug.Log("Socket error: " + e);
            Destroy(GetComponent<ConnectedSmoker>());
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
        theReader.Close();
        mySocket.Close();
        socketReady = false;
    }
}
