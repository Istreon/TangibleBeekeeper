using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointManager : MonoBehaviour
{
    public ConfigurableJoint leftJoint;
    public ConfigurableJoint rightJoint;
    private InteractiveGrabber leftGrabber;
    private InteractiveGrabber rightGrabber;
    private Vector3 leftGrabDistance;
    private Vector3 rightGrabDistance;

    // Start is called before the first frame update
    void Start()
    {
        leftGrabber = leftJoint.connectedBody.gameObject.GetComponent<InteractiveGrabber>();
        rightGrabber = rightJoint.connectedBody.gameObject.GetComponent<InteractiveGrabber>();
        leftGrabber.onSelectEntered.AddListener(delegate {ConfigureLeftJoint();});
        rightGrabber.onSelectEntered.AddListener(delegate {ConfigureRightJoint();});

        leftGrabber.onSelectExited.AddListener(delegate {ResetLeftPointPos();});
        rightGrabber.onSelectExited.AddListener(delegate {ResetRightPointPos();});

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeJoints()
    {
        Rigidbody rigidBody = this.gameObject.GetComponent<Rigidbody>();
        SoftJointLimitSpring leftLinSpring = leftJoint.linearLimitSpring;
        SoftJointLimit leftLinLimit = leftJoint.linearLimit;
        SoftJointLimitSpring leftRotSpring = leftJoint.angularYZLimitSpring;
        
        SoftJointLimitSpring rightLinSpring = rightJoint.linearLimitSpring;
        SoftJointLimit rightLinLimit = rightJoint.linearLimit;
        SoftJointLimitSpring rightRotSpring = rightJoint.angularYZLimitSpring;
        
        //Debug.Log("Spring values are: L" + leftJoint.spring + " for " + leftJoint.maxDistance + "m & R" + rightJoint.spring + " for " + rightJoint.maxDistance + "m");
        if(rigidBody.mass < 1.0f)
        {
            leftJoint.angularZMotion = ConfigurableJointMotion.Locked;
            rightJoint.angularZMotion = ConfigurableJointMotion.Locked;
        }
        leftLinSpring.spring = 750 - rigidBody.mass*150;
        rightLinSpring.spring = 750 - rigidBody.mass*150;
        leftLinLimit.limit = (rigidBody.mass / 2) * 0.01f;
        rightLinLimit.limit = (rigidBody.mass / 2) * 0.01f;
        leftRotSpring.spring = 750 - rigidBody.mass*150;
        rightRotSpring.spring = 750 - rigidBody.mass*150;
        
        //Debug.Log("AFTER UPDATE | Spring values are: L" + leftJoint.spring + " for " + leftJoint.maxDistance + "m & R" + rightJoint.spring + " for " + rightJoint.maxDistance + "m");
    }

    public void ConfigureLeftJoint()
    {
        Vector3 interactorLocalPos = leftGrabber.GetInteractorPosition() - this.gameObject.transform.position;
        leftJoint.connectedAnchor = interactorLocalPos;
        //Debug.Log("Anchor at position " + leftJoint.connectedAnchor + " with interactor at position " + leftGrabber.GetInteractorPosition());
        leftGrabDistance = leftGrabber.gameObject.transform.position - this.gameObject.transform.position;
    }

    public void ConfigureRightJoint()
    {
        Vector3 interactorLocalPos = rightGrabber.GetInteractorPosition() - this.gameObject.transform.position;
        rightJoint.connectedAnchor = interactorLocalPos;
        //Debug.Log("Anchor at position " + rightJoint.connectedAnchor + " with interactor at position " + rightGrabber.GetInteractorPosition());
        rightGrabDistance = rightGrabber.gameObject.transform.position - this.gameObject.transform.position;
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
