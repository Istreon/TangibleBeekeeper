using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateWings : MonoBehaviour
{

    [SerializeField]
    private GameObject [] wings;



    int actualPosition = 0;
    int nbPosition = 0;
    int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        nbPosition = wings.Length;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (actualPosition == nbPosition - 1) direction = -1;
        if (actualPosition == 0) direction = 1;
        actualPosition = actualPosition + direction % nbPosition;
        /*
        this.transform.localPosition = positions[actualPosition].localPosition;
        this.transform.localRotation = positions[actualPosition].localRotation;*/

        foreach(GameObject w in wings)
        {
            w.SetActive(false);
        }
        wings[actualPosition].SetActive(true);
      

    }
}
