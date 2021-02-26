using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryScript : MonoBehaviour
{
    
    [SerializeField]
    private float step = 0.01f;
    [SerializeField]
    private Transform optitrack_reference = null;
    [SerializeField]
    private Transform vive_reference = null;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            this.transform.position += new Vector3(-step, 0.0f, 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            this.transform.position += new Vector3(step, 0.0f, 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.transform.position += new Vector3(0.0f, -step, 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            this.transform.position += new Vector3(0.0f, step, 0.0f);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            this.transform.position += new Vector3(0.0f, 0.0f, -step);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            this.transform.position += new Vector3(0.0f, 0.0f, step);
        }
        

        if(optitrack_reference!=null && vive_reference!=null)
        {
            //Vector3 position = vive_headset.position - opti_headset.position;
            //Vector3 rotation = vive_headset.rotation.eulerAngles - opti_headset.rotation.eulerAngles;

            //Debug.Log("Position : " + position + " Rotation : " +  rotation);
           // Debug.Log(position.x + "  " + position.y + "  " + position.z);
           // this.transform.position += position;
            //Debug.Log(this.transform.position);
        }
    }
}
