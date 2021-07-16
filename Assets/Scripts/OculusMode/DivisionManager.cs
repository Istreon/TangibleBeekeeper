using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class DivisionManager : MonoBehaviour
{
    [Header("Device characteristics")]
    public InputDeviceCharacteristics leftChara;
    public InputDeviceCharacteristics rightChara;
    private InputDevice leftDevice;
    private InputDevice rightDevice;
    private bool lTriggerLastState = false;
    private bool rTriggerLastState = false;

    [Header("Division state panel")]
    public GameObject divisionPanel;
    
    [Header("Frames types")]
    public TMPro.TextMeshProUGUI honey;
    public TMPro.TextMeshProUGUI pollen;
    public TMPro.TextMeshProUGUI opened;
    public TMPro.TextMeshProUGUI closed;
    public TMPro.TextMeshProUGUI queen;
    
    [Header("Global state")]
    public TMPro.TextMeshProUGUI frameCount;
    public TMPro.TextMeshProUGUI divState;
    public Slider divSlider;
    public Image sliderFill;
    public Color normalColor;
    public Color doneColor;
    public Color wrongColor;
    public Color mediumColor;

    [Header("Result state")]
    public GameObject resultPanel;
    public TMPro.TextMeshProUGUI food;
    public TMPro.TextMeshProUGUI brood;
    public TMPro.TextMeshProUGUI finalQueen;
    public TMPro.TextMeshProUGUI blank;
    public TMPro.TextMeshProUGUI empty;
    public TMPro.TextMeshProUGUI order;
    public TMPro.TextMeshProUGUI divisionGrade;
    private float grade;

    public bool isToDivide = true;
    private bool isDivided = false;
    private HiveManager hiveManager;


    // Start is called before the first frame update
    void Start()
    {
        //Setting devices
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(leftChara, devices);
        leftDevice = devices[0];
        devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(rightChara, devices);
        rightDevice = devices[0];
        lTriggerLastState = false;
        rTriggerLastState = false;

        //Setting colors for the slider
        /*normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        doneColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        wrongColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        mediumColor = new Color(1.0f, 0.761f, 0.0f, 1.0f);*/

        divisionPanel.SetActive(true);
        resultPanel.SetActive(false);

        SetHiveEmpty();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDivided)
        {
            if(leftDevice.TryGetFeatureValue(CommonUsages.trigger, out float leftValue) && leftValue > 0.1f)
            {
                lTriggerLastState = true;
            }
            else if(lTriggerLastState)
            {
                lTriggerLastState = false;
                /*if(!isDivided)
                    divisionPanel.SetActive(!divisionPanel.activeInHierarchy);
                else
                {
                }*/
                if(isToDivide)
                {
                    hiveManager.VerifyDivision();
                    SetFinalGrade();
                }
            }

            if(rightDevice.TryGetFeatureValue(CommonUsages.trigger, out float rightValue) && rightValue > 0.1f)
            {
                rTriggerLastState = true;
            }
            else if(rTriggerLastState)
            {
                rTriggerLastState = false;
                /*if(!isDivided)
                    divisionPanel.SetActive(!divisionPanel.activeInHierarchy);
                else
                {
                }*/
                if(isToDivide)
                {
                    hiveManager.VerifyDivision();
                    SetFinalGrade();
                }
            }
        }

        /*if(verifyDivision)
        {
            hiveManager.VerifyDivision();
        }*/
    }

    public void SetHiveManager(HiveManager manager)
    {
        hiveManager = manager;
    }

    public void SetHiveEmpty()
    {
        SetHoney(0);
        SetPollen(0);
        SetOpened(0);
        SetClosed(0);
        SetQueen(false);
        SetDivisionState(0, 0);
    }

    public void SetHoney(int count)
    {
        honey.SetText("Miel : " + count + " cadres");
    }

    public void SetPollen(int count)
    {
        pollen.SetText("Pollen : " + count + " cadres");
    }

    public void SetOpened(int count)
    {
        opened.SetText("Couvain ouvert : " + count + " cadres");
    }

    public void SetClosed(int count)
    {
        closed.SetText("Couvain fermé : " + count + " cadres");
    }

    public void SetQueen(bool hasQueen)
    {
        if(hasQueen)
        {
            queen.SetText("Reine : OUI");
        }
        else
        {
            queen.SetText("Reine : NON");
        }
    }

    public void SetDivisionState(int frames, int blanks)
    {
        int percentage = (int) ((frames / divSlider.maxValue) * 100);
        if(blanks > 0)
            frameCount.SetText("Total : " + (frames+blanks) + " cadres (dont " + blanks + " vides)");
        else
            frameCount.SetText("Total : " + frames + " cadres");
        if(frames < 5)
        {
            divSlider.value = frames;
            divState.SetText(percentage + "% ; division incomplète");
            sliderFill.color = normalColor;
        }
        else if(frames == 5)
        {
            divSlider.value = frames;
            divState.SetText(percentage + "% ; division prête");
            sliderFill.color = doneColor;
            isDivided = true;
        }
        else
        {
            divSlider.value = divSlider.maxValue;
            divState.SetText(percentage + "% ; trop de cadres !");
            sliderFill.color = wrongColor;
        }

    }

    public void SetFramesOrder(string order)
    {
        if(order.Equals("310153") || order.Equals("351013") || order.Equals("31013"))
        {
            this.order.SetText("ORDRE : Cadres bien-ordonnés");
            this.order.color = doneColor;
            grade += 2;
        }
        else
        {
            this.order.SetText("ORDRE : Cadres mal-ordonnés");
            this.order.color = wrongColor;
        }
        Debug.Log("Player's order is: " + order);
    }

    public void SetFoodQuantity(int nbOfFrames)
    {
        if(nbOfFrames == 2)
        {
            food.SetText("QUANTITÉ DE NOURRITURE : " + nbOfFrames + "/2");
            food.color = doneColor;
            grade += 2;
        }
        else
        {
            food.SetText("QUANTITÉ DE NOURRITURE : " + nbOfFrames + "/2");
            food.color = wrongColor;
            if(nbOfFrames == 1 || nbOfFrames > 2)
            {
                grade += 1;
            }
        }
    }
    
    public void SetBroodQuantities(int openedBrood, int closedBrood)
    {
        brood.SetText("COUVAIN OUVERT : " + openedBrood + "/1 ; COUVAIN FERMÉ : " + closedBrood + "/2");
        if(openedBrood == 1 && closedBrood == 2)
        {
            brood.color = doneColor;
            grade += 3;
        }
        else if(openedBrood != 1 && closedBrood != 2)
        {
            brood.color = wrongColor;
        }
        else
        {
            brood.color = mediumColor;
            if(openedBrood == 1)
            {
                grade += 1;
            }
            else if(openedBrood > 1)
            {
                grade += 0.5f;
            }
            if(closedBrood == 2)
            {
                grade += 2;
            }
            else if(closedBrood == 1 || closedBrood > 2)
            {
                grade += 1;
            }
        }
    }

    public void SetQueenPresence(bool queen)
    {
        if(queen)
        {
            finalQueen.SetText("REINE : 1/1");
            finalQueen.color = doneColor;
            grade += 1;
        }
        else
        {
            finalQueen.SetText("REINE : 0/1 (pourrait fonctionner)");
            finalQueen.color = mediumColor;
        }
    }

    public void SetBlankFrames(int nbOfBlanks)
    {
        if(nbOfBlanks == 1)
        {
            blank.SetText("CADRE VIDE : 1/1");
            blank.color = doneColor;
            grade += 1;
        }
        else
        {
            blank.SetText("CADRE VIDE : " + nbOfBlanks + "/1 (1 cadre vide favorise la division)");
            blank.color = mediumColor;
            if(nbOfBlanks > 1)
            {
                grade += 0.5f;
            }
        }
    }

    public void SetEmptySpaces(string hiveOccupation)
    {
        if(hiveOccupation.Contains("101") | hiveOccupation.Contains("1001")| hiveOccupation.Contains("10001")| hiveOccupation.Contains("100001")| hiveOccupation.Contains("1000001"))
        {
            empty.SetText("ESPACE ENTRE LES CADRES : Cadres pas assez proches");
            empty.color = wrongColor;
        }
        else
        {
            empty.SetText("ESPACE ENTRE LES CADRES : 0");
            empty.color = doneColor;
            grade += 1;
        }
    }

    public void SetFinalGrade()
    {
        string finalGrade = "TOTAL : " + grade + "/10";
        if(grade == 10)
        {
            divisionGrade.SetText(finalGrade + " TRÈS BIEN");
            divisionGrade.color = doneColor;
        }
        else if(grade >= 7 & grade <= 9.5f)
        {
            divisionGrade.SetText(finalGrade + " BIEN");
            divisionGrade.color = doneColor;
        }
        else if(grade >= 4 & grade < 7)
        {
            divisionGrade.SetText(finalGrade + " INSUFFISANT");
            divisionGrade.color = mediumColor;
        }
        else
        {
            divisionGrade.SetText(finalGrade + " MAUVAIS");
            divisionGrade.color = wrongColor;
        }

        divisionPanel.SetActive(false);
        resultPanel.SetActive(true);
        isToDivide = false;
    }
}
