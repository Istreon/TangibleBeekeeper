using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class DivisionProgression : MonoBehaviour
{
    private bool canContinue = true;
    public InputDeviceCharacteristics rightCharacteristics;
    private InputDevice targetDevice;
    public GameObject textBox;
    private TMPro.TextMeshProUGUI subText;
    private AudioSource sceneAudio;
    private List<string> displayText;
    public List<AudioClip> playAudio;
    private int sceneIndex = 0;
    private bool wasPressed = false;
    private bool btnALastState = false;
    //private bool areHivesDivided = false;

    //public HiveManager firstHive;
    //public HiveManager secondHive;

    public GameObject waitingScreen;
    public List<AudioSource> ambientSound;
    public LoadingScreen loadingScreen;
    public BlackBoxMode blackBox;

    // Start is called before the first frame update
    void Start()
    {
        sceneAudio = gameObject.GetComponent<AudioSource>();

        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(rightCharacteristics, devices);
        targetDevice = devices[0];

        displayText = new List<string>();
        subText = textBox.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        string[] lines = System.IO.File.ReadAllLines("Assets/Audio/DivisionScene/TTS.txt");
        foreach (string l in lines)
        {
            displayText.Add(l);
        }

        sceneIndex = 0;
        wasPressed = false;
        btnALastState = false;
        canContinue = true;
        //areHivesDivided = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(canContinue && !sceneAudio.isPlaying)
        {
            if(sceneIndex == 0)
            {
                canContinue = false;
                Debug.Log("Step 1: intructions pt. 1");
                textBox.SetActive(true);
                subText.SetText(displayText[sceneIndex]);
                sceneAudio.PlayOneShot(playAudio[sceneIndex]);
                CanContinue();
            }
            else if(sceneIndex == 1)
            {
                canContinue = false;
                Debug.Log("Step 2: intructions pt. 2");
                subText.SetText(displayText[sceneIndex]);
                sceneAudio.PlayOneShot(playAudio[sceneIndex]);
                CanContinue();
            }
            else if(sceneIndex == 2)
            {
                canContinue = false;
                Debug.Log("Step 3: division");
                textBox.SetActive(false);
                //areHivesDivided = CheckHivesStates();
                CheckIfAPressed();
            }

            else if(sceneIndex == 3)
            {
                canContinue = false;
                Debug.Log("Step 4: wait for next mission");
                foreach (AudioSource audio in ambientSound)
                {
                    audio.Stop();
                }
                blackBox.EnableBlackBoxMode();
                waitingScreen.SetActive(true);
                CheckIfAPressed();
            }

            else if(sceneIndex == 4)
            {
                canContinue = false;
                blackBox.EnableBlackBoxMode();
                waitingScreen.SetActive(false);
                loadingScreen.gameObject.SetActive(true);
                loadingScreen.StartLoading("GraphScene");
            }
        }

        if(sceneIndex >= 2 && !sceneAudio.isPlaying)
        {
            CheckIfAPressed();
        }
        
    }

    private void CanContinue()
    {
        canContinue = true;
        sceneIndex += 1;
    }

    /*private bool CheckHivesStates()
    {
        Debug.Log("Entered CheckHivesStates()");
        bool isFirstDiv = false;
        bool isSecondDiv = false;
        if(firstHive.IsDivided() && !firstHive.HasQueen())
        {
            Debug.Log("First hive divided: OK");
            isFirstDiv = true;
        }
        if(secondHive.IsDivided() && secondHive.HasQueen())
        {
            Debug.Log("Secondhive divided: OK");
            isSecondDiv = true;
        }

        return isFirstDiv && isSecondDiv;
    }*/

    private void CheckIfAPressed()
    {
        Debug.Log("Entered CheckIfAPressed()");
        if(sceneIndex >= 2)
        {
            if(targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed) && isPressed)
            {
                Debug.Log("Button \'A\' is being pressed");
                btnALastState = true;
            }
            else if (btnALastState)
            {
                Debug.Log("Button \'A\' was pressed");
                btnALastState = false;
                //wasPressed = true;
                CanContinue();
            }
        }
            
    }
}
