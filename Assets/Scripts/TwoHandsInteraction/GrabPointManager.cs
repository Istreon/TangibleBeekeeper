using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabPointManager : MonoBehaviour
{
    public GameObject firstGrabPoint;
    public GameObject secondGrabPoint;

    private Collider managerCollider = null;

    public FixedJoint firstJoint;
    public FixedJoint secondJoint;

    private bool isFirstGrabbed = false;
    private bool isSecondGrabbed = false;

    // Start is called before the first frame update
    void Start()
    {
        managerCollider = GetComponent<Collider>();
        managerCollider.isTrigger = true;

        //firstGrabPoint.GetComponent<Rigidbody>().isKinematic = true;
        firstJoint.connectedBody = firstGrabPoint.GetComponent<Rigidbody>();
        //secondGrabPoint.GetComponent<Rigidbody>().isKinematic = true;
        secondJoint.connectedBody = secondGrabPoint.GetComponent<Rigidbody>();       

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(!isFirstGrabbed)
        {
            firstJoint.connectedBody = null;
            firstGrabPoint.transform.position = other.gameObject.transform.position;
            firstJoint.connectedBody = firstGrabPoint.GetComponent<Rigidbody>();
        }
        
        if(isFirstGrabbed && !isSecondGrabbed)
        {
            secondJoint.connectedBody = null;   
            secondGrabPoint.transform.position = other.gameObject.transform.position;
            secondJoint.connectedBody = secondGrabPoint.GetComponent<Rigidbody>();   
        }
    }

    public void SetFirstGrab()
    {
        if(!isFirstGrabbed)
        {
            isFirstGrabbed = true;
        }
    }

    public void ClearFirstGrab()
    {
        if(isFirstGrabbed)
        {
            isFirstGrabbed = false;
        }
    }

    public void SetSecondGrab()
    {
        if(!isSecondGrabbed)
        {
            isSecondGrabbed = true;
        }
    }

    public void ClearSecondGrab()
    {
        if(isSecondGrabbed)
        {
            isSecondGrabbed = false;
        }
    }
}
