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


    private Vector3 nextObjective;

    private Rigidbody beeBody;


    private Vector3 directionVector;


    private float delay=3.0f;
    private float time;


    // Start is called before the first frame update
    void Start()
    {
        nextObjective = this.transform.position;
        updateNextObjective();
        beeBody = GetComponent<Rigidbody>();
        time = 0;
 
    }

    // Update is called once per frame
    void Update()
    {



    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;
        if (Vector3.Distance(transform.position, nextObjective) > 0.05f && time<delay)
        {
            this.transform.LookAt(this.transform.position + beeBody.velocity);
            beeBody.velocity = directionVector * beeSpeed;
        }
        else
        {
            updateNextObjective();
        }
    }

    void updateNextObjective()
    {
        time=0;
        delay = Random.value * 10.0f + 3.0f;
        Vector3 temp = Random.insideUnitCircle * diameter;
        nextObjective = new Vector3(temp.x + areaOrigin.position.x, areaOrigin.position.y + Random.value * height * 2 - height, areaOrigin.position.z + temp.y);
        //this.transform.LookAt(nextObjective);
        directionVector = (nextObjective - this.transform.position).normalized;
    }
}
