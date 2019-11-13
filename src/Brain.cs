using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;



public class BrainNode : MonoBehaviour
{
    public int index;
    public float bold;
    public string desc;
    public string region;

    //public BrainNode(int inIndex, Vector3 loc, string inRegion, string inDescription)
    //{
    //    node = Instantiate(node, loc, Quaternion.identity);
    //    index = inIndex;
    //    node.name = "RollerBall" + index;
    //    region = inRegion;
    //    desc = inDescription;
    //}

    public static BrainNode CreateBrainNode(GameObject node, int ind, string inRegion, string inDescription)
    {
        BrainNode myBrainNode = node.AddComponent<BrainNode>();
        myBrainNode.index = ind;
        myBrainNode.region = inRegion;
        myBrainNode.desc = inDescription;
        node.name = inRegion;
        return myBrainNode;
    }

    public void updateIntensity(float newBold)
    {
        bold = newBold;
    }

    public string getRegion()
    {
        return region;
    }

    public string getDescription()
    {
        return desc;
    }

    public float getBold()
    {
        return bold;
    }

    public float[] getLoc()
    {
        // Find a way to return location, add it to PIV if you want see what's up
        return null;
    }

}

public class Brain : MonoBehaviour
{
    private static int NUM_FRAMES = 1221;
    private int NUM_TUPLES = 50;
    private float[][] adjacencies = new float[51282][];
    private static int covMax = 19126;
    private static int covMin = 6500;
    public GameObject visualizer; // empty object 
    public GameObject[] brain = new GameObject[96];
    public GameObject[] chords = new GameObject[50];

    //public GameObject[] child = new GameObject[96];

    private void Start()
    {
        brain = new GameObject[96];
        float[][] positions = ReadCSVFile();
        string[] regions = ReadRegions();
        SetFrameData();

        for (int i = 0; i < 96; i++)
        {
            Vector3 loc = new Vector3(positions[i][0], positions[i][1], positions[i][2]);
            brain[i] = Instantiate(visualizer, loc, Quaternion.identity);
            //placeholder sphere object
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = loc;
            //make sphere child component of brain node, with no particular use by now...
            //comment it bacuase rollerball prefab moves ...? comment it out if rollerball works fine
            //sphere.transform.parent = brain[i].transform; 

            BrainNode.CreateBrainNode(brain[i], i, "a" + i, "a" + i);
            if (brain[i].GetComponent<LineRenderer>() == null)
            {
                brain[i].AddComponent<LineRenderer>();

            }
            //assign text
            MeshFilter meshFilter = brain[i].GetComponent<MeshFilter>();
            DestroyImmediate(meshFilter);
            brain[i].AddComponent<TextMesh>();
            brain[i].GetComponent<TextMesh>().text = regions[i];
            //make text invisible for now; text will be visible in laser script
            //brain[i].GetComponent<MeshRenderer>().enabled = false;
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
            chords[i].GetComponent<Renderer>().material.color = new Color(1, 0, 1, scale);
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

        for (int i = 0; i < NUM_FRAMES; i++)
        {
            RenderAllLines(GetFrameData(i));
            //Debug.Log(i);
            yield return StartCoroutine(clear_helper());
            yield return new WaitForSeconds(1.0f);
        }

    }

    IEnumerator clear_helper()
    {
        yield return new WaitForSeconds(5.0f);
        for (int i = 0; i < NUM_TUPLES; i++)
        {
            Destroy(chords[i]);
        }
        chords = new GameObject[NUM_TUPLES];
    }

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
            Debug.Log(regions[i]);
        }
        return regions;
    }

    float[][] GetFrameData(int frameNum)
    {
        float[][] frame = new float[NUM_TUPLES][];
        int start = frameNum * NUM_TUPLES;
        for (int i = 0; i < NUM_TUPLES; i++)
        {
            frame[i] = new float[3];
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

        for (int i = 0; i < 51282; i++)
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
