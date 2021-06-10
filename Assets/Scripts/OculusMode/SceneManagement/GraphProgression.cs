﻿using System.Collections;
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
    private int remainingGraph = 2;
    
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
        canContinue = true;

        displayText = new List<string>();
        subText = textBox.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        string[] lines = System.IO.File.ReadAllLines("Assets/Audio/GraphScene/TTS.txt");
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
            
            else
            {
                Debug.Log("Step 2: graph study");
                textBox.SetActive(false);
                CheckIfNext();
            }
        }

        if(remainingGraph <= 0)
        {
            Debug.Log("Step 3: end of the simulation");
            endScreen.SetActive(true);
            blackBox.EnableBlackBoxMode();
            ambientSound.Stop();
            //LOAD LOBBY SCENE TO START OVER
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
                remainingGraph -= 1;
            }
        }

        CheckIfNext();
    }

    public void CanContinue()
    {
        sceneIndex += 1;
        canContinue = true;
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
            Debug.Log("\'A\' released");
            if(sceneIndex < displayText.Count)
            {
                CanContinue();
            }
            else
            {
                wasAPressed = true;
                nbOfAPressed += 1;
            }
        } 
    }
}
