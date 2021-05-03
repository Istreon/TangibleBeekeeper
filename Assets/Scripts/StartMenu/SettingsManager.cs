using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{

    bool optitrackEnabled = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void EnableOptitrack(bool value)
    {
        optitrackEnabled = value;
    }

    public bool IsOptitrackEnabled()
    {
        return optitrackEnabled;
    }
}
