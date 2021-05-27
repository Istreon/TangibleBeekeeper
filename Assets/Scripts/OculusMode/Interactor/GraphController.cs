using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GraphController : MonoBehaviour
{
    public GameObject vrCamera;

    public XRController rayController;
    public InputHelpers.Button rayActivationButton;
    [Range(0.0f,1.0f)]
    public float activationThreshold = 0.1f;
    public GraphPlayer graphPlayer;
    public GraphHider graphHider;
    
    [HideInInspector]
    public bool isGraphShown = false;
    public GameObject graph;
    public List<MainController> controllers;


    // Start is called before the first frame update
    void Start()
    {
        UpdateGraphVisibility(false, Vector3.zero);
        graph.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(rayController)
        {
            rayController.gameObject.SetActive(CheckIfActivated(rayController));
        }

        graphHider.gameObject.SetActive(isGraphShown);

    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, rayActivationButton, out bool isActivate, activationThreshold);
        return isActivate;
    }

    public void UpdateGraphVisibility(bool show, Vector3 position)
    {
        Vector3 graphEuler = graph.transform.eulerAngles;
        Vector3 vrEuler = vrCamera.transform.localEulerAngles;
        isGraphShown = show;
        graph.transform.position = position;
        graph.transform.eulerAngles = new Vector3(graphEuler.x, vrEuler.y, graphEuler.z);
        graph.SetActive(show);
            foreach (MainController controller in controllers)
            {
                controller.ShowController(show);
            }
    }
}
