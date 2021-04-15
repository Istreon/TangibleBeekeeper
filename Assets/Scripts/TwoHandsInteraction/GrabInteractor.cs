using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabInteractor : MonoBehaviour
{
    private XRDirectInteractor firstInteractor = null;
    private XRDirectInteractor secondInteractor = null;

    private bool firstGrab = false;
    private bool secondGrab = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInteractor(XRDirectInteractor interactor)
    {
        if(firstInteractor == null)
        {
            firstInteractor = interactor;
            firstGrab = true;
        }
        else if(firstInteractor != null && secondInteractor == null)
        {
            secondInteractor = interactor;
            secondGrab = true;
        }
    }

    public void ClearInteractor(XRDirectInteractor interactor)
    {
        if(interactor == firstInteractor)
        {
            firstInteractor = null;
            firstGrab = false;
        }
        else if(interactor == secondInteractor)
        {
            secondInteractor = null;
            secondGrab = false;
        }
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
                firstInteractor.SendHapticImpulse(0.3f, 0.3f);
                secondInteractor.SendHapticImpulse(0.1f, 0.3f);
            }
            else
            {
                secondInteractor.SendHapticImpulse(0.3f, 0.3f);
                firstInteractor.SendHapticImpulse(0.1f, 0.3f);
            }
        }
        else if(firstGrab && !secondGrab)
        {
            firstInteractor.SendHapticImpulse(0.3f, 0.3f);
        }
        else if (secondGrab && !firstGrab)
        {
            secondInteractor.SendHapticImpulse(0.3f, 0.3f);
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
