using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class HiveManager : MonoBehaviour
{
    //public bool isMainHive = true;
    public enum HiveState
    {
        Empty = 0,
        Divided = 1,
        Full = 2
    }
    public HiveState hiveState;
    private Dictionary<Vector3,GameObject> hiveSpotDict;
    public GameObject frameToInstantiate;
    public GameObject roof;
    public Transform roofReferencePoint;

    private bool hasQueen = false;
    private int nbOfFrames = 0;

    // Start is called before the first frame update
    void Start()
    {
        System.Random rand = new System.Random();
        hiveSpotDict = new Dictionary<Vector3, GameObject>();
        for(int i = 0; i < 10; i++)
        {
            Vector3 pos = new Vector3( 0.0f, 0.3025f, (float) (-0.1715 + i*0.038) );
            if(hiveState == HiveState.Full)
            {
                GameObject newFrame = Instantiate(frameToInstantiate, transform);
                newFrame.transform.parent = gameObject.transform;
                newFrame.transform.localPosition = pos;
                newFrame.transform.localEulerAngles = Vector3.zero;
                hiveSpotDict.Add(pos, newFrame);
                nbOfFrames += 1;
                WoodFrameManager frameManager = newFrame.GetComponent<WoodFrameManager>();
                if(i == 0 || i == 9)
                    frameManager.SetFrameType(WoodFrameManager.FrameType.Miel);
                else if(i == 1 || i == 8)
                    frameManager.SetFrameType(WoodFrameManager.FrameType.Pollen);
                else if(i == 2 || i == 5)
                    frameManager.SetFrameType(WoodFrameManager.FrameType.CouvainOuvert);
                else
                    frameManager.SetFrameType(WoodFrameManager.FrameType.CouvainFerme);
            }
            else if(hiveState == HiveState.Divided)
            {
                if(i%2 == 0)
                {
                    GameObject newFrame = Instantiate(frameToInstantiate, transform);
                    newFrame.transform.parent = gameObject.transform;
                    newFrame.transform.localPosition = pos;
                    newFrame.transform.localEulerAngles = Vector3.zero;
                    hiveSpotDict.Add(pos, newFrame);
                    nbOfFrames += 1; 
                }
                else
                {
                    hiveSpotDict.Add(pos, null);
                }
            }
            else
            {
                hiveSpotDict.Add(pos, null);
            }
        }

        if(hiveState == HiveState.Full)
        {
            int frameIndex = rand.Next(hiveSpotDict.Values.Count);
            List<GameObject> frames = hiveSpotDict.Values.ToList();
            WoodFrameManager frameManager;
            frames[frameIndex].TryGetComponent<WoodFrameManager>(out frameManager);
            frameManager.SetQueen();
            hasQueen = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckFrames();
        
        foreach (GameObject frame in hiveSpotDict.Values)
        {
            if(frame != null)
            {
                WoodFrameManager frameManager;
                if(frame.TryGetComponent<WoodFrameManager>(out frameManager))
                {
                    if(frameManager.IsFrameGrabbed())
                    {
                        OnTriggerExit(frame.GetComponent<Collider>());
                        break;
                    }
                }
            }
        }
        
        Debug.Log("The hive " + gameObject.transform.parent.gameObject.name + " has " + nbOfFrames + " frame(s)");
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        WoodFrameManager frameManager;
        //RoofManager roofManager;
        if(other.gameObject.TryGetComponent<WoodFrameManager>(out frameManager))
        {
            if(!hiveSpotDict.ContainsValue(other.gameObject) && hiveSpotDict.ContainsValue(null))
            {
                GameObject empty = new GameObject("newFrameEmpty");
                empty.transform.SetParent(gameObject.transform);
                empty.transform.position = other.gameObject.transform.position;
                Vector3 closestSpot = Vector3.zero;
                float dist = Vector3.Distance(closestSpot, empty.transform.localPosition);
                foreach (Vector3 key in hiveSpotDict.Keys)
                {
                    float distToKey = Vector3.Distance(key, empty.transform.localPosition);
                    if(hiveSpotDict[key] == null && dist > distToKey)
                    {
                        closestSpot = key;
                        dist = distToKey;
                    }
                }
                frameManager.SetHiveSpot(closestSpot, this);
                Destroy(empty);
            }
        }
        /*else if(other.gameObject.TryGetComponent<RoofManager>(out roofManager))
        {
            roofManager.SetOnHive(roofReferencePoint, this);
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        WoodFrameManager frameManager;
        //RoofManager roofManager;
        if(other.gameObject.TryGetComponent<WoodFrameManager>(out frameManager))
        {
            foreach (Vector3 key in hiveSpotDict.Keys)
            {
                if(hiveSpotDict[key] == other.gameObject)
                {
                    hiveSpotDict[key] = null;
                    break;
                }
            }
        }
        /*else if(other.gameObject.TryGetComponent<RoofManager>(out roofManager))
        {
            //RemoveRoof();
        }*/
    }

    public void SettleFrame(GameObject frame, Vector3 spot)
    {
        hiveSpotDict[spot] = frame;
    }

    public bool IsDivided()
    {
        return hiveState == HiveState.Divided;
    }

    public bool HasQueen()
    {
        return hasQueen;
    }

    private void CheckFrames()
    {
        nbOfFrames = 0;
        hasQueen = false;
        foreach (GameObject frame in hiveSpotDict.Values)
        {
            if(frame != null)
            {
                nbOfFrames += 1;
                WoodFrameManager frameManager;
                if(frame.TryGetComponent<WoodFrameManager>(out frameManager))
                {
                    hasQueen = frameManager.HasQueen() | hasQueen;
                }
            }
        }
        if(nbOfFrames < 5)
        {
            hiveState = HiveState.Empty;
        }
        else if(nbOfFrames == 5)
        {
            hiveState = HiveState.Divided;
        }
        else
        {
            hiveState = HiveState.Full;
        }
    }

    public void PutRoofOn()
    {
        foreach (GameObject frame in hiveSpotDict.Values)
        {
            if(frame != null)
            {
                WoodFrameManager frameManager;
                if(frame.TryGetComponent<WoodFrameManager>(out frameManager))
                {
                    frameManager.HideGrabPoints();
                }
            }
            
        }
    }

    public void RemoveRoof()
    {
        foreach (GameObject frame in hiveSpotDict.Values)
        {
            if(frame != null)
            {
                WoodFrameManager frameManager;
                if(frame.TryGetComponent<WoodFrameManager>(out frameManager))
                {
                    frameManager.ShowGrabPoints();
                }
            }
            
        }
    }
}
