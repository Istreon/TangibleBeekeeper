using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    private CharacterController controller;
    public float speed = 12f;

    Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        if (controller == null) Debug.LogError("Missing CharacterController component");
    }

    // Update is called once per frame
    void Update()
    {

        
        //Get horizontal input to rotate
        float x = Input.GetAxis("Horizontal");
        //Get vertical input to move forward
        float z = Input.GetAxis("Vertical");


        Vector3 move = transform.forward * z; 
        transform.Rotate(Vector3.up * x);
        controller.Move(move * speed * Time.deltaTime);
   




    }


}
