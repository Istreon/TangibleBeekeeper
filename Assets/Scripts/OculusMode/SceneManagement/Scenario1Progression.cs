using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Scenario1Progression : SceneProgression
{
    public BeesData dataModel;
    public GameObject endScreen;
    
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        dataModel.NextScenario();

    }

    // Update is called once per frame
    override protected void Update()
    {
        if(canContinue)
        {
            if(sceneIndex == 0)
            {
                canContinue = false;
                Debug.Log("Step 2: graph study");
                CheckIfNext();
            }
            else if(isWaiting && !isLoadingScene)
            {
                canContinue = false;
                Debug.Log("End screen activated");
                GoToWaitingZone();
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

    protected override void CanContinue()
    {
        base.CanContinue();
        isWaiting = !isWaiting;
    }

    protected override void GoToWaitingZone()
    {
        base.GoToWaitingZone();
        endScreen.SetActive(true);
    }

    protected override void ReturnToGame()
    {
        base.ReturnToGame();
        endScreen.SetActive(false);
    }

    protected override void LoadNextScene()
    {
        foreach (AudioSource audio in ambientSound)
        {
            audio.Stop();
        }
        blackBox.EnableBlackBoxMode();
        endScreen.SetActive(false);
        loadingScreen.StartLoading(0, false);
        isLoadingScene = true;;
    }

    protected override void LoadPreviousScene()
    {
        base.LoadPreviousScene();
        endScreen.SetActive(false);
    }

}
