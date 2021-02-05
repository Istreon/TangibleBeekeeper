using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveRoof : MonoBehaviour
{
    [SerializeField]
    private Transform roofOriginalPosition;

    private bool wellPositioned = false;

    private float timePassed = 0;
    private float frequency = 0.25f;


    private float sensibility=0.01f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
          timePassed += Time.deltaTime;
          if(timePassed>=frequency)
          {
              timePassed = 0;

            float dist = Vector3.Distance(roofOriginalPosition.position, gameObject.transform.position);
            if (!wellPositioned && dist <= sensibility)
            {
                Debug.Log("Roof is in place");
                wellPositioned = true;
            } else if (wellPositioned && dist > sensibility)
            {
                Debug.Log("Roof is not in place");
                wellPositioned = false;
            }
            }
        
    }
}
