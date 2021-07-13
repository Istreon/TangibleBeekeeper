using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboveHiveDetection : MonoBehaviour
{
    //2 pts
    //0.75 m

    [SerializeField]
    private GameObject[] hiveOutline;

    [SerializeField]
    private GameObject[] checkedObjectOutline;

    private int lastNbOfIncludedPoints = 0;


  
    // Update is called once per frame
    void FixedUpdate()
    {
        int nb = howManyPointAreIncluded();
        float dist = getAverageDistanceY();
        if(nb!=lastNbOfIncludedPoints)
        {
            lastNbOfIncludedPoints = nb;
            switch (nb)
            {
                case 0:
                    Debug.Log("Roof is not above the hive");
                    break;

                default:
                    Debug.Log("Roof. Only nb : " + nb + " points are well positioned .... dist = "+ dist);
                    break;
            }
        }
       
    }

    public bool objectHidesInsideOfTheHive()
    {
        bool result = false;

        int nb = howManyPointAreIncluded();
        float dist = getAverageDistanceY();

        if (nb>=2 && dist<0.75)
        {
            result = true;
        }

        return result;
    }

    private float getAverageDistanceY()
    {
        float heightObject = 0.0f;
        float heightHive = 0.0f;
        float result;
        int count = 0;

        //Get average height of the tested object
        foreach(GameObject g in checkedObjectOutline)
        {
            if(g!=null)
            {
                heightObject += g.transform.position.y;
                count++;
            }
        }
        heightObject = heightObject /count;


        //Get average height of the hive outline
        count = 0;
        foreach(GameObject g in hiveOutline)
        {
            if (g != null)
            {
                heightHive += g.transform.position.y;
                count++;
            }
        }
        heightHive = heightHive / count;


        //Get distance between both average height

        result = heightObject - heightHive;

        return result;
    }

    private int howManyPointAreIncluded()
    {
        int res = 0;
        for(int i=0;i< checkedObjectOutline.Length;i++)
        {
            if (pointIsIncluded(checkedObjectOutline[i].transform.position)) res++;
        }
        return res;
    }


    private bool pointIsIncluded(Vector3 pointToTest)
    {
        //Test if the point is below the hive 
        float a = hiveOutline[0].transform.up.x;
        float b = hiveOutline[0].transform.up.y;
        float c = hiveOutline[0].transform.up.z;
        float d = -(a * hiveOutline[0].transform.position.x + b * hiveOutline[0].transform.position.y + c * hiveOutline[0].transform.position.z);

        float dist = a * pointToTest.x + b * pointToTest.y + c * pointToTest.z + d;
        if (dist < 0)
        {
            return false;
        }

        //Test if the point is between the points forming the outline of the hive
        for (int i=0;i<hiveOutline.Length;i++)
        {
            int nextI = (i + 1) % hiveOutline.Length;

            Vector3 vec1 = hiveOutline[i].transform.position - pointToTest;
            vec1 = new Vector3(vec1.x, 0, vec1.z);
            Vector3 vec2 = hiveOutline[nextI].transform.position - pointToTest;
            vec2 = new Vector3(vec2.x, 0, vec2.z);
            //float angle=Vector3.Angle(vec1, vec2);
            float angle = Vector3.SignedAngle(vec2, vec1, Vector3.up);
            if (angle<0) return false;
        }
        return true;
    }

 }
