using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisorBehaviour : MonoBehaviour
{
    public SceneProgression sceneProgression;
    public GameObject displayVisor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == displayVisor)
        {
            sceneProgression.isPuttingVisor = true;
        }
    }
}
