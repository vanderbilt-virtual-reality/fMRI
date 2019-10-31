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
    public GameObject visualizer;
    private GameObject[] brain;

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
        float[][] adjacencies = ReadTupleData(0);
        RenderAllLines(adjacencies);
    }

    private void RenderLine(int i1, int i2, LineRenderer lineRenderer)
    {
        Transform origin = brain[i1].transform;
        Transform destination = brain[i2].transform;
        lineRenderer.SetPosition(0, origin.position);
        lineRenderer.SetPosition(1, destination.position);
    }

    private void RenderAllLines(float[][] tuples)
    {
        ResetLines();
        int size = tuples.Length;
        int i1;
        int i2;
        float cov;
        // LineRenderer[] linesToRender = new LineRenderer[size];
        for (int i = 0; i < size; ++i)
        {
            i1 = (int)tuples[i][0];
            i2 = (int)tuples[i][1];
            cov = tuples[i][2];
            LineRenderer curLR = brain[i1].GetComponent<LineRenderer>();
            int covMax = 19126;
            int covMin = -3135;
            float scale = (cov - covMin) / (covMax - covMin);
            Debug.Log("Scale: " + scale + "| Cov: " + cov);
            curLR.positionCount = 2;
            curLR.material.color = new Color(1, 1, scale, 0);
            RenderLine(i1, i2, curLR);
        }
    }

    private void ResetLines()
    {
        for (int i = 0; i < 96; i++)
        {
            brain[i].GetComponent<LineRenderer>().positionCount = 0;
        }
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
