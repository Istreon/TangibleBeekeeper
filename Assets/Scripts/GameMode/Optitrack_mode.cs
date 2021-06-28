using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optitrack_mode : MonoBehaviour
{

    private void Start()
    {
        //If there is a Settings manager, it will check if Optitrack is enable.
        //Depending of the result, it will active or disable different gameobject in the game.
        SettingsManager temp=FindObjectOfType<SettingsManager>();
        if(temp!=null)
        {
            bool test = temp.IsOptitrackEnabled();
            updateTrackedObjectState(test);
        }

    }

    void OnEnable()
    {
        updateTrackedObjectState(true);
    }

    void OnDisable()
    {
        updateTrackedObjectState(false);
    }

    private void updateTrackedObjectState(bool active)
    {
        //Childs
        Transform [] childList=GetComponentsInChildren<Transform>();
        foreach (Transform t in childList)
        {
            if(!this.gameObject.Equals(t.gameObject))
            {
                t.gameObject.SetActive(active);
            } 
        }

        //OptitrackRigidBody
        OptitrackRigidBody [] optitrackList=FindObjectsOfType<OptitrackRigidBody>();
        foreach (OptitrackRigidBody o in optitrackList)
        {
            o.enabled = active;

            Rigidbody r = o.GetComponent<Rigidbody>();
            if (r != null)
            {
                r.isKinematic = active;
            }
        }
        
    }
}
