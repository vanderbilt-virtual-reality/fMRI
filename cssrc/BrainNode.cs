using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Application
{
    public class BrainNode
    {
        private GameObject node;
        private int index;
        private float bold;
		private string desc;
		private string region;

        public BrainNode(int index, float[] loc, string inRegion, string inDescription)
        {
            node = Instantiate(node, new Vector3(loc[0], loc[1], loc[2]));
            node.name = "RollerBall";
			region = inRegion;
			desc = inDescription;
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
			return 0;
		}
    }
}
