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
        TextAsset f = (TextAsset)Resources.Load("custom_dir/" + "DivisionScene_TTS");
        string fileText = System.Text.Encoding.UTF8.GetString(f.bytes);
        //string[] lines = System.IO.File.ReadAllLines(fileText);
        string[] lines = fileText.Split('\n');
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
            if(sceneIndex < displayText.Count)
            {
                Debug.Log("Step 1: instruction pt. " + (sceneIndex + 1));
                canContinue = false;
                textBox.SetActive(true);
                subText.SetText(displayText[sceneIndex]);
                sceneAudio.PlayOneShot(playAudio[sceneIndex]);
                CheckIfAPressed();
            }
            else if(sceneIndex == displayText.Count)
            {
                Debug.Log("Step 2: division");
                canContinue = false;
                textBox.SetActive(false);
                //areHivesDivided = CheckHivesStates();
                CheckIfAPressed();
            }

            else if(sceneIndex == displayText.Count + 1)
            {
                Debug.Log("Step 3: wait for next mission");
                canContinue = false;
                foreach (AudioSource audio in ambientSound)
                {
                    audio.Stop();
                }
                blackBox.EnableBlackBoxMode();
                waitingScreen.SetActive(true);
                CheckIfAPressed();
            }

            else if(sceneIndex == displayText.Count + 2)
            {
                canContinue = false;
                blackBox.EnableBlackBoxMode();
                waitingScreen.SetActive(false);
                loadingScreen.gameObject.SetActive(true);
                loadingScreen.StartLoading("Scenario0Scene");
            }
        }

        if(!sceneAudio.isPlaying)
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
