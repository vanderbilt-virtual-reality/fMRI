using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Application
{
    public class BrainNode
    {
        public GameObject node;
        public int index;
        public float bold;

        public BrainNode(int index, float[] loc)
        {
            node = Instantiate(node, new Vector3(loc[0], loc[1], loc[2]);
            node.name = "RollerBall";
        }

        public updateIntensity()
        {

        }



    }
}
