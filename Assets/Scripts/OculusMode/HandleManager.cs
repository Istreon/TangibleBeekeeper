using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class HandleManager : MonoBehaviour
{
    private Rigidbody handleBody = null;
    private Renderer objectRend;
    public bool showHandle = false;
    /*public Material unhovered;
    public Material hovered;
    //public Material selected;*/

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
        grabManager = parentTransform.gameObject.GetComponent<GrabInteractor>();

        this.transform.parent = null;
        handleBody = this.GetComponent<Rigidbody>();
        handleBody.isKinematic = false;

        if(showHandle)
        {
            //objectRend = gameObject.GetComponent<Renderer>();
            //objectRend.material = unhovered;
            grabManager.InitializeFrameMaterial();
            //interactiveGrabber = this.gameObject.GetComponent<InteractiveGrabber>();
            //interactiveGrabber.onSelectEntered.AddListener(delegate{SetSelectedMaterial();});
            //interactiveGrabber.onSelectExited.AddListener(delegate{SetPreviousMaterial();});
        }

        originTransform = this.gameObject.transform;
        
        posDiff = originTransform.position - parentTransform.position;
        //rotDiff = parentTransform.eulerAngles - originTransform.eulerAngles;

        isHovered = false;
        isSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*//Debug.Log("isHovered = " + isHovered + " ; isSelected = " + isSelected);
        if(showHandle)
        {
            if(isHovered | grabManager.firstGrab | grabManager.secondGrab)
            {
                //Debug.Log("Update.if loops entered");
                //objectRend.material = unhovered;
                foreach (Renderer rend in frameParts)
                {
                    rend.material = hovered;
                }
            }
            else
            {
                foreach (Renderer rend in frameParts)
                {
                    rend.material = unhovered;
                }
            }
            
        }*/
        
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
        if(showHandle /*&& !isSelected*/)
        {
            if(other.gameObject.TryGetComponent<HandInteractor>(out HandInteractor interactor))
            {
                grabManager.HoverFrame();
                /*Debug.Log("OnTriggerEnter with interactor " + other.gameObject.name + " on grabpoint " + this.gameObject.name);
                isHovered = true;
                //Debug.Log("All OnTriggerEnter() conditions validated and isHovered = " + isHovered);
                //objectRend.material = hovered;
                foreach (Renderer rend in frameParts)
                {
                    //Debug.Log("OnTriggerEnter.foreach loop for renderer " + rend.gameObject.name);
                    rend.material = hovered;
                }*/
            }
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        /*if(showHandle && !isSelected)
        {
            if(other.gameObject.TryGetComponent<HandInteractor>(out HandInteractor interactor))
            {
                isHovered = true;
                //objectRend.material = hovered;
                foreach (Renderer rend in frameParts)
                {
                    rend.material = hovered;
                }
            }
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        if(showHandle /*&& !isSelected*/)
        {
            if(other.gameObject.TryGetComponent<HandInteractor>(out HandInteractor interactor))
            {
                grabManager.UnhoverFrame();
                /*isHovered = false;
                //Debug.Log("All OnTriggerExit() conditions validated and isHovered = " + isHovered);
                //objectRend.material = unhovered;
                foreach (Renderer rend in frameParts)
                {
                    //Debug.Log("OnTriggerExit.foreach loop for renderer " + rend.gameObject.name);
                    rend.material = unhovered;
                }*/
            }
        }
    }

    public void SetSelectedMaterial()
    {
        grabManager.SelectFrame();
        /*//previousMaterial = objectRend.material;
        //objectRend.material = selected;
        foreach (Renderer rend in frameParts)
        {
            rend.material = hovered;
        }
        isSelected = true;
        //Debug.Log("OnSelectEntered() on grabpoint " + this.gameObject.name + " and isSelected = " + isSelected);*/
    }

    public void SetPreviousMaterial()
    {
        grabManager.UnselectFrame();
        /*//objectRend.material = previousMaterial;
        //objectRend.material = unhovered;
        //if(!grabManager.firstGrab && !grabManager.secondGrab)
        //{
            foreach (Renderer rend in frameParts)
            {
                rend.material = unhovered;
            }
            isHovered = false;
            isSelected = false;
        //}*/
        
        //Debug.Log("OnSelectExited() on grabpoint " + this.gameObject.name + " and isSelected = " + isSelected);
        
    }

}
