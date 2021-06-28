using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeFakeBehaviourRigidbody : MonoBehaviour
{
    [SerializeField]
    private GameObject beeArea;

    [SerializeField]
    private float beeSpeed;
    [SerializeField]
    private float maxSpeed = 3.0f;

    private Rigidbody beeBody;

    private bool tooFar = false;
    private float power = 0.2f;




    // Start is called before the first frame update
    void Start()
    {
        beeBody = GetComponent<Rigidbody>();
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

        //Add random force to the bee
        beeBody.AddForce(new Vector3((Random.value-0.5f)/1000.0f, (Random.value-0.5f)/1000.0f, (Random.value-0.5f)/1000.0f));
        //Update look orientation of the bee
        this.transform.LookAt(this.transform.position + beeBody.velocity);

        float temp = Vector3.Distance(beeBody.velocity, new Vector3());

        //Limit bee speed to a max value
        if(temp>maxSpeed)
        {
            beeBody.velocity = (beeBody.velocity.normalized) * maxSpeed;
        }

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
