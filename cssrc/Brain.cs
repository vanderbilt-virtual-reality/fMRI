using System;

public class Brain
{
    private BrainNode[] brainNodes;

    public Brain(String[][] loc, float[][] tuples) {
        brainNodes = new BrainNode[96];
        // String[][] loc = Instantiate.ReadCSVFile();
        
        for (int i = 0; i < brainNodes.length; i++) {
            int coordinates = new int[3];
            Array.Copy(loc[i], 2, coordinates, 0, 3);
            brainNodes[i] = new BrainNode(i, coordinates, loc[i][0], "description");
        }

        updateBold(tuples);
    }

    public updateBold(float[][] tuples) {
        for (int i = 0; i < tuples.length; i++) {

        }
    }
}
