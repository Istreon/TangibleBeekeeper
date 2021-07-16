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

    [HideInInspector]
    public bool isHovered;

    public Transform parentTransform;
    private Transform originTransform;
    private Vector3 posDiff;
    private Vector3 rotDiff;
    private GrabInteractor grabManager;
    
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

        originTransform = this.gameObject.transform;
        
        posDiff = parentTransform.position - originTransform.position;
        //rotDiff = parentTransform.eulerAngles - originTransform.eulerAngles;

        grabManager = parentTransform.gameObject.GetComponent<GrabInteractor>();
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
        if(showHandle && !grabManager.firstGrab && !grabManager.secondGrab)
        {
            this.gameObject.transform.eulerAngles = parentTransform.eulerAngles;
            this.gameObject.transform.position = parentTransform.position + posDiff;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(showHandle)
        {
            if(other.gameObject.TryGetComponent<HandInteractor>(out HandInteractor interactor))
            {
                objectRend.material = selected;
                isHovered = true;
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
                isHovered = false;
            }
        }
    }

}
