using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class DivisionProgression : SceneProgression
{
    public GameObject textBox;
    private TMPro.TextMeshProUGUI subText;
    private AudioSource sceneAudio;
    private List<string> displayText;
    public List<AudioClip> playAudio;

    public GameObject waitingScreen;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        sceneAudio = this.gameObject.GetComponent<AudioSource>();

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

        if(skipTuto)
        {
            sceneIndex = displayText.Count;
        }

    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();

        if(canContinue && !sceneAudio.isPlaying)
        {
            if(sceneIndex < displayText.Count)
            {
                Debug.Log("Step 1: instruction pt. " + (sceneIndex + 1));
                canContinue = false;
                textBox.SetActive(true);
                subText.SetText(displayText[sceneIndex]);
                sceneAudio.clip = playAudio[sceneIndex];
                sceneAudio.Play();
                CheckIfNext();
            }
            else if(sceneIndex == displayText.Count)
            {
                Debug.Log("Step 2: division");
                canContinue = false;
                textBox.SetActive(false);
                CheckIfNext();
            }

            else if(isWaiting && !isLoadingScene)
            {
                Debug.Log("Step 3: wait for next mission");
                canContinue = false;
                GoToWaitingZone();
                CheckIfNext();
            }

            else if(!isWaiting && !isLoadingScene)
            {
                canContinue = false;
                LoadNextScene();
            }
        }

        CheckIfNext();

        CheckIfPrecedent();
    }

    override protected void SkipInScene()
    {
        base.SkipInScene();
        sceneIndex = displayText.Count - 1;
        textBox.SetActive(false);
    }

    public override void SkipOnLoad()
    {
        base.SkipOnLoad();
        Start();
    }

    protected override void CanContinue()
    {
        base.CanContinue();
        if(sceneAudio.isPlaying)
            sceneAudio.Pause();
        if(sceneIndex > displayText.Count)
        {
            isWaiting = !isWaiting;
        }
    }

    protected override void GoBack()
    {
        base.GoBack();
        if(sceneAudio.isPlaying)
            sceneAudio.Pause();
    }
    
    protected override void GoToWaitingZone()
    {
        base.GoToWaitingZone();
        waitingScreen.SetActive(true);
    }

    protected override void ReturnToGame()
    {
        base.ReturnToGame();
        waitingScreen.SetActive(false);
    }

    protected override void LoadNextScene()
    {
        base.LoadNextScene();
        waitingScreen.SetActive(false);
    }

    protected override void LoadPreviousScene()
    {
        base.LoadPreviousScene();
        waitingScreen.SetActive(false);
    }
}
