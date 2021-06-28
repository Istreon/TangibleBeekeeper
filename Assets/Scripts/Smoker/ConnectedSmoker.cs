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
    private String Host = "10.29.239.64";
    private Int32 Port = 80;

    private Smoker smokerControlled;
    private bool active=false;

    float timer = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        setupSocket();
        readSocket();
        smokerControlled = this.GetComponent<Smoker>();
    }


    private void FixedUpdate()
    {
        //Try to connect again if the last try failed
        if (!socketReady && timer > 2.0f)
        {
            setupSocket();
            readSocket();
        }  else
        {
            timer += Time.deltaTime;
        }
    }
    // Update is called once per frame
    void Update()
    {
        string msg=readSocket();
        if (msg != "")
        {
           //Debug.Log(msg.ToString());
            if (!active && msg.Equals("On") && smokerControlled!=null) {
                //Release smoke from the somker
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
            timer = 0.0f;
        }
    }
    public String readSocket() { 
        if (!socketReady)
            return "";
        try {
            return theReader.ReadLine();
        } catch (Exception) {
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
