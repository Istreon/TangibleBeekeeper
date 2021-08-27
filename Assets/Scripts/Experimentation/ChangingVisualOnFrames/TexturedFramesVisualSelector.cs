using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexturedFramesVisualSelector : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    private Transform face1;
    [SerializeField]
    private Transform face2;

    #endregion


    #region Private fields
    private GameObject visualFace1;
    private GameObject visualFace2;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        if(face1==null || face2==null)
        {
            Debug.LogError("Missing Parameter value, face1 or face2 is null", this);
        }
    }

    public void ChangeVisual(GameObject visual1, GameObject visual2)
    {
        //Remove old visuals
        if (visualFace1 != null) Destroy(visualFace1);
        if (visualFace2 != null) Destroy(visualFace2);

        //Instanciate new visuals
        visualFace1 = Instantiate(visual1);
        visualFace2 = Instantiate(visual2);

        //Giving visual the right position by changing their parents and reseting their position
        visualFace1.transform.parent = face1;
        visualFace2.transform.parent = face2;

        visualFace1.transform.localPosition = Vector3.zero;
        visualFace1.transform.localRotation = Quaternion.identity;
        visualFace2.transform.localPosition = Vector3.zero;
        visualFace2.transform.localRotation = Quaternion.identity;
        visualFace1.transform.localScale = Vector3.one;
        visualFace2.transform.localScale = Vector3.one;
    }
}
