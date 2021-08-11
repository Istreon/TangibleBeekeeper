using UnityEngine;

public class AnchorFollowQRCode : MonoBehaviour
{
    #region Serialized fields
    [SerializeField]
    private Transform QRCodeLinked;
    #endregion


    #region Private fields
    private Vector3 lastPosition;
    private Quaternion lastRotation;

    private GameObject parentObject;
    #endregion

    #region MonoBehaviour Callbacks

    // Start is called before the first frame update
    void Start()
    {
        if (QRCodeLinked == null)
        {
            Debug.LogError("QRCode reference missing ", this);
            this.enabled = false;
        }

        Transform parent = gameObject.transform.parent;
        if (parent == null)
        {
            Debug.LogError("This gameobject must have a parent  ", this);
            this.enabled = false;

        }
        else
        {
            parentObject = parent.gameObject;
        }
        lastPosition = this.transform.position;
        lastRotation = this.transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (lastPosition != QRCodeLinked.position || lastRotation != QRCodeLinked.rotation)
        {
            //Update Position
            parentObject.transform.position += QRCodeLinked.position - transform.position;
            lastPosition = QRCodeLinked.position;

            //Update Rotation
            Vector3 rotDiff = Vector3.zero;
            rotDiff = QRCodeLinked.rotation.eulerAngles - transform.rotation.eulerAngles;

            parentObject.transform.Rotate(rotDiff, Space.Self);
            lastRotation = QRCodeLinked.rotation;
        }
    }
    #endregion
}
