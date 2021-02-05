using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClippingPlane : MonoBehaviour
{

    [Header("Materials that can undergo the clipping plan")]
    public Material[] materials; //contains the materials that can undergo the clipping plan (defined in editor)
    private List<Material> matList=new List<Material>(); //contains the materials that can undergo the clipping plan (defined during game)

    // Update is called once per frame
    void Update()
    {
        foreach (Material m in materials)
        {
            m.SetVector("_PlanePosition", transform.position);
            m.SetVector("_PlaneNormal", transform.forward);
        }
        foreach (Material m in matList)
        {
            m.SetVector("_PlanePosition", transform.position);
            m.SetVector("_PlaneNormal", transform.forward);
        }

    }

    public void addMaterial(Material m) {

        matList.Add(m);
    }
}
