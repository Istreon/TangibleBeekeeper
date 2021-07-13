using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoneycombVisualManager : MonoBehaviour
{

    [SerializeField]
    InputActionAsset control;
    InputAction actionN;



    private HoneycombVisualSelector[] selectorTab;
    // Start is called before the first frame update
    void Start()
    {
        selectorTab = FindObjectsOfType<HoneycombVisualSelector>();
        var keyboardActionMap = control.FindActionMap("KeyboardMap");
        actionN = keyboardActionMap.FindAction("Next");
        actionN.performed += OnActivation;
        actionN.Enable();
    }

    void OnActivation(InputAction.CallbackContext context)
    {
        NextVisual();
    }

    void NextVisual()
    {
        foreach(HoneycombVisualSelector h in selectorTab)
        {
            if(h!=null)
            {
                h.NextVisual();
            }
        }
    }
}
