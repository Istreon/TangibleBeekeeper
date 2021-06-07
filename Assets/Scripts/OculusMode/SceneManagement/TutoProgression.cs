using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutoProgression : MonoBehaviour
{
    [Header("Input devices")]
    public InputDeviceCharacteristics leftCharacteristics;
    public InputDeviceCharacteristics rightCharacteristics;
    private InputDevice leftDevice;
    private InputDevice rightDevice;

    [Header("Controllers")]
    public List<GameObject> controllers;
    public List<HandInteractor> hands;
    public List<XRRayInteractor> rays;

    [Header("Data & display")]
    public List<AudioClip> playAudio;
    private List<string> displayText = new List<string>();
    public TMPro.TextMeshProUGUI screenText;
    public List<GameObject> controllerButtons;
    public List<Button> buttons;
    public List<GameObject> images;
    private AudioSource sceneAudio;
    private AudioClip clipToPlay;

    private int sceneIndex = 0;
    private bool canContinue = true;
    private int nbOfBtnClicked = 0;
    private int wasClicked = 0;
    private int wasPicked = 0;
    private float t;
    private bool lPBLastSate = false;
    private bool rPBLastState = false;
    private bool lSBLastState = false;
    private bool rSBLastState = false;
    private bool lGLastState = false;
    private bool rGLastState = false;
    private bool lTLastState = false;
    private bool rTLastState = false;

    [HideInInspector]
    public bool isPuttingVisor = false;

    [Header("Visors")]
    public GameObject displayVisor;
    public GameObject playerVisor;

    [Header("Loading screen")]
    public LoadingScreen loadingScreen;
    public AudioSource bgm;
    public BlackBoxMode blackBox;
    


    // Start is called before the first frame update
    void Start()
    {
        loadingScreen.gameObject.SetActive(false);
        sceneIndex = 0;
        canContinue = true;
        wasClicked = 0;
        wasPicked = 0;
        blackBox.DisableBlackBoxMode();

        sceneAudio = gameObject.GetComponent<AudioSource>();
        clipToPlay = sceneAudio.GetComponent<AudioClip>();
        
        leftDevice = RetrieveDevice(leftCharacteristics);
        Debug.Log(leftDevice.name);
        rightDevice = RetrieveDevice(rightCharacteristics);
        Debug.Log(rightDevice.name);

        RetrieveText("Assets/Audio/Tuto/textToDisplay.txt");
        
        foreach (GameObject item in controllers)
        {
            item.SetActive(true);
        }
        foreach (HandInteractor item in hands)
        {
            item.gameObject.SetActive(false);
        }
        foreach (XRRayInteractor item in rays)
        {
            item.gameObject.SetActive(false);
        }
        foreach (GameObject canvas in controllerButtons)
        {
            canvas.SetActive(false);
        }
        foreach (Button btn in buttons)
        {
            btn.gameObject.SetActive(false);
        }
        foreach (GameObject img in images)
        {
            img.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(!sceneAudio.isPlaying && canContinue)
        {
            if(sceneIndex == 0)
            {
                canContinue = false;
                screenText.SetText(displayText[sceneIndex]);
                clipToPlay = playAudio[sceneIndex];
                sceneAudio.PlayOneShot(clipToPlay, 1.0f);
                ContinueScene();
            }
            else if(sceneIndex == 1)
            {
                canContinue = false;
                screenText.SetText(displayText[sceneIndex]);
                clipToPlay = playAudio[sceneIndex];
                sceneAudio.PlayOneShot(clipToPlay, 1.0f);
                images[0].SetActive(true);
                foreach (GameObject canvas in controllerButtons)
                {
                    canvas.SetActive(true);
                }
                nbOfBtnClicked = 0;
                CheckButtonsClicked();
                
            }
            else if(sceneIndex == 2)
            {
                canContinue = false;
                screenText.SetText(displayText[sceneIndex]);
                clipToPlay = playAudio[sceneIndex];
                sceneAudio.PlayOneShot(clipToPlay, 1.0f);
                images[0].SetActive(false);
                foreach (GameObject canvas in controllerButtons)
                {
                    canvas.SetActive(false);
                }
                foreach (Button btn in buttons)
                {
                    btn.gameObject.SetActive(true);
                    btn.onClick.AddListener(OneButtonClicked);
                }
                foreach (XRRayInteractor ray in rays)
                {
                    ray.gameObject.SetActive(true);
                }
                wasClicked = 0;
            }
            else if(sceneIndex == 3)
            {
                canContinue = false;
                screenText.SetText(displayText[sceneIndex]);
                clipToPlay = playAudio[sceneIndex];
                sceneAudio.PlayOneShot(clipToPlay, 1.0f);
                images[1].SetActive(true);
                foreach (Button btn in buttons)
                {
                    btn.onClick.RemoveAllListeners();
                    btn.gameObject.SetActive(false);
                }
                foreach (XRRayInteractor ray in rays)
                {
                    ray.gameObject.SetActive(false);
                }
                foreach (GameObject controller in controllers)
                {
                    controller.SetActive(false);
                }
                foreach (HandInteractor hand in hands)
                {
                    hand.gameObject.SetActive(true);
                    hand.onSelectEntered.AddListener(OneObjectPicked);
                }
                wasPicked = 0;
            }
            else if(sceneIndex == 4)
            {
                canContinue = false;
                screenText.SetText(displayText[sceneIndex]);
                clipToPlay = playAudio[sceneIndex];
                sceneAudio.PlayOneShot(clipToPlay, 1.0f);
                images[1].SetActive(false);
                foreach (HandInteractor hand in hands)
                {
                    hand.onSelectEntered.RemoveAllListeners();
                }
                t = Time.realtimeSinceStartup;
                ContinueScene();
            }
            else if(sceneIndex == 5)
            {
                canContinue = false;
                TryPutVisorOn();
            }
        }
        //Condition for the first step of the tutorial (i.e. learning about the controllers)
        if(nbOfBtnClicked <= 8 && !canContinue && sceneIndex <= 1)
        {
            CheckButtonsClicked();
        }
        //Condition before the last step of the tutorial (i.e. putting the visor on)
        if(!canContinue && sceneIndex >= 5)
        {
            TryPutVisorOn();
        }
        //Condition for the last step of the tutorial (i.e. visor put or time up)
        float deltaT = Time.realtimeSinceStartup - t;
        if((!sceneAudio.isPlaying && canContinue && sceneIndex == 6) | (sceneIndex == 6 && deltaT > 60))
        {
            //Debug.Log("LOAD NEXT SCENE");   
            bgm.Stop();
            sceneIndex = 10;
            blackBox.EnableBlackBoxMode();
            loadingScreen.gameObject.SetActive(true);
            loadingScreen.StartLoading("HivesScene");  
        }
    }

    private InputDevice RetrieveDevice(InputDeviceCharacteristics chara)
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(chara, devices);
        return devices[0];
    }

    private void RetrieveText(string path)
    {
        string[] lines = System.IO.File.ReadAllLines(path);
        foreach (string l in lines)
        {
            displayText.Add(l);
        }
    }

    private void CheckButtonsClicked()
    {
        if (nbOfBtnClicked < 8 )
        {
            if(leftDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool lPrimaryBtn) && lPrimaryBtn)
            {
                lPBLastSate = true;
            }
            else if(lPBLastSate) 
            {
                nbOfBtnClicked += 1;
                lPBLastSate = false;
            }
            if(rightDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool rPrimaryBtn) && rPrimaryBtn)
            {
                rPBLastState = true;
            }
            else if(rPBLastState) 
            {
                nbOfBtnClicked += 1;
                rPBLastState = false;
            }
            if(leftDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool lSecondaryBtn) && lSecondaryBtn)
            {
                lSBLastState = true;
            }
            else if(lSBLastState)
            {
                nbOfBtnClicked += 1;
                lSBLastState = false;
            }
            if(rightDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool rSecondaryBtn) && rSecondaryBtn)
            {
                rSBLastState = true;
            }
            else if(rSBLastState)
            {
                nbOfBtnClicked += 1;
                rSBLastState = false;
            }
            if(leftDevice.TryGetFeatureValue(CommonUsages.grip, out float lGrip) && lGrip > 0.2)
            {
                lGLastState = true;
            }
            else if(lGLastState)
            {
                nbOfBtnClicked += 1;
                lGLastState = false;
            }
            if(rightDevice.TryGetFeatureValue(CommonUsages.grip, out float rGrip) && rGrip > 0.2)
            {
                rGLastState = true;
            }
            else if(rGLastState)
            {
                nbOfBtnClicked += 1;
                rGLastState = false;
            }
            if(leftDevice.TryGetFeatureValue(CommonUsages.trigger, out float lTrigger) && lTrigger > 0.2)
            {
                lTLastState = true;
            }
            else if(lTLastState)
            {
                nbOfBtnClicked += 1;
                lTLastState = false;
            }
            if(rightDevice.TryGetFeatureValue(CommonUsages.trigger, out float rTrigger) && rTrigger > 0.2)
            {
                rTLastState = true;
            }
            else if(rTLastState)
            {
                nbOfBtnClicked += 1;
                rTLastState = false;
            }
        }
        else
        {
            ContinueScene();
        }
    }

    private void ContinueScene()
    {
        canContinue = true;
        sceneIndex += 1;
    }

    public void OneButtonClicked()
    {
        wasClicked += 1;
        Debug.Log("Number of buttons clicked " + wasClicked);
        if(wasClicked >= 2 && wasClicked < 5)
        {
            wasClicked = 10000;
            ContinueScene();
        }
    }

    protected virtual void OneObjectPicked(XRBaseInteractable interactable)
    {
        wasPicked += 1;
        Debug.Log("Number of objects picked " + wasPicked);
        if(wasPicked > 6 && wasPicked < 10)
        {
            wasPicked = 10000;
            ContinueScene();
        }
    }

    protected virtual void TryPutVisorOn()
    {
        if(isPuttingVisor)
        {
            GrabInteractor visorInteractor = displayVisor.GetComponent<GrabInteractor>();
            if(!visorInteractor.IsGrabbed())
            {
                displayVisor.SetActive(false);
                playerVisor.SetActive(true);
                isPuttingVisor = false;

                ContinueScene();
            }
        }
    }
}
