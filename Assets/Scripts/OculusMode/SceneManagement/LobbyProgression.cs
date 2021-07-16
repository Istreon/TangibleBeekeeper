using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LobbyProgression : SceneProgression
{
    protected override void Start()
    {
        base.Start();
        isWaiting = true;

    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();

        if(isWaiting && !isLoadingScene)
        {
            CheckIfNext();
        }
        
        if(!isWaiting && !isLoadingScene)
        {
            LoadNextScene();
        }
    }

    override protected void CanContinue()
    {
        base.CanContinue();
        isWaiting = false;
    }
}
