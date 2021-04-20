using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net.Sockets;

public class OpenXRError : MonoBehaviour
{
    private String Host = "10.0.0.0";
    private Int32 Port = 80;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            new TcpClient(Host, Port);
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
