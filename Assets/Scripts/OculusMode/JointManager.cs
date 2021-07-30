using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointManager : MonoBehaviour
{
    public HingeJoint leftJoint;
    public HingeJoint rightJoint;
    public SpringJoint springJoint;
    public InteractiveGrabber leftGrabber;
    public InteractiveGrabber rightGrabber;
    private Vector3 leftGrabDistance;
    private Vector3 rightGrabDistance;


    // Start is called before the first frame update
    void Start()
    {
        //leftGrabber = leftJoint.connectedBody.gameObject.GetComponent<InteractiveGrabber>();
        //rightGrabber = rightJoint.connectedBody.gameObject.GetComponent<InteractiveGrabber>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeJoints()
    {
        /*Debug.Log("Entered InitializeJoints()");
        Rigidbody rigidBody = this.gameObject.GetComponent<Rigidbody>();
        JointSpring leftSpring = leftJoint.spring;
        JointSpring rightSpring = rightJoint.spring;
        
        
        //Debug.Log("Spring values are: L" + leftJoint.spring + " for " + leftJoint.maxDistance + "m & R" + rightJoint.spring + " for " + rightJoint.maxDistance + "m");
        
        leftSpring.spring = rigidBody.mass/*750 - rigidBody.mass*150*/;
        //rightSpring.spring = rigidBody.mass/*750 - rigidBody.mass*150*/;
        //springJoint.maxDistance = (rigidBody.mass / 2) * 0.02f;
        //springJoint.spring = 750 - rigidBody.mass*150;*/
        
        //Debug.Log("AFTER UPDATE | Spring values are: L" + leftJoint.spring + " for " + leftJoint.maxDistance + "m & R" + rightJoint.spring + " for " + rightJoint.maxDistance + "m");
    }

    public void ConfigureLeftJoint()
    {
        /*GameObject anchor = new GameObject("Empty");
        anchor.transform.position = leftGrabber.GetInteractorPosition();
        anchor.transform.SetParent(this.transform);
        //Vector3 interactorLocalPos = leftGrabber.GetInteractorPosition() - this.gameObject.transform.position;
        leftJoint.connectedAnchor = anchor.transform.localPosition;
        Destroy(anchor);*/
        //Debug.Log("Anchor at position " + leftJoint.connectedAnchor + " with interactor at position " + leftGrabber.GetInteractorPosition());
        //leftGrabDistance = leftGrabber.gameObject.transform.position - this.gameObject.transform.position;
    }

    public void ConfigureRightJoint()
    {
        /*GameObject anchor = new GameObject("Empty");
        anchor.transform.position = rightGrabber.GetInteractorPosition();
        anchor.transform.SetParent(this.transform);
        //Vector3 interactorLocalPos = rightGrabber.GetInteractorPosition() - this.gameObject.transform.position;
        rightJoint.connectedAnchor = anchor.transform.localPosition;
        DestroyImmediate(anchor);*/
        //Debug.Log("Anchor at position " + rightJoint.connectedAnchor + " with interactor at position " + rightGrabber.GetInteractorPosition());
        //rightGrabDistance = rightGrabber.gameObject.transform.position - this.gameObject.transform.position;
    }

    public void ResetLeftPointPos()
    {
        leftGrabber.gameObject.transform.rotation = this.gameObject.transform.rotation;
        leftGrabber.gameObject.transform.position = this.gameObject.transform.position + leftGrabDistance;
    }

    public void ResetRightPointPos()
    {
        rightGrabber.gameObject.transform.rotation = this.gameObject.transform.rotation;
        rightGrabber.gameObject.transform.position = this.gameObject.transform.position + rightGrabDistance;
    }
}
