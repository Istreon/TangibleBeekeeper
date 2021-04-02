using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookInteractive : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 100f;
    public Transform cursorbody; //Contains the cursor

    float zDistance = 0.5f;  //Distance between this.gameObject and cursor
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Get the mouse position change
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;


        //Update distance by checking mouse scroll change
        zDistance += Input.mouseScrollDelta.y * 0.1f;
        if (zDistance < 0.5f) zDistance = 0.5f;

        //Update cursor position
        cursorbody.localPosition =
            new Vector3(cursorbody.localPosition.x+mouseX, cursorbody.localPosition.y + mouseY, zDistance);

        //Reset mouse position
        if(Input.GetButtonDown("Fire2"))
            cursorbody.localPosition =
           new Vector3(0f,0f , zDistance);
    }
}
