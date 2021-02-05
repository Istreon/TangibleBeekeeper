using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableClippingOnMaterial : MonoBehaviour
{
    [Header("Materials for which it is possible to activate clipping or not")]
    [SerializeField]
    private Material[] materials;   //contains the materials that can undergo the clipping plan (defined in editor)

    private List<Material> matList=new List<Material>(); //contains the materials that can undergo the clipping plan (defined during game)

    [Header("Material which indicates by its color if clipping is active or not")]
    [SerializeField]
    private Material statusIndicatorMaterial; //Contains material which indicates by its color if clipping is active or not

    [Header("Wireframe material activated when clipping is active")]
    [SerializeField]
    private Material wireframeMat = null;  //Contains wireframe material to activate while clipping is enable to avoid injuries during vr manipulation with real hive

    private bool clippingActive = false;
    // Start is called before the first frame update
    void Start()
    {

        foreach (Material m in materials)
        {
            m.SetFloat("_ActiveClipping", clippingActive? 1f : 0f);
        }
        foreach (Material m in matList)
        {
            m.SetFloat("_ActiveClipping", clippingActive ? 1f : 0f);
        }
        if(wireframeMat!=null) wireframeMat.SetFloat("_ActiveWireframe", clippingActive ? 1f : 0f);
        statusIndicatorMaterial.SetFloat("_Active", clippingActive ? 1f : 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("InteractiveWithSwitchButton"))
        {
            clippingActive = !clippingActive;

            foreach (Material m in materials)
            {
                m.SetFloat("_ActiveClipping", clippingActive ? 1f : 0f);
            }
            foreach (Material m in matList)
            {
                m.SetFloat("_ActiveClipping", clippingActive ? 1f : 0f);
            }
            if (wireframeMat != null) wireframeMat.SetFloat("_ActiveWireframe", clippingActive ? 1f : 0f);
            statusIndicatorMaterial.SetFloat("_Active", clippingActive ? 1f : 0f);
        }
    }

    public void addMaterial(Material m) {

        m.SetFloat("_ActiveClipping", clippingActive ? 1f : 0f);
        matList.Add(m);
    }
}
