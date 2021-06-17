using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodFrameManager : MonoBehaviour
{
    private bool isFrameGrabbed = false;
    private GrabInteractor grabInteractor;
    private Vector3 newHiveSpot;
    private HiveManager newParent;
    public GameObject queen;
    private bool hasQueen = false;

    public List<GameObject> grabPoints;

    private float[] yPos = new float[2] {-0.2665f, -0.0052f};
    private float[] xPos = new float[2] {-0.2008f, 0.2008f};
    private float zPos = 0.009f;

    // Start is called before the first frame update
    void Start()
    {
        grabInteractor = gameObject.GetComponent<GrabInteractor>();
        newHiveSpot = gameObject.transform.localPosition;
        newParent = gameObject.transform.parent.gameObject.GetComponent<HiveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(grabInteractor.firstGrab | grabInteractor.secondGrab)
        {
            isFrameGrabbed = true;
            gameObject.layer = 11; //FrameLayer, so that the frame does not interact with the others
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 11;
            }
            foreach (GameObject gp in grabPoints)
            {
                gp.layer = 10; //SecondGrabLayer, so that the grab points d not interact with the other objects
            }
        }
        else if(!grabInteractor.firstGrab && !grabInteractor.secondGrab)
        {
            //This order ensure that the frame is setteled only once
            PutInHive();
            isFrameGrabbed = false;
            gameObject.layer = 9; //InteractableLayer, bas layer for interactable objects
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 9;
            }
            foreach (GameObject gp in grabPoints)
            {
                gp.layer = 10; //SecondGrabLayer, so that the grab points d not interact with the other objects
            }
        }
    }

    public bool IsFrameGrabbed()
    {
        return isFrameGrabbed;
    }

    public bool HasQueen()
    {
        return hasQueen;
    }

    public void SetHiveSpot(Vector3 pos, HiveManager parent)
    {
        newHiveSpot = pos;
        newParent = parent;
    }

    private void PutInHive()
    {
        if(isFrameGrabbed)
        {
            gameObject.transform.SetParent(newParent.gameObject.transform);
            newParent.SettleFrame(gameObject, newHiveSpot);
        }
        gameObject.transform.localPosition = newHiveSpot;
        gameObject.transform.localEulerAngles = Vector3.zero;
    }

    public void SetQueen()
    {
        Vector3 pos;
        Vector3 rot;
        float mult = Random.Range(-1.0f, 1.0f);
        if(mult <= 0.0f)
        {
            pos = new Vector3(Random.Range(xPos[0], xPos[1]), Random.Range(yPos[0], yPos[1]), -1 * zPos);
            rot = new Vector3(180, Random.Range(0,360), Random.Range(0,360));
        }
        else
        {
            pos = new Vector3(Random.Range(xPos[0], xPos[1]), Random.Range(yPos[0], yPos[1]), zPos);
            rot = new Vector3(0, Random.Range(0,360), Random.Range(0,360));
        }
        GameObject hiveQueen = Instantiate(queen, transform);
        hiveQueen.transform.localPosition = pos;
        hiveQueen.transform.localEulerAngles = rot;
        hasQueen = true;
    }

    public void HideGrabPoints()
    {
        foreach (GameObject gp in grabPoints)
        {
            gp.SetActive(false);
        }
    }

    public void ShowGrabPoints()
    {
        foreach (GameObject gp in grabPoints)
        {
            gp.SetActive(true);
        }
    }

}
