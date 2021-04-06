using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeFakeBehaviourRigidbody : MonoBehaviour
{
    [SerializeField]
    private Transform areaOrigin;

    [SerializeField]
    private float beeSpeed;

    private float diameter = 3.0f;
    private float height = 0.7f;


    private Rigidbody beeBody;



    // Start is called before the first frame update
    void Start()
    {
        beeBody = GetComponent<Rigidbody>();
 
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        beeBody.AddForce(new Vector3((Random.value-0.5f)/1000.0f, (Random.value-0.5f)/1000.0f, (Random.value-0.5f)/1000.0f));
        this.transform.LookAt(this.transform.position + beeBody.velocity);

    }

}
