using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GraphHider : MonoBehaviour
{
    public GraphController manager;
    public XRController hiderController;
    public InputHelpers.Button hideActivationButton;
    [Range(0.0f,1.0f)]
    public float activationThreshold = 0.5f;

    void Start() 
    {

    }

    void Update() 
    {
        InputHelpers.IsPressed(hiderController.inputDevice, hideActivationButton, out bool isActivate, activationThreshold);
        if(isActivate)
        {
            manager.UpdateGraphVisibility(false, Vector3.zero);
        }
    }
   
}
