﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramePositioningDetection : MonoBehaviour
{
  
    [SerializeField]
    private int id;

    [SerializeField]
    private float sensibility=0.04f;


    [Header("GameObject from hive")]
    [SerializeField]
    private GameObject[] frameDetectionVolume;

    [SerializeField]
    private GameObject[] framePositionReferenceInHive;

    [SerializeField]
    private GameObject[] framePositionReference;


    private bool[] wellPositionedAt = { false, false, false };

    private int distPrecision = 3;

    private float actualDist = 0.0f;
    private float actualStandardDeviation = 0.0f;







    private bool tidy = false; //true if honeycomb his well stored in the hive

    private bool tidyButIncorrect = false; //Used to stop message spam



 
    void FixedUpdate()
    {
        isStillTidy();



        //Reset values 
        for (int i = 0; i < wellPositionedAt.Length; i++)
        {
            wellPositionedAt[i] = false;
        }
    }


    public void setId(int newID)
    {
        id = newID;
    }

    public float getActualDist()
    {
        return actualDist;
    }

    public float getActualStandardDeviation()
    {
        return actualStandardDeviation;
    }

    public bool isInTheHive()
    {
        bool res = true;
        //Check if the wood frame touches all detection areas
        foreach (bool b in wellPositionedAt)
        {
            if (!b) res = false;

        }
        return res;
    }

    /*
     * Check if the object is still tidy in a storage area of the hive, by calculate distance between them
     * if not, pass "tidy" attribut to false
     */
    private void isStillTidy()
    {

        //If the wood frame touches all detection areas, now check if it well positionned
        float standardDeviation=0;
        float averageDist=0;

        bool res = isInTheHive();
        if (res)
        {

            //Calculate 3 distances between reference points on frame and reference points in hive
            float distDown = Vector3.Distance(framePositionReferenceInHive[2].transform.position, framePositionReference[2].transform.position);

            float distUp1 = Vector3.Distance(framePositionReferenceInHive[0].transform.position, framePositionReference[0].transform.position);
            distUp1 = Mathf.Min(distUp1, Vector3.Distance(framePositionReferenceInHive[1].transform.position, framePositionReference[0].transform.position));

            float distUp2 = Vector3.Distance(framePositionReferenceInHive[0].transform.position, framePositionReference[1].transform.position);
            distUp2 = Mathf.Min(distUp2, Vector3.Distance(framePositionReferenceInHive[1].transform.position, framePositionReference[1].transform.position));

            //Calculate standard deviation and average dist
            averageDist = (distDown + distUp1 + distUp2) / 3;
            standardDeviation = Mathf.Max(Mathf.Max(distDown, distUp1), distUp2) - Mathf.Min(Mathf.Min(distDown, distUp1), distUp2);
            averageDist = truncate(averageDist, distPrecision);

            //Save actual values in a var
            actualDist = averageDist;
            actualStandardDeviation= standardDeviation;
        }

        if (tidy && !res)
        {
            tidy = false;
            tidyButIncorrect = false ;
            Debug.Log("I am " + id + "and I moved");
        }
        else if (!tidy && res)
        {
            if(standardDeviation<sensibility)
            {
                Debug.Log("I am " + id + " and I found an area for me with a distance of " + averageDist + " in the hive");
                tidy = true;
                tidyButIncorrect = false;
            } else
            {
                if(!tidyButIncorrect)
                {
                    Debug.Log("I am " + id + " and I am in the hive but I am not straight, my positioning is incorrect " + standardDeviation);
                    tidyButIncorrect = true;
                }

            }

        }
    }


    /**
     * truncate a float number keeping only a certain number of decimal
     **/
    private float truncate(float val, int nbAfterTheDecimalPoint)
    {
        float tenPow= Mathf.Pow(10f, (float) nbAfterTheDecimalPoint);
        float res=Mathf.Floor(val * tenPow) / tenPow;
        return res;
    }
   

    private void OnTriggerStay(Collider other)
    {
        for (int i = 0; i < frameDetectionVolume.Length; i++)
        {
            if (other.Equals(frameDetectionVolume[i].GetComponent<Collider>()))
            {
                wellPositionedAt[i] = true;
            }
        }
    }

}
