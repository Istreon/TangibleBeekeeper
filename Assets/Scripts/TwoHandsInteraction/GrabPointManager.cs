using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabPointManager : MonoBehaviour
{
    public GrabInteractor interactionManager;
    public GameObject firstGrabPoint;
    public GameObject secondGrabPoint;

    public GameObject firstDefault;
    public GameObject secondDefault;

    private Collider managerCollider = null;

    public HingeJoint firstJoint;
    public HingeJoint secondJoint;


    // Start is called before the first frame update
    void Start()
    {
        managerCollider = GetComponent<Collider>();
        managerCollider.isTrigger = true;
        
        firstDefault.SetActive(true);
        secondDefault.SetActive(true);
        
        firstJoint.connectedBody = firstDefault.GetComponent<Rigidbody>();
        secondJoint.connectedBody = secondDefault.GetComponent<Rigidbody>();

        firstGrabPoint.SetActive(false);
        secondGrabPoint.SetActive(false);       

    }

    // Update is called once per frame
    void Update()
    {
        UpdateJointRotation();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Equals("LeftHand") || other.gameObject.name.Equals("RightHand"))
        {
            if(!interactionManager.firstGrab)
            {
                firstGrabPoint.transform.position = other.gameObject.transform.position;
                firstGrabPoint.SetActive(true);
                firstJoint.connectedBody = firstGrabPoint.GetComponent<Rigidbody>();
                firstDefault.SetActive(false);
            }
            
            if(interactionManager.firstGrab && !interactionManager.secondGrab)
            { 
                secondGrabPoint.transform.position = other.gameObject.transform.position;
                secondGrabPoint.SetActive(true);
                secondJoint.connectedBody = secondGrabPoint.GetComponent<Rigidbody>(); 
                secondDefault.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name.Equals("LeftHand") || other.gameObject.name.Equals("RightHand"))
        {
            if(!interactionManager.firstGrab)
            {
                firstDefault.SetActive(true);
                firstJoint.connectedBody = firstDefault.GetComponent<Rigidbody>();
                firstGrabPoint.SetActive(false);
            }
            if(!interactionManager.secondGrab)
            {
                secondDefault.SetActive(true);
                secondJoint.connectedBody = secondDefault.GetComponent<Rigidbody>();
                secondGrabPoint.SetActive(false);
            }
        }
    }

    public void UpdateJointRotation()
    {
        float xRotation = managerCollider.gameObject.transform.rotation.x;
        if(xRotation > 45.0f)
        {
            firstJoint.axis = new Vector3(-1, 0, 0);
            secondJoint.axis = new Vector3(-1, 0, 0);
            JointSpring firstSpring = firstJoint.spring;
            firstSpring.targetPosition = 20;
            JointSpring secondSpring = secondJoint.spring;
            secondSpring.targetPosition = 20;

        }
        else if(xRotation < -45.0f)
        {
            firstJoint.axis = new Vector3(1, 0, 0);
            secondJoint.axis = new Vector3(1, 0, 0);
            JointSpring firstSpring = firstJoint.spring;
            firstSpring.targetPosition = -20;
            JointSpring secondSpring = secondJoint.spring;
            secondSpring.targetPosition = -20;
        }
        else
        {
            firstJoint.axis = new Vector3(0, 0, 1);
            secondJoint.axis = new Vector3(0, 0, 1);
            JointSpring firstSpring = firstJoint.spring;
            firstSpring.targetPosition = -5;
            JointSpring secondSpring = secondJoint.spring;
            secondSpring.targetPosition = -5;
        }
    }

}
