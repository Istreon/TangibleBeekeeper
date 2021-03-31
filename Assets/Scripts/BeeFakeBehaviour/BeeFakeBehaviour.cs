﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeFakeBehaviour : MonoBehaviour
{

    [SerializeField]
    private Transform areaOrigin;

    [SerializeField]
    private float beeSpeed;

    private float diameter = 3.0f;
    private float height=0.7f;


    private Vector3 nextObjective;


    float time;
    float reach;

    // Start is called before the first frame update
    void Start()
    {
        nextObjective = this.transform.position;
        time = 0;
        reach = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, nextObjective) > 0.05f)
        {
            time += Time.deltaTime;
            reach = Mathf.InverseLerp(0, 1 / beeSpeed, time);
            this.transform.position = Vector3.Lerp(transform.position, nextObjective, reach);
        }
        else
        {
            updateNextObjective();
        }


    }

    void updateNextObjective()
    {
        time = 0;
        Vector3 temp = Random.insideUnitCircle * diameter;
        nextObjective = new Vector3(temp.x + areaOrigin.position.x, areaOrigin.position.y + Random.value * height*2 - height, areaOrigin.position.z + temp.y); ;
        this.transform.LookAt(nextObjective);
            
    }
}
