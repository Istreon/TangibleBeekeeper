using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GraphProgression : MonoBehaviour
{
    public InputDeviceCharacteristics rightCharacteristics;
    private InputDevice targetDevice;
    private int sceneIndex;
    private bool wasAPressed;
    private bool btnALastState = false;
    private int nbOfAPressed = 0;
    public BeesData dataModel;
    public GameObject textBox;
    public GameObject waitingScreen;
    public GameObject endScreen;
    private AudioSource sceneAudio;
    public AudioSource ambientSound;
    public BlackBoxMode blackBox;
    
    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(rightCharacteristics, devices);
        targetDevice = devices[0];
        Debug.Log("Using device " + targetDevice.name);

        waitingScreen.SetActive(false);
        endScreen.SetActive(false);
        blackBox.DisableBlackBoxMode();

        sceneAudio = gameObject.GetComponent<AudioSource>();

        sceneIndex = 0;
        wasAPressed = false;
        nbOfAPressed = 0;

        TMPro.TextMeshProUGUI subText = textBox.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        string[] lines = System.IO.File.ReadAllLines("Assets/Audio/GraphScene/TTS.txt");
        subText.SetText(lines[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if(sceneIndex == 0 && !sceneAudio.isPlaying)
        {
            sceneAudio.PlayOneShot(sceneAudio.clip);
            textBox.SetActive(true);
            sceneIndex += 1;
            nbOfAPressed = 0;
            Debug.Log("Step 0");
        }
        if(sceneIndex >= 1 && sceneIndex <= 3 && !sceneAudio.isPlaying && nbOfAPressed < 2)
        {
            Debug.Log("Step 1 & 2");
            textBox.SetActive(false);
            CheckIfNext();
        }
        if(sceneIndex > 3)
        {
            Debug.Log("Step 3");
            endScreen.SetActive(true);
            blackBox.EnableBlackBoxMode();
            ambientSound.Stop();
        }

        if(wasAPressed)
        {
            if(nbOfAPressed == 1)
            {
                Debug.Log("Waiting screen activated");
                waitingScreen.SetActive(true);
                blackBox.EnableBlackBoxMode();
                ambientSound.Pause();
                sceneIndex += 1;
                dataModel.NextScenario();
                wasAPressed  = false;
            }
            else if(nbOfAPressed == 2)
            {
                Debug.Log("Next graph observation");
                waitingScreen.SetActive(false);
                blackBox.DisableBlackBoxMode();
                ambientSound.UnPause();
                wasAPressed = false;
                nbOfAPressed = 0;
            }
        }
    }

    private void CheckIfNext()
    {
        Debug.Log("Entered CheckIfNext()");
        if(targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed) && isPressed)
        {
            btnALastState = true;
            Debug.Log("\'A\' pressed");
        }
        else if (btnALastState)
        {
            btnALastState = false;
            wasAPressed = true;
            nbOfAPressed += 1;
            Debug.Log("\'A\' released");
        } 
    }
}
