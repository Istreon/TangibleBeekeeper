using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class HandleManager : MonoBehaviour
{
    private Rigidbody handleBody = null;
    private Renderer objectRend;
    public bool showHandle = false;
    public Material unselected;
    public Material selected;

    
    
    // Start is called before the first frame update
    void Start()
    {
        this.transform.parent = null;
        handleBody = this.GetComponent<Rigidbody>();
        handleBody.isKinematic = false;

        if(showHandle)
        {
            objectRend = gameObject.GetComponent<Renderer>();
            objectRend.material = unselected;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(this.transform.parent!=null)
        {
            //Held cuff
            enableConstraints();
        }
        else
        {
            //not Held cuff
            disableConstraints();
        }
    }


    private void enableConstraints()
    {
        handleBody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationZ;


    }

    private void disableConstraints()
    {
        handleBody.constraints = RigidbodyConstraints.None;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(showHandle)
        {
            if(other.gameObject.TryGetComponent<HandInteractor>(out HandInteractor interactor))
            {
                objectRend.material = selected;
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(showHandle)
        {
            if(other.gameObject.TryGetComponent<HandInteractor>(out HandInteractor interactor))
            {
                objectRend.material = unselected;
            }
        }
    }

}
