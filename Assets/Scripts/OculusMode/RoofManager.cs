using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofManager : MonoBehaviour
{
    private GrabInteractor grabInteractor;
    

    // Start is called before the first frame update
    void Start()
    {
        grabInteractor = GetComponent<GrabInteractor>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetOnHive(Transform roofPosition, HiveManager hiveManager)
    {
        if(!grabInteractor.firstGrab && ! grabInteractor.secondGrab)
        {
            gameObject.transform.position = roofPosition.position;
            gameObject.transform.rotation = roofPosition.rotation;

            //hiveManager.PutRoofOn();
        }
    }
}
