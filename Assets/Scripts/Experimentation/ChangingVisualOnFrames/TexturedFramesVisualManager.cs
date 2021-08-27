using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TexturedFramesVisualManager : MonoBehaviour
{
    [SerializeField]
    InputActionAsset control;
    InputAction actionN;

    [SerializeField]
    private GameObject[] visualsPrefabTab;

    private TexturedFramesVisualSelector[] selectorTab;
    // Start is called before the first frame update
    void Start()
    {
        selectorTab = FindObjectsOfType<TexturedFramesVisualSelector>();
        var keyboardActionMap = control.FindActionMap("KeyboardMap");
        actionN = keyboardActionMap.FindAction("Next");
        actionN.performed += OnActivation;
        actionN.Enable();
    }

    void OnActivation(InputAction.CallbackContext context)
    {
        DivideVisuals();
    }

    private void DivideVisuals()
    {
        ShuffleVisualPrefabTab();
        int i = 0;
        foreach(TexturedFramesVisualSelector t in selectorTab)
        {
            if(i>=visualsPrefabTab.Length-1) //We need two value of the array at each iteration. We check here if there is enough value in our array position
            {
                ShuffleVisualPrefabTab();
                i = 0;
            }
            t.ChangeVisual(visualsPrefabTab[i], visualsPrefabTab[i + 1]);
            i += 2;
        }
    }


    private void ShuffleVisualPrefabTab()
    {
        for (int i = 0; i < visualsPrefabTab.Length; i++)
        {
            GameObject temp = visualsPrefabTab[i];
            int randomIndex = Random.Range(i, visualsPrefabTab.Length);
            visualsPrefabTab[i] = visualsPrefabTab[randomIndex];
            visualsPrefabTab[randomIndex] = temp;
        }
    }
}
