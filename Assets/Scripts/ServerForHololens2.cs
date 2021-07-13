using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerForHololens2 : MonoBehaviour
{

    private GameObject[] objects;
    private int[] id;
    // Start is called before the first frame update
    void Start()
    {
        OptitrackRigidBody [] tab=FindObjectsOfType<OptitrackRigidBody>();
        id = new int[tab.Length];
        objects = new GameObject[tab.Length];

        int i = 0;
        foreach(OptitrackRigidBody o in tab)
        {
            id[i] = o.RigidBodyId;
            objects[i] = o.gameObject;
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Create a server that communicate tracked object transforms
}
