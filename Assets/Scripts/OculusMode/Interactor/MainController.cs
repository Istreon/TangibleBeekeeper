using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MainController : XRController
{
   public HandPresence handPresenceManager;
   public List<GameObject> handChildren;
   public List<GameObject> controllerChildren;

   public void ShowController(bool show)
   {
     //handPresenceManager.showController = show;
     foreach (GameObject child in handChildren)
     {
         child.SetActive(!show);
     }
     foreach (GameObject child in controllerChildren)
     {
         child.SetActive(show);
     }
   }
   
}
