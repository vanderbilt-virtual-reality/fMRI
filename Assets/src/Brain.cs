using System.Collections;
using System.Diagnostics;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Valve.VR.Extras;
using System.Text.RegularExpressions;

public class Brain : MonoBehaviour
{
    private static int NUM_FRAMES = 1221;
    private int NUM_TUPLES = 50;
    private float[][] adjacencies = new float[61050][];
    private static int covMax = 15000;
    private static int covMin = 5000;
    public GameObject visualizer;
    public GameObject[] brain = new GameObject[96];
    private string[] regions = new string[96];
    private string[] descriptions = new string[96];
    private GameObject[] chords = new GameObject[50];
    public SteamVR_LaserPointer laserPointer;
    public Camera m_Camera;
    public GameObject brainPrefab;


    public Text information;
    
    public void Start()
    {
        brain = new GameObject[96];
        //create button served as textBoard
        Button[] textBoards = new Button[96];
        float[][] positions = ReadCSVFile();
        regions = ReadRegions();
        descriptions = ReadDescription();
        SetFrameData();
        information.text = "Point at any node to learn about that part of the Brain.";

        for (int i = 0; i < 96; i++)
        {
            Vector3 loc = new Vector3(positions[i][0], positions[i][1], positions[i][2]);
            brain[i] = Instantiate(visualizer, loc, Quaternion.identity);
           
            //set name to neural object
            brain[i].name = i+" "+ regions[i];
            //placeholder sphere created
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = loc;
            sphere.transform.localScale = new Vector3(2, 2, 2);
            sphere.transform.parent = brain[i].transform;
            sphere.name = brain[i].name;
           
            if (brain[i].GetComponent<LineRenderer>() == null)
            {
                brain[i].AddComponent<LineRenderer>();
            }
        }

        StartCoroutine(ClearChords());
    }

    public void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClickOn;
    }

    public void PointerClickOn(object sender, PointerEventArgs e)
    {
        for(int i = 0; i<96; i++)
        {
            if(e.target.name == brain[i].name)
            {
              
                brain[i].GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        UnityEngine.Debug.Log("Inside node");
        for (int i = 0; i < 96; i++)
        {
            if (e.target.name == brain[i].name)
            {
                information.text = regions[i] + "\n" + descriptions[i];
                UnityEngine.Debug.Log(information.text);
            }
        }
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        for (int i = 0; i < 96; i++)
        {
            if (e.target.name == brain[i].name)
            {
                information.text = "Point at any node to learn about that part of the Brain.";
            }
        }
    }

    private float getScale(float cov)
    {
        return  (cov - covMin) / (covMax - covMin);
    }

    private void RenderAllLines(float[][] tuples)
    {
        int size = tuples.Length;
        int i1;
        int i2;
        float cov;
        for (int i = 0; i < size; ++i)
        {
            i1 = (int)tuples[i][0];
            i2 = (int)tuples[i][1];
            cov = tuples[i][2];

            Vector3 origin = brain[i1].transform.position;
            Vector3 destination = brain[i2].transform.position;
            Vector3 mid = (origin + destination) * 0.5f;

            chords[i] = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            chords[i].transform.localPosition = mid;
            float scale = getScale(cov);
            chords[i].transform.localScale = new Vector3(scale,
                Vector3.Distance(origin, destination) * 0.5f,
                scale);
            chords[i].transform.up = destination - origin;
            chords[i].GetComponent<Renderer>().material.color = new Color(1, scale, 1, 0);
        }
    }

    IEnumerator ClearChords()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        for (int i = 0; i < NUM_FRAMES; i++)
        {
            RenderAllLines(GetFrameData(i));
            yield return StartCoroutine(clear_helper());
        }

        stopwatch.Stop();

        float eTime = stopwatch.ElapsedMilliseconds;
        UnityEngine.Debug.Log(eTime);

    }

    IEnumerator clear_helper() {
        yield return new WaitForSeconds(0.517f);
        for (int i = 0; i < NUM_TUPLES; i++)
        {
            Destroy(chords[i]);
        }
        chords = new GameObject[NUM_TUPLES];
    }
    //per 100 characters change line

    float[][] ReadCSVFile()
    {
        //read file
        StreamReader strReader = new StreamReader("./Assets/src/coordinate_data.csv");
        float[][] positions = new float[96][];
        for (int i = 0; i < 96; i++)
        {
            string data_String = strReader.ReadLine();
            //store data 
            string[] data_value = data_String.Split(',');
            float[] position = new float[3];
            position[0] = (float)System.Convert.ToDouble(data_value[0]);
            position[2] = (float)System.Convert.ToDouble(data_value[1]);
            position[1] = (float)System.Convert.ToDouble(data_value[2]);
            positions[i] = position;
        }
        return positions;
    }
    string[] ReadRegions()
    {
        string[] regions = new string[96];
        StreamReader strReader = new StreamReader("./Assets/src/location_data.csv");
        for (int i = 0; i < 96; i++)
        {
            string data_String = strReader.ReadLine();
            string[] data_value = data_String.Split(',');
            regions[i] = data_value[0];
            
        }
        return regions;
    }
   
    public static string SpliceText(string str)
    {
        return Regex.Replace(str, "(.{" + 100 + "})", "$1" + System.Environment.NewLine);
    }
  
    //Read Description
    string [] ReadDescription()
    {
        string[] descriptions = new string[96];
        StreamReader strReader = new StreamReader("./Assets/src/brain_description.txt");
        for (int i=0; i<96; i++)
        {
            string data_String = strReader.ReadLine();
            descriptions[i] = System.Convert.ToString(SpliceText(data_String));
            
        }
        return descriptions;
    }
    float[][] GetFrameData(int frameNum)
    {
        float[][] frame = new float[NUM_TUPLES][];
        int start = frameNum * NUM_TUPLES;
        for (int i = 0; i < NUM_TUPLES; i++) {
            frame[i] = new float[3];
            for (int j = 0; j < 3; j++) {
                frame[i][j] = adjacencies[start + i][j];
            }
        }

        return frame;
    }

    void SetFrameData()
    {
        StreamReader strReader = new StreamReader("./Assets/src/tuples.csv");
        //strReader.ReadLine();

        for (int i = 0; i < 61050; i++) {
            string dataString = strReader.ReadLine();
            string[] dataValue = dataString.Split(',');
            float[] adjacency = new float[3];
            adjacency[0] = (float)System.Convert.ToDouble(dataValue[0]);
            adjacency[1] = (float)System.Convert.ToDouble(dataValue[1]);
            adjacency[2] = (float)System.Convert.ToDouble(dataValue[2]);
            adjacencies[i] = adjacency;
        }
    }
}
