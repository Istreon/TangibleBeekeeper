using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class HandleManager : MonoBehaviour
{
    private Rigidbody handleBody = null;
    private Renderer objectRend;
    public bool showHandle = false;
    public Material unhovered;
    public Material hovered;
    public Material selected;

    [HideInInspector]
    public bool isHovered;

    public Transform parentTransform;
    private Transform originTransform;
    private Vector3 posDiff;
    private Vector3 rotDiff;
    private GrabInteractor grabManager;
    private InteractiveGrabber interactiveGrabber;
    private Material previousMaterial;
    private bool isSelected;
    
    // Start is called before the first frame update
    void Start()
    {
        this.transform.parent = null;
        handleBody = this.GetComponent<Rigidbody>();
        handleBody.isKinematic = false;

        if(showHandle)
        {
            objectRend = gameObject.GetComponent<Renderer>();
            objectRend.material = unhovered;
            interactiveGrabber = this.gameObject.GetComponent<InteractiveGrabber>();
            interactiveGrabber.onSelectEntered.AddListener(delegate{SetSelectedMaterial();});
            interactiveGrabber.onSelectExited.AddListener(delegate{SetPreviousMaterial();});
        }

        originTransform = this.gameObject.transform;
        
        posDiff = originTransform.position - parentTransform.position;
        //rotDiff = parentTransform.eulerAngles - originTransform.eulerAngles;

        grabManager = parentTransform.gameObject.GetComponent<GrabInteractor>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isHovered && !isSelected)
        {
            objectRend.material = unhovered;
        }
        
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
            this.gameObject.transform.rotation = parentTransform.rotation;
            this.gameObject.transform.position = parentTransform.position + posDiff;
    
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(showHandle && !isSelected)
        {
            if(other.gameObject.TryGetComponent<HandInteractor>(out HandInteractor interactor))
            {
                objectRend.material = hovered;
                isHovered = true;
            }
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(showHandle && !isSelected)
        {
            if(other.gameObject.TryGetComponent<HandInteractor>(out HandInteractor interactor))
            {
                objectRend.material = hovered;
                isHovered = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(showHandle && !isSelected)
        {
            if(other.gameObject.TryGetComponent<HandInteractor>(out HandInteractor interactor))
            {
                objectRend.material = unhovered;
                isHovered = false;
            }
        }
    }

    private void SetSelectedMaterial()
    {
        //previousMaterial = objectRend.material;
        objectRend.material = selected;
        isSelected = true;
    }

    private void SetPreviousMaterial()
    {
        //objectRend.material = previousMaterial;
        objectRend.material = unhovered;
        isHovered = false;
        isSelected = false;
    }

}
