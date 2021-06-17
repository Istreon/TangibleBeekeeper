using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactGrapherRetriever : MonoBehaviour
{
    public BeesData model;

    public Vector3 dimension;
    public Vector3 maxValues;

    public PointCloudReferencer pointCloud;

    public bool xRelative = true;
    public bool yRelative = true;
    public bool zRelative = true;

    public bool zLogarithmicScale = true;

    public float refreshRate = 1;

    private float lastRefresh = -10;

    private Dictionary<int, int> idToPointID = new Dictionary<int, int>();

    private void Start()
    {
        /*if(zLogarithmicScale)
        {
            maxValues.z = Mathf.Log10(maxValues.z + 1);
        }*/
    }

    /**** EXPERIMENTAL ****/
    public void clearGraph()
    {
        foreach(KeyValuePair<int, int> entry in idToPointID)
        {
            pointCloud.freeIndex(entry.Value);
        }
        idToPointID.Clear();
    }
    /*********************/
    void Update()
    {
        if(zLogarithmicScale)
        {
            maxValues.z = Mathf.Log10(maxValues.z + 1);
        }

        if(Time.realtimeSinceStartup - lastRefresh > refreshRate)
        {
            List<Vector3> targets = new List<Vector3>();
            List<int> ids = new List<int>();
            List<Color> colors = new List<Color>();

            //Debug.Log("ContactGrapher AgentsSize - " + model.theAgents.Count);

            //update graph
            int size = model.beeData[model.turnIndex].Count;
            foreach (Bee b in model.beeData[model.turnIndex])
            {                
                Vector3 point = transformPoint(new Vector3(b.realAge, b.physioAge, b.exchange));
                targets.Add(point);
                colors.Add(b.physioAge > 0.5f ? Color.yellow : Color.red); //On change la couleur du point selon l'age physio
                //Debug.Log(b.physioAge > 0.5f ? Color.yellow : Color.red);

                int pointID;

                if(!idToPointID.ContainsKey(b.id))
                {
                    pointID = pointCloud.idManager.getNextFreeIndex();
                    idToPointID.Add(b.id, pointID);

                }
                else
                {
                    pointID = idToPointID[b.id];
                }

                ids.Add(pointID);
            }
            if(model.forward)
            {
                for(int i = 0; i < model.turnIndex - 1; i++)
                {
                    foreach (Bee deadBee in model.deadBees[i])
                    {
                        Debug.Log("Checking dead bees in the graph");
                        if(idToPointID.ContainsKey(deadBee.id))
                        {
                            Debug.Log("One dead bee spotted");
                            int beePointID = idToPointID[deadBee.id];
                            pointCloud.freeIndex(beePointID);
                            idToPointID.Remove(deadBee.id);
                        }
                    }
                }
                
            }
            else
            {
                for(int i = model.bornBees.Count - 1; i >= model.turnIndex; i--)
                {
                    foreach (Bee bornBee in model.bornBees[i])
                    {
                        Debug.Log("Checking new bees in the graph");
                        if(idToPointID.ContainsKey(bornBee.id))
                        {
                            Debug.Log("One new bee spotted");
                            int beePointID = idToPointID[bornBee.id];
                            pointCloud.freeIndex(beePointID);
                            idToPointID.Remove(bornBee.id);
                        }
                    }
                }
                
            }
            
            pointCloud.updatePoints(new UpdateOrder(targets, ids, colors));

            lastRefresh = Time.realtimeSinceStartup;
        }        
    }


    public Vector3 transformPoint(Vector3 point)
    {
        //if(point.z > 0)
            //Debug.Log(point.z);

        if (zLogarithmicScale)
        {
            point.z = Mathf.Log10(point.z + 1);
        }

        if (xRelative)
            maxValues.x = Mathf.Max(maxValues.x, point.x);
        if(yRelative)
            maxValues.y = Mathf.Max(maxValues.y, point.y);
        if(zRelative)
            maxValues.z = Mathf.Max(maxValues.z, point.z);


        Vector3 transformedPos = new Vector3();
        transformedPos.x = (point.x / maxValues.x) * dimension.x/* - transform.position.x*/;
        transformedPos.y = (point.y / maxValues.y) * dimension.y/* - transform.position.y*/;
        transformedPos.z = (point.z / maxValues.z) * dimension.z/* - transform.position.z*/;

        return transformedPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * dimension.x);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * dimension.y);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * dimension.z);
    }
}
