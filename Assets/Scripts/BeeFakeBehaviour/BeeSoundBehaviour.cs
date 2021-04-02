using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSoundBehaviour : MonoBehaviour
{
    [SerializeField]
    private AudioSource beeSound;


    bool playing = false;
    float timer;
    float delay;
    // Start is called before the first frame update
    void Start()
    {
        beeSound.spatialize = true;
        beeSound.Stop();
        timer = 0;
        delay = Random.value*1.25f;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!playing)
        {
            timer += Time.deltaTime;
            if (timer > delay)
            {
                beeSound.Play();
                playing = true;
            }
        }

    }
}
