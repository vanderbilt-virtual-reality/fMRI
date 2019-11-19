using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
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
    private GameObject[] chords = new GameObject[50];
    //private GameObject[] chords2 = new GameObject[50];
    //private bool flag = true;
    public SteamVR_LaserPointer laserPointer;
    public Camera m_Camera;

    public void Start()
    {
        brain = new GameObject[96];
        //create button served as textBoard
        Button[] textBoards = new Button[96];
        float[][] positions = ReadCSVFile();
        string[] regions = ReadRegions();
        string[] descriptions = ReadDescription();
        SetFrameData();

        for (int i = 0; i < 96; i++)
        {
            Vector3 loc = new Vector3(positions[i][0], positions[i][1], positions[i][2]);
            brain[i] = Instantiate(visualizer, loc, Quaternion.identity);

            //set name to neural object
            brain[i].name = regions[i];
            //placeholder sphere created
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = loc;
            brain[i].transform.parent = sphere.transform;
            sphere.name = brain[i].name;

            if (brain[i].GetComponent<LineRenderer>() == null)
            {
                brain[i].AddComponent<LineRenderer>();
            }
            //assign text
            MeshFilter meshFilter = brain[i].GetComponent<MeshFilter>();
            DestroyImmediate(meshFilter);
            brain[i].AddComponent<TextMesh>();
            brain[i].GetComponent<TextMesh>().text = regions[i];
            string appending = "\n" + descriptions[i];
            // add values on position

            brain[i].GetComponent<TextMesh>().text += appending;
            //making it face to the camera, rotate the text
            //brain[i].GetComponent<TextMesh>().transform.localEulerAngles += new Vector3(0, 0, 0);
            //brain[i].GetComponent<TextMesh>().transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            //m_Camera.transform.rotation * Vector3.up+new Vector3(0,75,15));
            //make text invisible for now; text will be visible in laser script
            brain[i].GetComponent<MeshRenderer>().enabled = false;
        }

        /**for (int i = 0; i < NUM_FRAMES; ++i)
        {
            //RenderAllLines(GetFrameData(i));
            //StartCoroutine(ClearChords());
        }**/

        //RenderAllLines(GetFrameData(0));
        StartCoroutine(ClearChords());
        //RenderAllLines(GetFrameData(20));
        //StartCoroutine(ClearChords());
    }

    public void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClickOn;
    }

    public void PointerClickOn(object sender, PointerEventArgs e)
    {
        for (int i = 0; i < 96; i++)
        {
            if (e.target.name == brain[i].name)
            {
                //brain[i].SetActive(true);
                brain[i].GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        for (int i = 0; i < 96; i++)
        {
            if (e.target.name == brain[i].name)
            {
                //brain[i].SetActive(true);
                brain[i].GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        for (int i = 0; i < 96; i++)
        {
            if (e.target.name == brain[i].name)
            {
                //brain[i].SetActive(false);
                brain[i].GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    private float getScale(float cov)
    {
        return (cov - covMin) / (covMax - covMin);
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
        //yield return new WaitForSeconds(5);

        //for (int i = 0; i < NUM_TUPLES; i++)
        //{
        //  Destroy(chords[i]);
        //}
        //chords = new GameObject[NUM_TUPLES];
        //RenderAllLines(GetFrameData(20));

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        for (int i = 0; i < NUM_FRAMES; i++)
        {
            RenderAllLines(GetFrameData(i));
            yield return StartCoroutine(clear_helper());
            //yield return new WaitForSeconds(1.0f);
        }

        stopwatch.Stop();

        float eTime = stopwatch.ElapsedMilliseconds;


    }

    IEnumerator clear_helper()
    {
        //yield return new WaitForSeconds(0.389f);
        yield return new WaitForSeconds(0.1f);
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
            position[1] = (float)System.Convert.ToDouble(data_value[1]);
            position[2] = (float)System.Convert.ToDouble(data_value[2]);
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
    string[] ReadDescription()
    {
        string[] descriptions = new string[96];
        StreamReader strReader = new StreamReader("./Assets/src/brain_description.txt");
        for (int i = 0; i < 96; i++)
        {
            string data_String = strReader.ReadLine();
            descriptions[i] = System.Convert.ToString(SpliceText(data_String));
            UnityEngine.Debug.Log(descriptions[i]);
        }
        return descriptions;
    }
    float[][] GetFrameData(int frameNum)
    {
        float[][] frame = new float[NUM_TUPLES][];
        int start = frameNum * NUM_TUPLES;
        for (int i = 0; i < NUM_TUPLES; i++)
        {
            frame[i] = new float[3];
            //UnityEngine.Debug.Log(start + i);
            for (int j = 0; j < 3; j++)
            {
                frame[i][j] = adjacencies[start + i][j];
                //Debug.Log(start + i);
                //Debug.Log(adjacencies[start + i][j]);
            }
        }

        return frame;
    }

    void SetFrameData()
    {
        StreamReader strReader = new StreamReader("./Assets/src/tuples.csv");
        //strReader.ReadLine();

        for (int i = 0; i < 61050; i++)
        {
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

//public class Instantiate : MonoBehaviour
//{
//    public GameObject myPrefab;
//    // Start is called before the first frame update
//    void Start()
//    {
//        float[][] positions = ReadCSVFile();
//        // Instantiate at position (0, 0, 0) and zero rotation.
//        for (int i=0; i<96; i++)
//        {
//            myPrefab = Instantiate(myPrefab, new Vector3((positions[i][0]), positions[i][1], positions[i][2]), Quaternion.identity);
//            myPrefab.name = "RollerBall";
//        }
//        //myPrefab = Instantiate(myPrefab, new Vector3(0,0,0), Quaternion.identity);
//    }

//    float[][] ReadCSVFile()
//    {
//        //read file
//        StreamReader strReader = new StreamReader("./Assets/src/coordinate_data.csv");
//        float[][] positions = new float[96][];
//        for(int i=0; i<96;i++){
//            string data_String = strReader.ReadLine();
//            //store data 
//            string[] data_value = data_String.Split(',');
//            float[] position = new float[3];
//            position[0] = (float)System.Convert.ToDouble(data_value[0]);
//            position[1] = (float)System.Convert.ToDouble(data_value[1]);
//            position[2] = (float)System.Convert.ToDouble(data_value[2]);
//            positions[i]=position;
//        }
//        return positions;
//    }
//     //Update is called once per frame
//    void Update()
//    {

//    }
//}
