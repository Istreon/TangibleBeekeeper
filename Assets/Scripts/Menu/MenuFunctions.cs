using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFunctions : MonoBehaviour
{
    [Header("The beekeper visor who need to be disable/enable")]
    [SerializeField]
    private GameObject visor;  //Peut être le transformer en tableau
    public void setVisor()
    {
        if(visor!=null)
        {
            visor.SetActive(!visor.activeSelf);
        }
    }
}
