using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private Vector3 previousPosition;

    private Queue<float> lastSpeeds = new Queue<float>();
    private int maxValues = 5;
    // Start is called before the first frame update
    void Start()
    {
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dist = Vector3.Distance(previousPosition, transform.position);
        float speed= dist / Time.deltaTime;
        if (lastSpeeds.Count >= maxValues) lastSpeeds.Dequeue();
        lastSpeeds.Enqueue(speed);


        previousPosition = transform.position;
    }

    public float getSpeed()
    {
        float[] tab = new float[lastSpeeds.Count];
        lastSpeeds.CopyTo(tab, 0);
        float speed = 0;
        foreach (float f in tab)
        {
            speed += f;
        }
        speed = speed / (tab.Length);
        return speed;
    }
}
