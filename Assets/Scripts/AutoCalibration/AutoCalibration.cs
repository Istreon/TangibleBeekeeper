using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AutoCalibration : MonoBehaviour
{

    [SerializeField]
    private bool oculusQuest2 = false;

    [SerializeField]
    InputActionAsset control;
    InputAction actionTriggerRight;
    InputAction actionTriggerLeft;

    [SerializeField]
    private Transform optitrackSpaceTransform;

    [SerializeField]
    private Transform mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        var keyboardActionMap = control.FindActionMap("KeyboardMap");
        actionTriggerRight = keyboardActionMap.FindAction("Calibrate");
        actionTriggerRight.performed += OnActivation;
        actionTriggerRight.Enable();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnActivation(InputAction.CallbackContext context)
    {
        Calibrate();
    }

    void Calibrate()
    {
        Vector3 rotDiff = Vector3.zero;
        rotDiff.y = mainCamera.rotation.eulerAngles.y - this.transform.rotation.eulerAngles.y;
        Debug.Log(rotDiff);

        optitrackSpaceTransform.Rotate(rotDiff, Space.Self);

        Vector3 diff = mainCamera.position - this.transform.position;
        if (oculusQuest2) diff.y = 0.0f;
        optitrackSpaceTransform.position = optitrackSpaceTransform.position + diff;     
   

    }
}
