﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Scenario0Progression : MonoBehaviour
{
    public InputDeviceCharacteristics rightCharacteristics;
    private InputDevice rightDevice;
    public InputDeviceCharacteristics leftCharacteristics;
    private InputDevice leftDevice;
    private int sceneIndex;
    private bool wasAPressed;
    private bool btnALastState = false;
    private bool btnXLastState = false;
    private bool isGoingBack = false;
    private int nbOfAPressed = 0;
    private bool canContinue = true;
    public BeesData dataModel;
    public GameObject textBox;
    private TMPro.TextMeshProUGUI subText;
    private List<string> displayText;
    public List<AudioClip> playAudio;
    public GameObject waitingScreen;
    public GameObject endScreen;
    private AudioSource sceneAudio;
    public AudioSource ambientSound;
    public BlackBoxMode blackBox;
    private int nbOfGraphs = 2;
    private bool isWaiting = false;
    public LoadingScreen loadingScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(rightCharacteristics, devices);
        rightDevice = devices[0];
        Debug.Log("Using device " + rightDevice.name);

        devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(leftCharacteristics, devices);
        leftDevice = devices[0];

        waitingScreen.SetActive(false);
        endScreen.SetActive(false);
        blackBox.DisableBlackBoxMode();
        loadingScreen.gameObject.SetActive(false);

        sceneAudio = gameObject.GetComponent<AudioSource>();

        sceneIndex = 0;
        wasAPressed = false;
        nbOfAPressed = 0;
        canContinue = true;
        isWaiting = false;

        displayText = new List<string>();
        subText = textBox.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        TextAsset f = (TextAsset)Resources.Load("custom_dir/" + "GraphScene_TTS");
        string fileText = System.Text.Encoding.UTF8.GetString(f.bytes);
        //string[] lines = System.IO.File.ReadAllLines(fileText);
        string[] lines = fileText.Split('\n');
        foreach(string l in lines)
        {
            displayText.Add(l);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(canContinue && !sceneAudio.isPlaying)
        {
            if(sceneIndex < displayText.Count)
            {
                Debug.Log("Step 1: instructions pt. " + (sceneIndex+1));
                canContinue = false;
                sceneAudio.PlayOneShot(playAudio[sceneIndex]);
                textBox.SetActive(true);
                subText.SetText(displayText[sceneIndex]);
                nbOfAPressed = 0;
                CheckIfNext();
            }
            
            else if(sceneIndex == displayText.Count)
            {
                canContinue = false;
                Debug.Log("Step 2: graph study");
                textBox.SetActive(false);
                CheckIfNext();
            }
            else if(sceneIndex == displayText.Count + 1)
            {
                canContinue = false;
                Debug.Log("Waiting screen activated");
                waitingScreen.SetActive(true);
                blackBox.EnableBlackBoxMode();
                ambientSound.Stop();
            }
            else if(sceneIndex == displayText.Count + 2)
            {
                canContinue = false;
                blackBox.EnableBlackBoxMode();
                waitingScreen.SetActive(false);
                loadingScreen.gameObject.SetActive(true);
                loadingScreen.StartLoading("Scenario1Scene");
            }
        }

        if(!sceneAudio.isPlaying)
        {
            CheckIfNext();
        }

        /*if(nbOfGraphs <= 0)
        {
            Debug.Log("Step 3: end of the simulation");
            endScreen.SetActive(true);
            blackBox.EnableBlackBoxMode();
            ambientSound.Stop();
        }

        if(wasAPressed)
        {
            if(isWaiting && nbOfGraphs == 2)
            {
                Debug.Log("Waiting screen activated");
                waitingScreen.SetActive(true);
                blackBox.EnableBlackBoxMode();
                ambientSound.Pause();
                //sceneIndex += 1;
                dataModel.NextScenario();
                wasAPressed  = false;
            }
            else if(!isWaiting && nbOfGraphs == 2)
            {
                Debug.Log("Next graph observation");
                waitingScreen.SetActive(false);
                blackBox.DisableBlackBoxMode();
                ambientSound.UnPause();
                wasAPressed = false;
                //nbOfAPressed = 0;
                nbOfGraphs -= 1;
            }
            else if(nbOfGraphs <= 1)
            {
                loadingScreen.gameObject.SetActive(true);
                loadingScreen.StartLoading("LobbyScene");
            }
        }

        CheckIfNext();*/

        if(leftDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool xPressed) && xPressed)
        {
            btnXLastState  =true;
        }
        else if(btnXLastState)
        {
            btnXLastState = false;
            if(!isGoingBack)
            {
                isGoingBack = true;
                blackBox.EnableBlackBoxMode();
                loadingScreen.gameObject.SetActive(true);
                loadingScreen.StartLoading("DivisionScene");
            }
        }


    }

    public void CanContinue()
    {
        sceneIndex += 1;
        canContinue = true;
    }

    private void CheckIfNext()
    {
        //Debug.Log("Entered CheckIfNext()");
        if(rightDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed) && isPressed)
        {
            btnALastState = true;
            //Debug.Log("\'A\' pressed");
        }
        else if (btnALastState)
        {
            btnALastState = false;
            CanContinue();
            //Debug.Log("\'A\' released");
            /*if(sceneIndex < displayText.Count)
            {
                CanContinue();
            }
            else
            {
                wasAPressed = true;
                isWaiting = !isWaiting;
            }*/
        } 
    }
}
