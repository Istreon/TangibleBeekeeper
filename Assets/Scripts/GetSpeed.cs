using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSpeed : MonoBehaviour
{
    float speed = 0.0f;

    private Vector3 lastPosition;


    Rigidbody body;

    float delay = 1.0f;
    float time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = this.transform.position;
        body=this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float dist=Vector3.Distance(lastPosition, this.transform.position);

        speed += dist;
        time += Time.deltaTime;
        lastPosition = this.transform.position;

        if(time>=delay)
        {
            speed = speed / time;
            Debug.Log("Vitesse " + speed);

            time = 0.0f;
            speed = 0.0f;
        }
    }
}
