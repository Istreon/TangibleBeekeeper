using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandInteractor : XRDirectInteractor
{
    public GameObject ghostHand;

    public void SetAnchor(Transform anchor)
    {
        //Debug.Log("HandInteractor.SetAnchor");
        //ghostHand.SetActive(true);
        ghostHand.GetComponent<GhostHandPresence>().SetAnchor(anchor);
    }

    public void ClearAnchor()
    {
        //Debug.Log("HandInteractor.ClearAnchor");
        ghostHand.GetComponent<GhostHandPresence>().ClearAnchor(gameObject.GetComponent<XRController>().modelPrefab);
        //ghostHand.SetActive(false);
    }

    
}
