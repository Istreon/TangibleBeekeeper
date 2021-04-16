using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CatcherCursor : MonoBehaviour
{

    private bool catchThis;
    private GameObject catchableObject;
    private GameObject catchedObject;
    private bool catchableDetected=false;
    private bool catched = false;
	

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //Check if catching/releasing button is pushed
        if (Input.GetButtonDown("Fire1"))
        {
            catchThis = true;
        }
        else
        {
            catchThis = false;
        }

        //Catch an object
        if (catchableDetected && !catched)
        {
            //Check if catchable object is still near
            Vector3 holdPoint= catchableObject.GetComponent<Collider>().ClosestPointOnBounds(gameObject.transform.position);
            float dist = Vector3.Distance(gameObject.transform.position, holdPoint);
            //Catch an object
            if(catchThis && dist < 0.1f)
            {
                catchableObject.transform.parent = this.transform;
                catchedObject = catchableObject;
                catchedObject.GetComponent<Rigidbody>().isKinematic = true;
                catched = true;
            }
        } //Release an object
        else if (catched && catchThis)
        {
            catchedObject.transform.parent = null;
            catchedObject.GetComponent<Rigidbody>().isKinematic = false;
            catchedObject = null;
            catched = false;
        } 



    }



private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            //Detect catchable object and save it
            case "Catchable":
               // Debug.Log("Contact avec l'objet");
                catchableObject = other.gameObject;
                catchableDetected = true;
                break;

           
            default:
                break;

        }
    }
}
