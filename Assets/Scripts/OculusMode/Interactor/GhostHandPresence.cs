using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GhostHandPresence : MonoBehaviour
{
    private Animator ghostAnimator = null;

    private Transform ghostTransform = null;

    private Transform parent = null;

    public InputDeviceCharacteristics controllerCharacteristics;
    private InputDevice targetDevice;
    
    public bool isLeft = true;
    private float repositionFactor = 1;


    // Start is called before the first frame update
    void Start()
    {
        TryInitialyze();
        if(!isLeft)
        {
            repositionFactor = -1;
        }
        else
        {
            repositionFactor = 1;
        }
        parent = gameObject.transform.parent;
    }

    void TryInitialyze()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }
        if(devices.Count > 0){
            targetDevice = devices[0];
            ghostAnimator = gameObject.GetComponent<Animator>();
            ghostTransform = gameObject.transform;
        }
    }

    void UpdateHandAnimation()
    {
        if(!targetDevice.isValid)
        {
            TryInitialyze();
        }
        else
        {
            if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                ghostAnimator.SetFloat("Pinch", triggerValue);
            }
            else
            {
                ghostAnimator.SetFloat("Pinch", 0.0f);
            }
            
            if(targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            {
                ghostAnimator.SetFloat("Flex", gripValue);
            }
            else
            {
                ghostAnimator.SetFloat("Flex", 0.0f);
            }
        }
           
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHandAnimation();
    }

    public void SetAnchor(Transform anchor)
    {
        gameObject.transform.SetParent(anchor);
    }

    public void ClearAnchor(Transform anchor)
    {
        gameObject.transform.SetParent(parent);
        gameObject.transform.position= parent.GetComponent<XRController>().modelTransform.position;
        gameObject.transform.rotation = parent.GetComponent<XRController>().modelTransform.rotation;
        gameObject.transform.Rotate(0.0f, 0.0f, repositionFactor * 90.0f);
        gameObject.transform.Translate(new Vector3(-0.001f * repositionFactor, 0.001f * repositionFactor, -0.035f));
    }


}
