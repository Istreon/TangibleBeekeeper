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

    public float refreshRate = 1;

    private float lastRefresh = -10;

    private Dictionary<int, int> idToPointID = new Dictionary<int, int>();

    void Update()
    {
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

            pointCloud.updatePoints(new UpdateOrder(targets, ids, colors));

            lastRefresh = Time.realtimeSinceStartup;
        }        
    }


    public Vector3 transformPoint(Vector3 point)
    {
        maxValues.x = Mathf.Max(maxValues.x, point.x);
        maxValues.y = Mathf.Max(maxValues.y, point.y);
        maxValues.z = Mathf.Max(maxValues.z, point.z);

        Vector3 transformedPos = new Vector3();
        transformedPos.x = (point.x / maxValues.x) * dimension.x;
        transformedPos.y = (point.y / maxValues.y) * dimension.y;
        transformedPos.z = (point.z / maxValues.z) * dimension.z;
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
