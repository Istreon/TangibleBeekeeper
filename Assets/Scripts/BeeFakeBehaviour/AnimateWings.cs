using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*--This script activate different gameObject from an array in the same order as they are stored is the array. Only one gameobject is activate at each time*/
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

        foreach(GameObject w in wings)
        {
            w.SetActive(false);
        }
        wings[actualPosition].SetActive(true);
      

    }
}
