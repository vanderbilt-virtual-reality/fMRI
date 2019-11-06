using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
    private static int covMax = 19126;
    private static int covMin = 6500;
    public GameObject visualizer;
    private GameObject[] brain = new GameObject[96];
    private GameObject[] chords = new GameObject[42];

    private void Start()
    {
        brain = new GameObject[96];
        float[][] positions = ReadCSVFile();
        for (int i = 0; i < 96; i++)
        {
            Vector3 loc = new Vector3(positions[i][0], positions[i][1], positions[i][2]);
            brain[i] = Instantiate(visualizer, loc, Quaternion.identity);
            BrainNode.CreateBrainNode(brain[i], i, "a" + i, "a" + i);
            if (brain[i].GetComponent<LineRenderer>() == null)
            {
                brain[i].AddComponent<LineRenderer>();
            }
        }
        RenderAllLines(ReadTupleData(0));
        // StartCoroutine(ClearChords());
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
            chords[i].GetComponent<Renderer>().material.color = new Color(1, 0, 1, scale);
        }
    }

    IEnumerator ClearChords()
    {
        yield return new WaitForSeconds(3);
        for (int i = 0; i < 42; i++)
        {
            Destroy(chords[i]);
        }
        chords = new GameObject[42];
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

    float[][] ReadTupleData(int frame)
    {
        StreamReader strReader = new StreamReader("./Assets/src/tuples.csv");
        float[][] adjacencies = new float[42][];
        strReader.ReadLine();
        int start = 42 * frame + 1;
        for (int i = 0; i < 42; i++)
        {
            string dataString = strReader.ReadLine();
            string[] dataValue = dataString.Split(',');
            float[] adjacency = new float[3];
            adjacency[0] = (float)System.Convert.ToDouble(dataValue[0]);
            adjacency[1] = (float)System.Convert.ToDouble(dataValue[1]);
            adjacency[2] = (float)System.Convert.ToDouble(dataValue[2]);
            adjacencies[i] = adjacency;
        }
        return adjacencies;
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
