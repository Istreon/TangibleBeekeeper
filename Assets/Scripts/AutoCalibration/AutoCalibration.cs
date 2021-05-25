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
        var keyboardActionMap = control.FindActionMap("KeyboardMap");
        actionCalibration = keyboardActionMap.FindAction("Calibrate");
        actionCalibration.performed += OnActivation;
        actionCalibration.Enable();
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
        diff.y = 0.0f;
        optitrackSpaceTransform.position = optitrackSpaceTransform.position + diff;     
   

    }
}
