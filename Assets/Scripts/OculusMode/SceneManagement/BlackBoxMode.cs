using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class BlackBoxMode : MonoBehaviour
{
    public Transform cameraTransform;
    public InputDeviceCharacteristics leftCharacteristics;
    private InputDevice leftDevice;
    private bool menuBtnLastState = false;

    public List<XRBaseInteractor> sceneInteractors;
    public List<GameObject> objectsToHide;
    public GameObject blackBox;
    public GameObject transparentBox;
    public GameObject menu;
    public GameObject tutoPanel;
    public List<XRRayInteractor> menuInteractors;
    
    private bool isBlackBoxActivated;
    private bool isMenuActivated;
    private Dictionary<XRBaseInteractor, bool> interactorsState;
    private Dictionary<GameObject, bool> objectsState;

    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(leftCharacteristics, devices);
        leftDevice = devices[0];

        isBlackBoxActivated = false;
        isMenuActivated = false;
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
        foreach (XRRayInteractor ray in menuInteractors)
        {
            ray.gameObject.SetActive(false);
        }

        blackBox.SetActive(false);
        transparentBox.SetActive(false);
        menu.SetActive(false);
        tutoPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isBlackBoxActivated && !isMenuActivated)
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

        if(leftDevice.TryGetFeatureValue(CommonUsages.menuButton, out bool isPressed) && isPressed)
        {
            menuBtnLastState = true;
        }
        else if(menuBtnLastState)
        {
            menuBtnLastState = false;
            if(isMenuActivated)
            {
                HideMenu();
            }
            else
            {
                DisplayMenu();
            }
        }
    }

    public void EnableBlackBoxMode()
    {
        if(!isBlackBoxActivated)
        {
            foreach (XRBaseInteractor interactor in sceneInteractors)
            {
                interactor.gameObject.SetActive(false);
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
            foreach (XRBaseInteractor interactor in sceneInteractors)
            {
                interactor.gameObject.SetActive(true);
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

    public void DisplayMenu()
    {
        EnableBlackBoxMode();
        blackBox.SetActive(false);
        transparentBox.SetActive(true);
        menu.SetActive(true);
        tutoPanel.SetActive(false);
        foreach (HandInteractor interactor in sceneInteractors)
        {
            interactor.gameObject.SetActive(true);
        }
        foreach (XRRayInteractor ray in menuInteractors)
        {
            ray.gameObject.SetActive(true);
        }
        isMenuActivated = true;
        menu.transform.SetParent(this.gameObject.transform);
        menu.transform.eulerAngles = new Vector3(0.0f, menu.transform.eulerAngles.y, 0.0f);
        menu.transform.position = new Vector3(menu.transform.position.x, cameraTransform.position.y, menu.transform.position.z);
    }

    public void HideMenu()
    {
        transparentBox.SetActive(false);
        menu.SetActive(false);
        tutoPanel.SetActive(false);
        foreach (XRRayInteractor ray in menuInteractors)
        {
            ray.gameObject.SetActive(false);
        }
        isMenuActivated = false;
        menu.transform.SetParent(cameraTransform);
        menu.transform.localPosition = Vector3.forward * 0.75f;
        menu.transform.localEulerAngles = Vector3.zero;
        DisableBlackBoxMode();
    }
}
