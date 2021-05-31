using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneycombVisualSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject [] orderedVisuals;

    private int tabSize;
    private int actualPosition = 0;
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


    public void NextVisual()
    {
        if(tabSize>0)
        {
            actualPosition = (actualPosition + 1) % tabSize;
            foreach (GameObject g in orderedVisuals)
            {
                if (g != null)
                    g.SetActive(false);
            }
            if (orderedVisuals[actualPosition] != null)
                orderedVisuals[actualPosition].SetActive(true);
        }
        
    }
}
