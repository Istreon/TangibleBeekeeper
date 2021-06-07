using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BlackBoxMode : MonoBehaviour
{
    public List<XRBaseInteractor> sceneInteractors;
    public List<GameObject> objectsToHide;
    public GameObject blackBox;
    
    private bool isBlackBoxActivated;
    private Dictionary<XRBaseInteractor, bool> interactorsState;
    private Dictionary<GameObject, bool> objectsState;

    // Start is called before the first frame update
    void Start()
    {
        isBlackBoxActivated = false;
        interactorsState = new Dictionary<XRBaseInteractor, bool>();
        objectsState = new Dictionary<GameObject, bool>();
        foreach (XRBaseInteractor interactor in sceneInteractors)
        {
            interactorsState.Add(interactor, interactor.enabled);
        }
        foreach (GameObject o in objectsToHide)
        {
            objectsState.Add(o, o.activeInHierarchy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isBlackBoxActivated)
        {
            foreach (XRBaseInteractor interactor in sceneInteractors)
            {
                interactorsState[interactor] = interactor.enabled;
            }
            foreach (GameObject o in objectsToHide)
            {
                objectsState[o] = o.activeInHierarchy;
            }
        }
    }

    public void EnableBlackBoxMode()
    {
        if(!isBlackBoxActivated)
        {
            GraphController graphController;
            if(gameObject.TryGetComponent<GraphController>(out graphController))
            {
                graphController.enabled = false;
            }
            foreach (XRBaseInteractor interactor in sceneInteractors)
            {
                interactor.enabled = false;
            }
            foreach (GameObject o in objectsToHide)
            {
                o.SetActive(false);
            }
            blackBox.SetActive(true);

            isBlackBoxActivated = true;
        }
        
    }

    public void DisableBlackBoxMode()
    {
        if(isBlackBoxActivated)
        {
            GraphController graphController;
            if(gameObject.TryGetComponent<GraphController>(out graphController))
            {
                graphController.enabled = true;
            }
            foreach (XRBaseInteractor interactor in sceneInteractors)
            {
                interactor.enabled = interactorsState[interactor];
            }
            foreach (GameObject o in objectsToHide)
            {
                o.SetActive(objectsState[o]);
            }
            blackBox.SetActive(false);

            isBlackBoxActivated = false;
        }
    }
}
