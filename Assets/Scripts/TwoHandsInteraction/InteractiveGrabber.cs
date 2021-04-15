using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractiveGrabber : XRGrabInteractable
{
    public GrabInteractor interactorManager = null;

    override protected void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);
        this.attachTransform.rotation = interactor.transform.localRotation;
        interactorManager.SetInteractor((XRDirectInteractor) interactor);
    }

    override protected void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);
        interactorManager.ClearInteractor((XRDirectInteractor) interactor);
    }
}
