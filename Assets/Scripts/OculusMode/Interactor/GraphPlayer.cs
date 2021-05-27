using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GraphPlayer : XRRayInteractor
{
    public GraphController manager;

    protected override void OnSelectEntered(XRBaseInteractable interactable)
    {
        base.OnSelectEntered(interactable);
    }

    protected override void OnSelectExited(XRBaseInteractable interactable)
    {
        base.OnSelectExited(interactable);
        RaycastHit hit;
        if(GetCurrentRaycastHit(out hit))
        {
            manager.UpdateGraphVisibility(true, hit.point);
        }
            
    }

}
