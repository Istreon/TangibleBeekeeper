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

    private bool pumpActivated=false;
    private float activationDuration=0.4f;
    private float activationTime=0.0f;
    


    [Header("Wood planch and his open/closed references")]
    [SerializeField]
    private GameObject woodPlanch;
    [SerializeField]
    private Transform openWoodPlanch;
    [SerializeField]
    private Transform closedWoodPlanch;

    [Header("Leather part and his open/closed references")]
    [SerializeField]
    private GameObject leatherPart;
    [SerializeField]
    private Transform openLeather;
    [SerializeField]
    private Transform closedLeather;



    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float state;




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
        if(pumpActivated)
        {
            activationTime += Time.deltaTime;
            if (activationTime > activationDuration)
            {
                pumpActivated = false;
                state = 0.0f;
            }
        }
        SmokerAnimation();
    }


    public void ReleaseSmoke()
    {
        if (timeSinceLastSmoke > delayBetweenSmoke)
        {
            timeSinceLastSmoke = 0.0f;
            GameObject smoke = Instantiate(smokePrefab, smokeStartPosition.position, smokeStartPosition.rotation);
            smoke.transform.parent = smokeStartPosition.transform;

            pumpActivated = true;
            activationTime = 0.0f;
            state = 1.0f;
        }

       
    }

    public void SmokerAnimation()
    {
        woodPlanch.transform.localPosition = Vector3.Lerp(openWoodPlanch.localPosition, closedWoodPlanch.localPosition, state);
        woodPlanch.transform.localRotation = Quaternion.Lerp(openWoodPlanch.localRotation, closedWoodPlanch.localRotation, state);

        leatherPart.transform.localPosition = Vector3.Lerp(openLeather.localPosition, closedLeather.localPosition, state);
        leatherPart.transform.localRotation = Quaternion.Lerp(openLeather.localRotation, closedLeather.localRotation, state);
        leatherPart.transform.localScale = Vector3.Lerp(openLeather.localScale, closedLeather.localScale, state);
    }
}
