using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AphaGradient : MonoBehaviour
{
    private TMPro.TextMeshProUGUI text;
    private Color textColor;
    private bool isIncr;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        textColor = text.color;
        isIncr = false;
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = textColor.a;
        if(isIncr)
        {
            text.color = new Color(textColor.r, textColor.g, textColor.b, alpha + 0.05f);
        }
        else
        {
            text.color = new Color(textColor.r, textColor.g, textColor.b, alpha - 0.05f);
        }
        
        if(alpha >= 1.0f || alpha <= 0.0f)
        {
            isIncr = !isIncr;
        }
    }
}
