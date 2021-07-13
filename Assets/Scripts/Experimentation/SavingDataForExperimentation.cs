using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using System.IO;
using System.Text;

public class SavingDataForExperimentation : MonoBehaviour
{

    [SerializeField]
    InputActionAsset control;
    
    InputAction saveDataVirtual;

    InputAction saveDataWithoutWeight;
    InputAction saveDataWeight;


    FramePositioningDetection[] frames;
    // Start is called before the first frame update
    void Start()
    {
        frames = FindObjectsOfType<FramePositioningDetection>();


        var keyboardActionMap = control.FindActionMap("KeyboardMap");
        saveDataVirtual = keyboardActionMap.FindAction("SaveDataVirtual");
        saveDataVirtual.performed += OnActivationVirtual;
        saveDataVirtual.Enable();

        saveDataWithoutWeight = keyboardActionMap.FindAction("SaveDataWithoutWeight");
        saveDataWithoutWeight.performed += OnActivationWithoutWeight;
        saveDataWithoutWeight.Enable();

        saveDataWeight = keyboardActionMap.FindAction("SaveDataWeight");
        saveDataWeight.performed += OnActivationWeight;
        saveDataWeight.Enable();
    }

    void OnActivationVirtual(InputAction.CallbackContext context)
    {
        SaveData("Virtuelle");
    }

    void OnActivationWithoutWeight(InputAction.CallbackContext context)
    {
        SaveData("SansPoids");
    }

    void OnActivationWeight(InputAction.CallbackContext context)
    {
        SaveData("Poids");
    }

    private void SaveData(string type)
    {

        string resDist = "";
        string resSD = "";
        foreach (FramePositioningDetection f in frames)
        {
            //Check if the frame is inside of the hive
            bool inHive = f.isInTheHive();
            
            if (inHive)  //If the frame is in the hive, save it position and it standard deviation
            {
                resDist += f.getActualDist().ToString()+";";
                resSD += f.getActualStandardDeviation().ToString() + ";";


                string temp="Data : {"+f.getActualDist().ToString();
                temp+=";"+f.getActualStandardDeviation().ToString()+"}";
                Debug.Log(temp);
            }
        }
        resDist += "\n";
        resSD += "\n";

        string fileName = "logExperimentation/distance" + type + ".csv";
        appendToAFile(fileName, resDist);

        fileName = "logExperimentation/standardDeviation" + type + ".csv";
        appendToAFile(fileName, resSD);
    }


    private void appendToAFile(string fileName, string text)
    {
        TextWriter writer;
        writer = File.AppendText(fileName);
        writer.Write(text);
        writer.Close();
    }

}
