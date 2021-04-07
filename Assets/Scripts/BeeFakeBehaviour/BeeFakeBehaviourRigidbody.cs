using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeFakeBehaviourRigidbody : MonoBehaviour
{
    [SerializeField]
    private GameObject beeArea;
    private Collider areaCollider;

    [SerializeField]
    private float beeSpeed;

    private float diameter = 3.0f;
    private float height = 0.7f;


    private Rigidbody beeBody;

    private bool tooFar = false;
    private float power = 0.2f;


    // Start is called before the first frame update
    void Start()
    {
        beeBody = GetComponent<Rigidbody>();
        areaCollider = beeArea.GetComponent<Collider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(tooFar)
        {
            //AddForceDirectedToAreaCenter
            Vector3 dir = ((beeArea.transform.position - this.transform.position).normalized)/1000;
            beeBody.AddForce(dir*power);
        }

        beeBody.AddForce(new Vector3((Random.value-0.5f)/1000.0f, (Random.value-0.5f)/1000.0f, (Random.value-0.5f)/1000.0f));
        this.transform.LookAt(this.transform.position + beeBody.velocity);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BeeArea")) tooFar = false; ;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BeeArea"))  tooFar=true;
    }

}
