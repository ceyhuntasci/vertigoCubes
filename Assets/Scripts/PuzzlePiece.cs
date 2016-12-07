using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{

    public List<Cube> puzzlePieces = new List<Cube>();
    public GridSystem gridSystem;

    public int oneFreq = 1;
    public int twoFreq = 3;
    public int threeFreq = 3;
    public int fourFreq = 1;

    public void GeneratePuzzlePiece()
    {         
        int count = DecideCount();
        float space;

        if (count == 1)
        {
            space = 0f;
        }
        else if (count == 2)
        {
            space = -0.5f;
        }
        else if (count == 3)
        {
            space = -1f;
        }
        else
        {
            space = -1.5f;
        }

        for (int i = 0; i < count; i++)
        {
            int colorNumber = Random.Range(1, 6);
            Vector3 pos = new Vector3(space, 4, 0f);
            if (colorNumber == 1)
            {
                Cube cube = (Cube)Instantiate(gridSystem.blueObject, pos, Quaternion.identity);
                cube.cubeColor = Cube.CubeColor.Blue;
                puzzlePieces.Add(cube);
            }
            else if (colorNumber == 2)
            {
                Cube cube = (Cube)Instantiate(gridSystem.greenObject, pos, Quaternion.identity);
                cube.cubeColor = Cube.CubeColor.Green;
                puzzlePieces.Add(cube);
            }
            else if (colorNumber == 3)
            {
                Cube cube = (Cube)Instantiate(gridSystem.redObject, pos, Quaternion.identity);
                cube.cubeColor = Cube.CubeColor.Red;
                puzzlePieces.Add(cube);
            }
            else if (colorNumber == 4)
            {
                Cube cube = (Cube)Instantiate(gridSystem.yellowObject, pos, Quaternion.identity);
                cube.cubeColor = Cube.CubeColor.Yellow;
                puzzlePieces.Add(cube);
            }
            else
            {
                Cube cube = (Cube)Instantiate(gridSystem.purpleObject, pos, Quaternion.identity);
                cube.cubeColor = Cube.CubeColor.Purple;
                puzzlePieces.Add(cube);
            }
            space += 1f;
        }


    }


    public void NextPiece()
    {
        foreach(Cube c in puzzlePieces)
        {
            c.Kill();
        }
        puzzlePieces = new List<Cube>();

        GeneratePuzzlePiece();

    }

    int DecideCount()
    {
        int totalFreq = oneFreq + twoFreq + threeFreq + fourFreq;

        int randomNumber = Random.Range(1,totalFreq+1);

        if (randomNumber<= oneFreq)
        {
            return 1;
        }
        else if (oneFreq<randomNumber && randomNumber <=oneFreq + twoFreq)
        {
            return 2;
        }
        else if (oneFreq + twoFreq < randomNumber && randomNumber <= oneFreq + twoFreq + threeFreq)
        {
            return 3;
        }
        else
        {
            return 4;
        }
    } 
}
