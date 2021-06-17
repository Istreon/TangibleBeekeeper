using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.XR;

public class BeesData : MonoBehaviour
{
    //Instances for manipulating the graph
    public InputDeviceCharacteristics characteristics;
    private InputDevice device;
    public Slider timeSlider;
    public TMPro.TextMeshProUGUI instant;

    public TMPro.TextMeshProUGUI nourrices;
    public TMPro.TextMeshProUGUI butineuses;
    public List<Sprite> images;
    public Image graphImage;
    

    //Lists to store data and instantiate the graph
    public List<List<Bee>> beeData = new List<List<Bee>>();
    public List<List<Bee>> deadBees = new List<List<Bee>>();
    public List<List<Bee>> bornBees = new List<List<Bee>>();
    [HideInInspector]
    public List<Vector3> beeMax = new List<Vector3>();
    public List<Bee> bees = new List<Bee>();
    public List<int[]> beePopulation = new List<int[]>();
    private List<string> otherTasks = new List<string>() {"EggTask", "LarvaTask", "NympheaTask"};
    private List<int> timeScale = new List<int>();

    //Time parameters
    [HideInInspector]
    public int currentTurn = 0;
    [HideInInspector]
    public int turnIndex = 0;
    [HideInInspector]
    public bool forward = true;
    [HideInInspector]
    public int nbOfTurns = 0;
    private bool joyActivated = false;
    private float joyTime = 0.0f;
    private float activationTime = 0.0f;
    private float joyValue = 0.0f;

    //Lists to retrieve the data from .csv
    private List<int> turns = new List<int>();
    private List<int> ids = new List<int>();
    private List<string> tasks = new List<string>();
    private List<float> hjs = new List<float>();
    private List<float> eos = new List<float>();
    private List<float> realAges = new List<float>();
    private List<float> exchanges = new List<float>();
    private int scenarioIndex = 0;
    private string fileName = "logsScenario0";

    //Instance to link & update the data to the graph
    public ContactGrapherRetriever grapherRetriever;

    private void Start()
    {
        InitializeGraph();

        //Initialization of the input devices for interaction
        GetDevice();

        //timeSlider.onValueChanged.AddListener(delegate {UpdateGrapherRetriever(timeSlider.value);});
        //timeSlider.onValueChanged.AddListener(delegate {CheckValueChanged(timeSlider.value);});
        //CheckValueChanged(timeSlider.value);

        //CheckBeeLists();
    }

    public void InitializeGraph()
    {
        beeData = new List<List<Bee>>();
        beeMax = new List<Vector3>();
        bees = new List<Bee>();
        beePopulation = new List<int[]>();
        deadBees = new List<List<Bee>>();
        currentTurn = 0;
        turnIndex = 0;
        nbOfTurns = 0;
        turns = new List<int>();
        ids = new List<int>();
        tasks = new List<string>();
        hjs = new List<float>();
        eos = new List<float>();
        realAges = new List<float>();
        exchanges = new List<float>();
        
        RetrieveData(fileName);
        
        //Debug.Log("Total of turns: " + turns.Count);

        //Data storage in the different lists
        currentTurn = turns[0];
        timeScale.Add(currentTurn);
        for( int i = 0; i < turns.Count; i++)
        {            
            if(turns[i] == currentTurn)
            {
                if(!otherTasks.Contains(tasks[i]))
                {
                    bees.Add(new Bee(ids[i], tasks[i], hjs[i], realAges[i], exchanges[i]));
                }
            }
            else
            {
                currentTurn = turns[i];
                beeMax.Add(GetMax(bees));
                beePopulation.Add(GetPopulation(bees));
                beeData.Add(bees);
                bees = new List<Bee>();
                timeScale.Add(currentTurn);
            }
            
        }
        GetDeadBees();
        GetNewBornBees();
        bees = beeData[0];
        bees[0].ToString();
        nbOfTurns = beeData.Count;
        //Debug.Log("There are " + bees.Count + " bees for " + nbOfTurns + " turns stored in a " + beeData.Count + " long list.");
        //Debug.Log("First turn: " + beeData[0].Count + " bees. Second turn: " + beeData[1].Count + " bees. Last turn: " + beeData[beeData.Count - 1].Count + " bees.");


        InitializeSlider(timeSlider);
        
    }

    private void Update()
    {
        
        CheckJoyActivated();
        
        if(joyActivated)
        {
            //Debug.Log("Entered if(joyActivated) condition");
            float deltaT = Time.realtimeSinceStartup - activationTime;
            if( deltaT <= 3.0f && Time.realtimeSinceStartup - joyTime >= 0.67f)
            {
                //Debug.Log("Time update at slow pace");
                UpdateTime(joyValue);
            }
            if(deltaT > 3.0f && deltaT <= 10.0f && Time.realtimeSinceStartup - joyTime >= 0.33f)
            {
                //Debug.Log("Time update at medium pace");
                UpdateTime(joyValue);
            }
            if(deltaT > 10.0f)
            {
                //Debug.Log("Time update at fast pace");
                UpdateTime(joyValue);
            }
        }
            
            
        timeSlider.value = turnIndex;
        grapherRetriever.maxValues = beeMax[turnIndex];
        instant.text = "T0 + " + GetTurnDay(timeScale[turnIndex]);
        nourrices.text = "Nourrices : " + beePopulation[turnIndex][0];
        butineuses.text = "Butineuses : " + beePopulation[turnIndex][1]; 
    }


    public void RetrieveData(string fileName)
    {
        TextAsset f = (TextAsset)Resources.Load("custom_dir/" + fileName);
        string fileText = System.Text.Encoding.UTF8.GetString(f.bytes);
        string[] lines = fileText.Split('\n');
        Debug.Log(lines[0]);
        Debug.Log(lines[1]);
        string[] lineOne = lines[1].Split(',');
        Debug.Log(lineOne[0] + lineOne[0].GetType());
        for(int i = 1; i< lines.Length; i++)
        {
            string l = (string) lines[i];
            //Debug.Log(l);
            string[] columns = l.Split(',');
            
            turns.Add(int.Parse((string)columns[0]));
            ids.Add(int.Parse(columns[1]));
            tasks.Add(columns[2]);
            hjs.Add(float.Parse(columns[3].Replace('.',',')));
            eos.Add(float.Parse(columns[4].Replace('.',',')));
            realAges.Add(float.Parse(columns[5].Replace('.',',')));
            exchanges.Add(float.Parse(columns[6].Replace('.',',')));
        }
    }

    public Vector3 GetMax(List<Bee> beeList)
    {
        Vector3 max = Vector3.zero;
        foreach (Bee b in beeList)
        {
            if(b.realAge > max.x && b.physioAge > max.y && b.exchange > max.z)
            {
                max = new Vector3(b.realAge, b.physioAge, b.exchange);
            }
        }
        return max;
    }

    public int[] GetPopulation(List<Bee> beeList)
    {
        int[] pop = new int[2];
        int nour = 0;
        int but = 0;
        foreach (Bee b in beeList)
        {
            if(b.physioAge < 0.5f)
                nour += 1;
            else
                but +=1;            
        }
        pop[0] = nour;
        pop[1] = but;
        return pop;
    }

    public int GetTurnDay(int turn)
    {
        int day = turn / 86400;
        return day;
    }

    public void GetDeadBees()
    {
        deadBees.Add(new List<Bee>());
        for(int i = 1; i < beeData.Count; i ++)
        {
            List<Bee> newDeads = new List<Bee>();
            foreach (Bee oldB in beeData[i-1])
            {
                if(!ContainsBee(beeData[i], oldB))
                {
                    newDeads.Add(oldB);
                }
            }
            deadBees.Add(newDeads);
            //Debug.Log("Turn No. " + i + " with " + deadBees[i].Count + " dead bees");
        }          
    }

    public void GetNewBornBees()
    {
        for(int i = 0; i < beeData.Count - 1; i++)
        {
            List<Bee> newBorns = new List<Bee>();
            foreach(Bee newB in beeData[i+1])
            {
                if(!ContainsBee(beeData[i], newB))
                {
                    newBorns.Add(newB);
                }
            }
            bornBees.Add(newBorns);
            //Debug.Log("Turn No. " + i + " with " + bornBees[i].Count + " new bees");
        }
    }

    public bool ContainsBee(List<Bee> beeList, Bee b)
    {
        foreach (Bee bee in beeList)
        {
            if(b.id == bee.id)
            {
                return true;
            }
        }
        return false;
    }

    public void InitializeSlider(Slider slider)
    {
        slider.minValue = 0;
        slider.maxValue = nbOfTurns-1;
        slider.wholeNumbers = true;
        slider.value = 0;
    }
    public void UpdateTime(float joystickX)
    {
        //Debug.Log("Entered UpdateTime()");
        if(joystickX > 0.2f && timeSlider.value < timeSlider.maxValue)
        {
            //Debug.Log("Updated forward");
            turnIndex += 1;
            joyTime = Time.realtimeSinceStartup;
            forward = true;
        }
        else if(joystickX < -0.2f && timeSlider.value > 0.0f)
        {
            //Debug.Log("Updated backward");
            turnIndex -= 1;
            joyTime = Time.realtimeSinceStartup;
            forward = false;
        }
    }

    public void CheckJoyActivated()
    {
        //Debug.Log("CheckJoyActivated()");
        if(device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 lJoyValue))
        {
            joyValue = lJoyValue.x;
            if(!joyActivated && Math.Abs(joyValue) > 0.1f)
            {
                //Debug.Log("Joystick is being activated");
                joyActivated = true;
                activationTime = Time.realtimeSinceStartup;
                joyTime = Time.realtimeSinceStartup;
                return;
                
            }
            else if(joyActivated && Math.Abs(joyValue) < 0.1f)
            {
                //Debug.Log("Joystick is being deactivated");
                joyActivated = false;
                return;
            }
            /*if(lJoyValue.x > 0.2f && timeSlider.value < timeSlider.maxValue)
            {
                turnIndex += 1;
                forward = true;
            }
            else if(lJoyValue.x < -0.2f && timeSlider.value > 0.0f)
            {
                turnIndex -= 1;
                forward = false;
            }*/
        }
    }

    public void GetDevice()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(characteristics, devices);
        if(devices.Count > 0)
        {
            device = devices[0];
        }
    }

    public void NextScenario()
    {
        scenarioIndex = (scenarioIndex + 1) % 2;
        fileName = "logsScenario" + scenarioIndex;
        graphImage.sprite = images[scenarioIndex];
        graphImage.preserveAspect = true;
        grapherRetriever.clearGraph();
        InitializeGraph();
    }


    public void CheckValueChanged(float value)
    {
        Debug.Log("Update slider to value " + value);
    }

    public void CheckBeeLists()
    {
        for(int i = 0; i < beeData.Count; i++)
        {
            if (!beeData[i].Equals(beeData[0]))
            {
                Debug.Log("The bees lists are all the same: " + false);
                return;
            }
        }
        Debug.Log("The bees lists are all the same: " + true);
    }
}

public class Bee
{
    public float realAge; /*** Typically between 0 and 2 000 000 (can go beyond) ***/
    public float physioAge; /*** [ 0 ; 1 ] ***/
    public float exchange; /*** [ 0 ; infinity [ ***/

    public string task;

    public int id;

    public Bee(int id, string task, float physioAge, float realAge, float exchange)
    {
        this.id = id;

        this.task = task;

        this.realAge = realAge;
        this.physioAge = physioAge;
        this.exchange = exchange;        
    }
    public void UpdateRealAge(float newAge)
    {
        realAge = newAge;
    }

    public void UpdatePhysioAge(float newAge)
    {
        physioAge = newAge;
    }

    public void UpdateExchange(float newExchange)
    {
        exchange = newExchange;
    }

    public void ToString()
    {
        Debug.Log(id + ", " + task + ", " + physioAge + ", " + realAge + ", " + exchange);
    }
}
