using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SmokerInteraction : MonoBehaviour
{
    private List<InputDevice> interactorDevices;
    // Start is called before the first frame update
    void Start()
    {
        interactorDevices = new List<InputDevice>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in interactorDevices)
        {
            if(item.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                OnTriggeredSmoker();
            }
        }
    }

    public void AddDevice(InputDevice device)
    {
        interactorDevices.Add(device);
    }

    public void RemoveDevice(InputDevice device)
    {
        interactorDevices.Remove(device);
    }

    public void ResetDevices()
    {
        interactorDevices = new List<InputDevice>();
    }

    private void OnTriggeredSmoker()
    {
        GetComponent<Smoker>().ReleaseSmoke();
    }
}
