using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameTrackingSecurity : MonoBehaviour
{
    AboveHiveDetection roofAboveHive;
    FramePositioningDetection [] frames;
    // Start is called before the first frame update
    void Start()
    {
        roofAboveHive=this.GetComponent<AboveHiveDetection>();
        frames=FindObjectsOfType<FramePositioningDetection>();
        Debug.Log("Nombre de cadre trouvé : " + frames.Length);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(roofAboveHive!=null)
        {
            //Test if the roof hides the inside of the hive
            bool hides=roofAboveHive.objectHidesInsideOfTheHive();
            foreach (FramePositioningDetection f in frames)
            {
                //Check if the frame is inside of the hive
                bool inHive = f.isInTheHive();
                OptitrackRigidBody o = f.gameObject.GetComponent<OptitrackRigidBody>();
                if (o != null)
                {
                    if (inHive)  //If the frame is in the hive, disable optitrack tracking if the roof is hiding the inside of the hive
                    {
                        o.enabled = !hides;
                    }
                    else //If the frame is not in the hive, let the optitrack tracking enable
                    {
                        o.enabled = true;
                    }
                }
            }
        }
    }
}
