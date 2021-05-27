using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GraphAnchorsManager : MonoBehaviour
{
    public List<Transform> itemAnchors;

    public Transform playerAnchor;

    public float timeToCome = 1f;

    public float closePos = 1;
    public float dismissPos = 25;

    private Transform selectedAnchor = null;
    private Quaternion anchorStartRot;
    private Quaternion rotAtStart;
    private float timeAtStart = -1;
    private bool moving = false;
    [HideInInspector]
    public int keyPadPressed = -1;
    private Transform originTransform;

    public InputDeviceCharacteristics characteristics;
    private InputDevice device;
    public RingMenu ringMenu;
    [HideInInspector]
    public bool isClicking = false;
    private int keyToPress = -1;

    private Vector3 posAtStart;

    void Start()
    {
        originTransform = gameObject.transform;
        originTransform.eulerAngles = Vector3.zero;

        GetDevice();

    }

    void Update()
    {
        ringMenu.activeElement = -1;
        
        keyPadPressed = GetJoystickPosition();
        
        RotateGraph();

        /*if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            keyPadPressed = 0;
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            keyPadPressed = 1;
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            keyPadPressed = 2;
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            keyPadPressed = 3;
        }*/

    }

    public void GetDevice()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(characteristics, devices);
        if(devices.Count > 0)
        {
            device = devices[0];
        }
    }

    public int GetJoystickPosition()
    {
        if(device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joyValue))
        {
            keyPadPressed = -1;
            //Debug.Log("Joystick value = (" + joyValue.x + ", " + joyValue.y + ")");
            //isClicking = true;
            if(joyValue.y > 0.5f && joyValue.x < 0.5f && joyValue.x > -0.5f)
            {
                ringMenu.activeElement = 1;
                return 1;
            }
            else if(joyValue.x > 0.5f && joyValue.y < 0.5f && joyValue.y > -0.5f)
            {
                ringMenu.activeElement = 0;
                return 2;
            }
            else if( joyValue.y < -0.5f && joyValue.x < 0.5f && joyValue.x > -0.5f)
            {
                ringMenu.activeElement = 3;
                return 0;
            }
            else if(joyValue.x < -0.5f && joyValue.y < 0.5f && joyValue.y > -0.5f)
            {
                ringMenu.activeElement = 2;
                return 3;
            }
        }
        return -1;
    }

    public void RotateGraph()
    {
        //CHANGED EVERY selectedAnchor.root BY gameObject.transform
        
        if(keyPadPressed == 0)
        {
            //Debug.Log("Entered \'if(keyPadPressed == 0)\'");
            if(selectedAnchor == null)selectedAnchor = originTransform;
            rotAtStart = gameObject.transform.rotation; //selectedAnchor.root.rotation;
            anchorStartRot = originTransform.rotation; //selectedAnchor.rotation;
            timeAtStart = Time.realtimeSinceStartup;
            
            posAtStart = gameObject.transform.localPosition;
            //playerAnchor.localPosition = new Vector3(0, 0, closePos);

            moving = true;
        }
        else if(keyPadPressed != -1 && itemAnchors.Count >= keyPadPressed)
        {
            //Debug.Log("Entered \'else if(keyPadPressed != -1 && itemAnchors.Count >= keyPadPressed)\'");
            selectedAnchor = itemAnchors[keyPadPressed-1];
            rotAtStart = gameObject.transform.rotation;
            anchorStartRot = selectedAnchor.rotation;
            timeAtStart = Time.realtimeSinceStartup;

            posAtStart = gameObject.transform.localPosition;
            //playerAnchor.localPosition = new Vector3(0, 0, closePos);

            moving = true;
        }


        if(moving)
        {
            float t = (Time.realtimeSinceStartup - timeAtStart) / timeToCome;

            Quaternion lookRot = playerAnchor.rotation * Quaternion.Inverse(anchorStartRot) * rotAtStart;

            gameObject.transform.rotation = Quaternion.Slerp(rotAtStart, lookRot, t);

            //selectedAnchor.root.rotation = Quaternion.LookRotation(anchorStartForward, playerAnchor.up) * Quaternion.FromToRotation(rootStartForward, anchorStartForward);

            gameObject.transform.localPosition = Vector3.Lerp(posAtStart, selectedAnchor.localPosition, t);

            if (t >= 1)
            {
                moving = false;
            }
        }
    }
}
