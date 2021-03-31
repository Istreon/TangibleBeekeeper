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


    // Start is called before the first frame update
    void Start()
    {
        nextObjective = this.transform.position;
        beeBody = GetComponent<Rigidbody>();
 
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, nextObjective) > 0.05f)
        {

        }
        else
        {
            updateNextObjective();
        }


    }

    void updateNextObjective()
    {
        Vector3 temp = Random.insideUnitCircle * diameter;
        nextObjective = new Vector3(temp.x + areaOrigin.position.x, areaOrigin.position.y + Random.value * height * 2 - height, areaOrigin.position.z + temp.y);

        Vector3 velocityVector = nextObjective.normalized*beeSpeed;
        this.transform.LookAt(nextObjective);

        beeBody.velocity = velocityVector;
    }
}
