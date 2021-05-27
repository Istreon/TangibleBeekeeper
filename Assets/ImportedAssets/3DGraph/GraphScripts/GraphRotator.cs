using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphRotator : MonoBehaviour
{
    private Vector3 originPosition = Vector3.zero;
    private bool readyToRotate = false;

    public GameObject slider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            originPosition = Input.mousePosition;
            readyToRotate = true;
        }
        
        if(Input.GetMouseButton(0))
        {
            float previousYRotation = gameObject.transform.rotation.y;
            float dragDistance = Vector3.Distance(originPosition, Input.mousePosition);
            gameObject.transform.Rotate(0, -dragDistance/100, 0);
            slider.transform.Rotate(0, dragDistance/100, 0);
            originPosition = Input.mousePosition;
        }
        
        if(Input.GetMouseButtonUp(0))
        {
            readyToRotate = false;
        }
    }
}
