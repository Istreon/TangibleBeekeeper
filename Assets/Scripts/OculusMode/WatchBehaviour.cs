using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchBehaviour : MonoBehaviour
{
    private Collider mCollider = null;
    public GameObject sampleGraph;
    public Collider watchCollider;

    // Start is called before the first frame update
    void Start()
    {
        sampleGraph.SetActive(false);
        mCollider = GetComponent<Collider>();
        mCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other == watchCollider)
        {
            sampleGraph.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other == watchCollider)
        {
            sampleGraph.SetActive(false);
        }
    }
}
