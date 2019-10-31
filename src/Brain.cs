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

    public static BrainNode CreateBrainNode(GameObject node, int ind, Vector3 loc, string inRegion, string inDescription)
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
            BrainNode.CreateBrainNode(brain[i], i, loc, "a" + i, "a" + i);
        }
    }

    private void RenderLine(int i1, int i2, LineRenderer lineRenderer, float cov)
    {
        Transform origin = brain[i1].transform;
        Transform destination = brain[i2].transform;
        lineRenderer.SetPosition(0, origin.position);
        lineRenderer.SetPosition(1, destination.position);
        int covMax = 19126;
        int covMin = -3135;
        double gVal = 255 * System.Math.Floor((cov - covMin) / (covMax - covMin));
        lineRenderer.material.color = new Color32(114, 0, (byte)gVal, 255);
    }

    private void RenderAllLines(float[][] tuples)
    {
        int size = tuples.Length;
        int i1 = 0;
        int i2 = 0;
        float cov = 0;
        LineRenderer[] linesToRender = new LineRenderer[size];
        for (int i = 0; i < size; ++i)
        {
            i1 = (int)tuples[i][0];
            i2 = (int)tuples[i][1];
            cov = tuples[i][2];
            linesToRender[i] = GetComponent<LineRenderer>();
            RenderLine(i1, i2, linesToRender[i], cov);
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
