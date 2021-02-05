using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class ShockDetection : MonoBehaviour
{
    private Vector3 previousPosition;
    private float previousSpeed;

    [SerializeField]
    private float sensibility = 400f; //Correspond to the minimum acceleration to detect action

    private float timer = 0.5f; //Correspond to high limit time of the action ( analysis time in seconds )

    private bool inAction = false;

    private Queue<float> lastAccele = new Queue<float>();
    private int maxValues = 5;

    // Start is called before the first frame update
    void Start()
    {
        float r = Random.Range(0, 100);
        previousPosition = transform.position;
        previousSpeed = 0f;
    }


    void FixedUpdate()
    {
        detectChock();
    }


    private void endedAction()
    {
        inAction = false;
        timer = 0.5f;
    }
 

    //Detect shock by calculating acceleration
    private void detectChock()
    {
        float dist = Vector3.Distance(previousPosition, transform.position);
        float speed = dist / Time.deltaTime;
        float accele = (speed - previousSpeed) / Time.deltaTime;

        if (lastAccele.Count >= maxValues) lastAccele.Dequeue();
        lastAccele.Enqueue(Mathf.Abs(accele));

       /* float temp=getAcceleration();
        if (temp > sensibility)
        {
            Debug.Log("There was a hit (accele = " + temp + " )");
            if(textArea!=null) textArea.GetComponent<TextMesh>().text = temp.ToString();
        }*/
       
        //Started action
         if (accele>= sensibility)
         {
             inAction = true;
         }

         //Second phase of the action
         if (inAction && (accele <= (-sensibility)))
         {
             Debug.Log("There was a hit (accele = " + accele + " )");
            endedAction();
         }


         //Timer update
         if (inAction)
         {
             timer -= Time.deltaTime;
         }

         //Ended timer
         if (timer <= 0)
         {
             endedAction();
         }

        previousPosition = transform.position;
        previousSpeed = speed;
    }


    //Detect shock with other objects
    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 1) Debug.Log("Collision with another object");
    }

    void OnTriggerEnter(Collider other)
    {
        Hand h=other.gameObject.GetComponent<Hand>();
        if(h!=null)
        {
            float speed = h.getSpeed();
            //Debug.Log("ok : " + speed);
            if (speed > 0.9) Debug.Log("Shock detected : " + speed);
           
        }
    }


    public float getAcceleration()
    {
        float[] tab = new float[lastAccele.Count];
        lastAccele.CopyTo(tab, 0);
        float accele = 0;
        foreach (float f in tab)
        {
            accele += f;
        }
        accele = accele / (tab.Length);
        return accele;
    }
}


