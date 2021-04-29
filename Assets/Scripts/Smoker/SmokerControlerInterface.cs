using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SmokerControlerInterface : MonoBehaviour
{
    [SerializeField]
    InputActionAsset control;
    InputAction actionTriggerRight;
    InputAction actionTriggerLeft;

    private bool touched = false;
    Smoker smoker;
    // Start is called before the first frame update
    void Start()
    {
        var gameplayActionMap = control.FindActionMap("XRI RightHand");
        actionTriggerRight = gameplayActionMap.FindAction("Activate");
        actionTriggerRight.performed += OnActivation;
        actionTriggerRight.Enable();

        gameplayActionMap = control.FindActionMap("XRI LeftHand");
        actionTriggerLeft = gameplayActionMap.FindAction("Activate");
        actionTriggerLeft.performed += OnActivation;
        actionTriggerLeft.Enable();
        smoker = GetComponent<Smoker>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnActivation(InputAction.CallbackContext context)
    {
        if (touched) smoker.ReleaseSmoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VRController")) touched = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("VRController")) touched = false;
    }
}
