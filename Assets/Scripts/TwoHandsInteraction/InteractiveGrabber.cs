using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractiveGrabber : XRGrabInteractable
{
    public GrabInteractor interactorManager;

    private Vector3 interactorPosition = Vector3.zero;
    private Quaternion interactorRotation = Quaternion.identity;

    override protected void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);
        interactorManager.SetInteractor((HandInteractor) interactor);
        StoreInteractor(interactor);
        MatchAttachmentPoints(interactor);
    }

    private void StoreInteractor(XRBaseInteractor interactor)
    {
        interactorPosition = interactor.attachTransform.localPosition;
        interactorRotation = interactor.attachTransform.localRotation;
    }

    private void MatchAttachmentPoints(XRBaseInteractor interactor)
    {
        bool hasAttach = attachTransform != null;
        interactor.attachTransform.position = hasAttach ? attachTransform.position : transform.position;
        interactor.attachTransform.rotation = hasAttach ? attachTransform.rotation : transform.rotation;
    }

    override protected void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);
        interactorManager.ClearInteractor((HandInteractor) interactor);
        ResetAttachmentPoint(interactor);
        ClearInteractor(interactor);
    }

    private void ResetAttachmentPoint(XRBaseInteractor interactor)
    {
        interactor.attachTransform.localPosition = interactorPosition;
        interactor.attachTransform.localRotation = interactorRotation;
    }

    private void ClearInteractor(XRBaseInteractor interactor)
    {
        interactorPosition = Vector3.zero;
        interactorRotation = Quaternion.identity;

    }
}
