using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Photon_mode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //If there is a Settings manager, it will check if Photon is enable.
        //Depending of the result, it will active or disable different gameobject in the game.
        SettingsManager temp = FindObjectOfType<SettingsManager>();
        if (temp != null)
        {
            bool test = temp.IsPhotonEnabled();
            updatePhotonState(test);
        }
    }

    // Update is called once per frame
    void updatePhotonState(bool active)
    {
        //OptitrackRigidBody
        PhotonLauncher[] photonLauncherList = FindObjectsOfType<PhotonLauncher>();
        foreach (PhotonLauncher p in photonLauncherList)
        {
            p.enabled = active;

        }


        PhotonView[] photonViewList = FindObjectsOfType<PhotonView>();
        foreach (PhotonView p in photonViewList)
        {
            p.enabled = active;

        }

        PhotonTransformView[] photonTransformViewList = FindObjectsOfType<PhotonTransformView>();
        foreach (PhotonTransformView p in photonTransformViewList)
        {
            p.enabled = active;

        }
    }
}
