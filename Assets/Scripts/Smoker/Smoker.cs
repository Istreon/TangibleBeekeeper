using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoker : MonoBehaviour
{
    [SerializeField]
    private GameObject smokePrefab;
    [SerializeField]
    private Transform smokeStartPosition;


    private float delay = 1.0f;
    private float time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(time<=delay)
        {
            time += Time.deltaTime;
        }
    }


    public void ReleaseSmoke()
    {
        if (time > delay)
        {
            time = 0.0f;
            GameObject smoke = Instantiate(smokePrefab, smokeStartPosition.position, smokeStartPosition.rotation);
            smoke.transform.parent = smokeStartPosition.transform;
        }
    }
}
