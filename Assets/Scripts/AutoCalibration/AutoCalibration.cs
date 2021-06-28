using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AutoCalibration : MonoBehaviour
{
    [SerializeField]
    InputActionAsset control;
    InputAction actionCalibration;

    [SerializeField]
    private Transform optitrackSpaceTransform;

    [SerializeField]
    private Transform mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        //Searching for control map (to use "space" key)
        var keyboardActionMap = control.FindActionMap("KeyboardMap");
        actionCalibration = keyboardActionMap.FindAction("Calibrate");
        actionCalibration.performed += OnActivation;
        actionCalibration.Enable();
    }

    void OnActivation(InputAction.CallbackContext context)
    {
        Calibrate();
    }

    void Calibrate()
    {
        //Calculate rotation difference between headset rotation and headset expected rotation
        Vector3 rotDiff = Vector3.zero;
        rotDiff.y = mainCamera.rotation.eulerAngles.y - this.transform.rotation.eulerAngles.y;

        //Update Optitrack origin rotation
        optitrackSpaceTransform.Rotate(rotDiff, Space.Self);

        //Calculate position difference between headset position and headset expected position
        Vector3 diff = mainCamera.position - this.transform.position;
        diff.y = 0.0f;

        //Update Optitrack Origin position
        optitrackSpaceTransform.position = optitrackSpaceTransform.position + diff;     
   

    }
}
