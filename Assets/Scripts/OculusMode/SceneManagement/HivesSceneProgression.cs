/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HivesSceneProgression : MonoBehaviour
{
    [Header("Data & display")]
    public List<AudioClip> playAudio;
    private List<string> displayText = new List<string>();
    public TMPro.TextMeshProUGUI subText;
    private AudioSource sceneAudio;
    private AudioClip clipToPlay;
    public List<GameObject> arrows;
    public GameObject waitingScreen;
    [Range(0,750)]
    public int playTime = 120;

    private int sceneIndex = 0;
    private bool canContinue = true;
    private float timeAtStart;
    private bool isWaiting = false;
    private bool changeScene;
    private bool btnALastState = false;
    public InputDeviceCharacteristics characteristics;
    private InputDevice targetDevice;

    [Header("Loading screen")]
    public LoadingScreen loadingScreen;
    public GameObject subFrame;
    public List<AudioSource> allAudios;
    public BlackBoxMode blackBox;

    // Start is called before the first frame update
    void Start()
    {
        sceneIndex = 0;
        canContinue = true;
        timeAtStart = Time.realtimeSinceStartup;
        changeScene  = false;
        blackBox.DisableBlackBoxMode();
        btnALastState = false;

        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(characteristics, devices);
        targetDevice = devices[0];

        sceneAudio = gameObject.GetComponent<AudioSource>();
        clipToPlay = sceneAudio.GetComponent<AudioClip>();

        RetrieveText("Assets/Audio/HivesScene/TTS.txt");

        loadingScreen.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!sceneAudio.isPlaying && canContinue && sceneIndex < displayText.Count)
        {
            canContinue = false;
            subText.SetText(displayText[sceneIndex]);
            clipToPlay = playAudio[sceneIndex];
            sceneAudio.PlayOneShot(clipToPlay, 1.0f);
            /*if(sceneIndex == 1)
            {
                foreach (GameObject arrow in arrows)
                {
                    arrow.SetActive(true);
                }
            }
            else
            {
                foreach (GameObject arrow in arrows)
                {
                    arrow.SetActive(false);
                }
            }
            ContinueScene();
        }

        if(!sceneAudio.isPlaying && sceneIndex >= displayText.Count)
        {
            subFrame.SetActive(false);
        }
        if(Time.realtimeSinceStartup - timeAtStart > playTime && !isWaiting) 
        {
            //Remove headset to answer questions
            isWaiting = true;
            //changeScene = targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed) && isPressed;
            waitingScreen.SetActive(true);
            foreach (AudioSource audio in allAudios)
            {
                audio.Stop();
            }
            blackBox.EnableBlackBoxMode();
        }

        if(isWaiting)
        {
            Debug.Log("\'isWaiting\' condition fulfilled, \'if\' condition entered");
            CheckIfAPressed();
        }
        if(changeScene)
        {
            changeScene = false;
            isWaiting = false;
            Debug.Log("\'changeScene\' condition fulfilled, \'if\' condition entered");
            loadingScreen.gameObject.SetActive(true);
            loadingScreen.StartLoading("DivisionScene");
        }
    }

    private void ContinueScene()
    {
        sceneIndex += 1;
        canContinue = true;
    }

    private void RetrieveText(string path)
    {
        string[] lines = System.IO.File.ReadAllLines(path);
        foreach (string l in lines)
        {
            displayText.Add(l);
        }
    }

    private void CheckIfAPressed()
    {
        Debug.Log("Entered CheckIfAPressed()");
        if(targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed) && isPressed)
        {
            Debug.Log("Button \'A\' is being pressed");
            btnALastState = true;
        }
        else if(btnALastState)
        {
            Debug.Log("Button \'A\' was pressed");
            btnALastState = false;
            changeScene = true;
        }
    }
}*/
