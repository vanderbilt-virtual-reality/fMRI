using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Instantiate : MonoBehaviour
{
    public GameObject myPrefab;
    // Start is called before the first frame update
    void Start()
    {
        float[][] positions = ReadCSVFile();
        // Instantiate at position (0, 0, 0) and zero rotation.
        for (int i=0; i<96; i++)
        {
            myPrefab = Instantiate(myPrefab, new Vector3((positions[i][0]), positions[i][1], positions[i][2]), Quaternion.identity);
            myPrefab.name = "RollerBall";
        }
        //myPrefab = Instantiate(myPrefab, new Vector3(0,0,0), Quaternion.identity);
    }

    float[][] ReadCSVFile()
    {
        //read file
        StreamReader strReader = new StreamReader("./coordinate_data.csv");
        float[][] positions = new float[96][];
        for(int i=0; i<96;i++){
            string data_String = strReader.ReadLine();
            //store data 
            string[] data_value = data_String.Split(',');
            float[] position = new float[3];
            position[0] = (float)System.Convert.ToDouble(data_value[0]);
            position[1] = (float)System.Convert.ToDouble(data_value[1]);
            position[2] = (float)System.Convert.ToDouble(data_value[2]);
            positions[i]=position;
        }
        return positions;
    }
     //Update is called once per frame
    void Update()
    {
        
    }
}
