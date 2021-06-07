using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMenu : MonoBehaviour
{
    public Ring ring;
    public RingCakePiece ringCakePiecePrefab;
    public float gapWithDegree = 1.0f;
    public Action<string> callback;
    protected RingCakePiece[] pieces;
    protected RingMenu parent;
    [HideInInspector]
    public string path;
    private Vector3 joystickPosition = Vector3.zero;

    [HideInInspector]
    public bool wasClicked = false;
    [HideInInspector]
    public int activeElement = -1;

    // Start is called before the first frame update
    void Start()
    {
        float stepLength = 360.0f/ring.ringElements.Length;
        float iconDist = Vector3.Distance(ringCakePiecePrefab.icon.transform.position, ringCakePiecePrefab.cakePiece.transform.position);

        //Position it
        pieces = new RingCakePiece[ring.ringElements.Length];
        for(int i = 0; i < ring.ringElements.Length; i++)
        {
            pieces[i] = Instantiate(ringCakePiecePrefab, transform);
            //Set root element
            pieces[i].transform.localPosition = Vector3.zero;
            pieces[i].transform.localRotation = Quaternion.identity;

            //Set cake piece
            pieces[i].cakePiece.fillAmount = 1.0f / ring.ringElements.Length - gapWithDegree / 360.0f;
            pieces[i].cakePiece.transform.localPosition = new Vector3(0, 0, -0.1f);
            pieces[i].cakePiece.transform.localRotation = Quaternion.Euler(0, 0, -stepLength/2.0f + gapWithDegree/2.0f + i*stepLength);
            pieces[i].cakePiece.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);

            //Set icon
            //Vector3 iconPos = pieces[i].icon.transform.localPosition;
            pieces[i].icon.transform.localPosition = pieces[i].cakePiece.transform.localPosition + Quaternion.AngleAxis(i * stepLength, Vector3.forward) * Vector3.up * iconDist;
            //pieces[i].icon.transform.localPosition = new Vector3(iconPos.x, iconPos.y, iconPos.z + 10);
            pieces[i].icon.sprite = ring.ringElements[i].elementIcon;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*float stepLength = 360.0f / ring.ringElements.Length;
        float joystickAngle = NormalizeAngle(Vector3.SignedAngle(Vector3.up, joystickPosition - transform.position, Vector3.forward) + stepLength / 2.0f);
        activeElement = (int)(joystickAngle / stepLength);*/
        for (int i = 0; i < ring.ringElements.Length; i++)
        {
            if(i == activeElement)
                pieces[i].cakePiece.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            else
                pieces[i].cakePiece.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
        }

        /*if(wasClicked)
        {
            callback?.Invoke(ring.ringElements[activeElement].name);
        }*/
    }

    private float NormalizeAngle(float a)
    {
        return (a + 360.0f) % 360.0f;
    }

    public void SetJoystickPosition(Vector2 newPos)
    {
        joystickPosition = new Vector3(newPos.x, newPos.y, 0.0f);
    }
}
