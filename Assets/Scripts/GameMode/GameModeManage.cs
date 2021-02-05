using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
/**
 * Change game mode according to the connected equipment
 */
public class GameModeManage : MonoBehaviour
{

    [SerializeField]
    private bool activeOptiTrack = false;

    [SerializeField]
    private GameObject VR_Mode;

    [SerializeField]
    private GameObject MouseKeyBoard_Mode;

    [SerializeField]
    private GameObject OptiTrack_Mode;



    

    // Update is called once per frame
    void FixedUpdate()
    {

        bool vrPresent = false;

        //Check if VR headset is connected
        InputDevice headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head); 
        if (headDevice.isValid == true)
        {
            //Debug.Log("Manager : Active Device");
            vrPresent = true;
            //bool presenceFeatureSupported = headDevice.TryGetFeatureValue(CommonUsages.userPresence, out userPresent);
            //Debug.Log("Manager : User : "+userPresent  + " Feature supported : "  + presenceFeatureSupported);

        }



        //Enable or disable mode
        VR_Mode.SetActive(vrPresent);
        XRSettings.enabled=vrPresent;
        MouseKeyBoard_Mode.SetActive(!vrPresent);
        OptiTrack_Mode.SetActive(activeOptiTrack);


    }


}
