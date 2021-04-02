using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandsInteractable : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();
    private XRBaseInteractor secondInteractor;
    private Quaternion attachInitialRotation;
    public enum TwoHandsRotationType {None, First, Second};
    public TwoHandsRotationType twoHandsRotationType;
    public bool snapToSecondHand = true;
    private Quaternion initialRotationOffset;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var item in secondHandGrabPoints)
        {
            item.onSelectEntered.AddListener(OnSecondHandGrab);
            item.onSelectExited.AddListener(OnSecondHandRelease);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        bool isAlreadyGrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
        return base.IsSelectableBy(interactor) && !isAlreadyGrabbed;
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        Debug.Log("FIRST GRAB ENTERED");
        attachInitialRotation = interactor.attachTransform.localRotation;
        if(GetComponent<Smoker>())
        {
            GetComponent<SmokerInteraction>().AddDevice(interactor.GetComponent<InputDevice>());
        }
        base.OnSelectEntered(interactor);
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        Debug.Log("FIRST GRAB EXITED");
        secondInteractor = null;
        interactor.attachTransform.localRotation = attachInitialRotation;
        if(GetComponent<Smoker>())
        {
            GetComponent<SmokerInteraction>().ResetDevices();
        }
        base.OnSelectExited(interactor);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if(secondInteractor && selectingInteractor)
        {
            //Compute the rotation
            if(snapToSecondHand)
            {
                selectingInteractor.attachTransform.rotation = GetTwoHandsRotation();
            }
            else
            {
                selectingInteractor.attachTransform.rotation = GetTwoHandsRotation() * initialRotationOffset;
            }
            
        }
        base.ProcessInteractable(updatePhase);
    }

    public void OnSecondHandGrab(XRBaseInteractor interactor)
    {
        Debug.Log("SECOND HAND GRAB");
        secondInteractor = interactor;
        initialRotationOffset = Quaternion.Inverse(GetTwoHandsRotation() * selectingInteractor.attachTransform.rotation);
        if(GetComponent<Smoker>())
        {
            GetComponent<SmokerInteraction>().AddDevice(interactor.GetComponent<InputDevice>());
        }
    }

    public void OnSecondHandRelease(XRBaseInteractor interactor)
    {
        Debug.Log("SECOND HAND RELEASE");
        secondInteractor = null;
        if(GetComponent<Smoker>())
        {
            GetComponent<SmokerInteraction>().RemoveDevice(interactor.GetComponent<InputDevice>());
        }
    }

    private Quaternion GetTwoHandsRotation()
    {
        Quaternion targetRotation;
        if(twoHandsRotationType == TwoHandsRotationType.None)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position);;
        }
        else if(twoHandsRotationType == TwoHandsRotationType.First)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, selectingInteractor.attachTransform.up);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, secondInteractor.attachTransform.up);
        }

        return targetRotation;
    }
}
