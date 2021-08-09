using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HoneycombVisualSelector : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    private GameObject [] orderedVisuals;

    private int tabSize;
    private int actualPosition = 0;


    #region IPunObservable implementation


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(actualPosition);
        }
        else
        {
            // Network player, receive data
            this.actualPosition = (int)stream.ReceiveNext();
        }
    }


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        tabSize = orderedVisuals.Length;
        foreach(GameObject g in orderedVisuals)
        {
            if(g!=null)
                g.SetActive(false);
        }
        if(tabSize>0)
        {
            if(orderedVisuals[0] != null)
                orderedVisuals[0].SetActive(true);
        }
    }


    private void FixedUpdate()
    {
        UpdateVisual();
    }
        

    public void NextVisual()
    {
        if(tabSize>0)
        {
            actualPosition = (actualPosition + 1) % tabSize;
        }  
    }

    public void UpdateVisual()
    {
        foreach (GameObject g in orderedVisuals)
        {
            if (g != null)
                g.SetActive(false);
        }
        if (orderedVisuals[actualPosition] != null)
            orderedVisuals[actualPosition].SetActive(true);
    }
}
