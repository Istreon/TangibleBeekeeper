using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabInteractor : MonoBehaviour
{
    private HandInteractor firstInteractor = null;
    private HandInteractor secondInteractor = null;

    [HideInInspector]
    public bool firstGrab = false;
    [HideInInspector]
    public bool secondGrab = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInteractor(HandInteractor interactor)
    {
        if(firstInteractor == null)
        {
            firstInteractor = interactor;
            firstGrab = true;
            firstInteractor.SetAnchor(gameObject.transform);
        }
        else if(firstInteractor != null && secondInteractor == null)
        {
            secondInteractor = interactor;
            secondGrab = true;
            secondInteractor.SetAnchor(gameObject.transform);
        }
    }

    public void ClearInteractor(HandInteractor interactor)
    {
        if(interactor == firstInteractor)
        {
            firstInteractor.ClearAnchor();
            firstInteractor = null;
            firstGrab = false;
        }
        else if(interactor == secondInteractor)
        {
            secondInteractor.ClearAnchor();
            secondInteractor = null;
            secondGrab = false;
        }
    }

    public bool IsGrabbed()
    {
        return firstGrab | secondGrab;
    }

    private void OnCollisionEnter(Collision other)
    {
        PlayHaptic(other);
    }

    private void OnCollisionStay(Collision other)
    {
        PlayHaptic(other);
    }

    private void OnCollisionExit(Collision other)
    {
        StopHaptic();
    }

    private void PlayHaptic(Collision collision)
    {
        if(firstGrab && secondGrab)
        {
            float toFirstDist = Vector3.Distance(collision.transform.position, firstInteractor.transform.position);
            float toSecondDist = Vector3.Distance(collision.transform.position, secondInteractor.transform.position);
            if(toFirstDist < toSecondDist)
            {
                firstInteractor.SendHapticImpulse(0.1f, 0.1f);
                secondInteractor.SendHapticImpulse(0.01f, 0.1f);
            }
            else
            {
                secondInteractor.SendHapticImpulse(0.1f, 0.1f);
                firstInteractor.SendHapticImpulse(0.01f, 0.1f);
            }
        }
        else if(firstGrab && !secondGrab)
        {
            firstInteractor.SendHapticImpulse(0.1f, 0.1f);
        }
        else if (secondGrab && !firstGrab)
        {
            secondInteractor.SendHapticImpulse(0.1f, 0.1f);
        }
    }

    private void StopHaptic()
    {
        if(firstGrab)
        {
            firstInteractor.SendHapticImpulse(0.0f, 0.3f);
        }
        if(secondGrab)
        {
            secondInteractor.SendHapticImpulse(0.0f, 0.3f);
        }
    }

}
