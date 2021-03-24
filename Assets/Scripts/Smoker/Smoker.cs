using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoker : MonoBehaviour
{
    [SerializeField]
    private GameObject smokePrefab;
    [SerializeField]
    private Transform smokeStartPosition;

    //Smoke release parameters
    private float delayBetweenSmoke = 0.01f;
    private float timeSinceLastSmoke = 0.0f;

    //Smoker animation parameters
    [SerializeField]
    private GameObject pumpClassicVersion;
    [SerializeField]
    private GameObject pumpPressedVersion;

    private bool pumpActivated=false;
    private float activationDuration=0.4f;
    private float activationTime=0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(timeSinceLastSmoke<=delayBetweenSmoke)
        {
            timeSinceLastSmoke += Time.deltaTime;
        }

        if(pumpActivated) {
            activationTime+= Time.deltaTime;
            if(activationTime>activationDuration) {
                pumpActivated=false;
                SmokerAnimation(false);
            }
        }
    }


    public void ReleaseSmoke()
    {
        if (timeSinceLastSmoke > delayBetweenSmoke)
        {
            timeSinceLastSmoke = 0.0f;
            GameObject smoke = Instantiate(smokePrefab, smokeStartPosition.position, smokeStartPosition.rotation);
            smoke.transform.parent = smokeStartPosition.transform;

            //Change visual smoker state
            pumpActivated=true;
            activationTime = 0.0f;
            SmokerAnimation(true);
        }
    }

    public void SmokerAnimation(bool activate) {
        pumpClassicVersion.SetActive(!activate);
        pumpPressedVersion.SetActive(activate);
    }
}
