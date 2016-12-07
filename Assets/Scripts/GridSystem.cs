using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GridSystem : MonoBehaviour
{
    public Cube grayObject;
    public Cube blueObject;
    public Cube greenObject;
    public Cube redObject;
    public Cube purpleObject;
    public Cube yellowObject;

    public GameLogic gameLogic;
    public PuzzlePiece puzzlePiece;

    public List<Cube> cubesList = new List<Cube>();
    public List<Cube> chainCubesList = new List<Cube>();
    public List<Cube> replacedCubesList = new List<Cube>();
    public List<Cube> treeList = new List<Cube>();

    void Start()
    {
        FillGridListGray();
        puzzlePiece.GeneratePuzzlePiece();
    }

    void Update()
    {

    }

    void FillGridListGray()
    {
        for (int i = 1; i < 6; i++)
        {
            for (int y = 1; y < 6; y++)
            {
                Vector3 pos = new Vector3(y - 3, 3 - i, 0f);
                Cube cube = (Cube)Instantiate(grayObject, pos, Quaternion.identity);

                cube.order = new Vector2(i, y);
                cube.position = new Vector2(y - 3, 3 - i);
                cube.cubeColor = Cube.CubeColor.Gray;

                cubesList.Add(cube);
            }
        }
    }

    bool PlaceCube(Vector3 clickPos, Cube c)
    {
        Cube clickedCube = GetClickedGridElement(clickPos);
        if (!clickedCube)
        {
          
            return false;
        }
        if (chainCubesList.Contains(clickedCube))
        {
        
            RollbackTo(clickedCube);
            return false;
        }

        else if (clickedCube.cubeColor == Cube.CubeColor.Gray)
        {
         
            if (chainCubesList.Count < puzzlePiece.puzzlePieces.Count)
            {

                if (puzzlePiece.puzzlePieces.Count > 1)
                {

                    if (chainCubesList.Count < 1 || isNeighbourTo(clickedCube, chainCubesList.Last()))
                    {

                        Cube cube = (Cube)Instantiate(c, RoundtoGrid(clickPos), Quaternion.identity);
                        cube.order = GridToOrder(RoundtoGrid(clickPos));
                        cube.position = RoundtoGrid(clickPos);
                        cube.cubeColor = c.cubeColor;
                        chainCubesList.Add(cube);
                        cubesList.Add(cube);
                        //replacedCubesList.Add(GetClickedGridElement(clickPos));
                        cubesList.Remove(clickedCube);
                        clickedCube.Kill();

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                
                    Cube cube = (Cube)Instantiate(c, RoundtoGrid(clickPos), Quaternion.identity);
                    cube.order = GridToOrder(RoundtoGrid(clickPos));
                    cube.position = RoundtoGrid(clickPos);
                    cube.cubeColor = c.cubeColor;
                    chainCubesList.Add(cube);
                    cubesList.Add(cube);
                    //replacedCubesList.Add(GetClickedGridElement(clickPos));
                    cubesList.Remove(clickedCube);
                    clickedCube.Kill();

                    return true;
                }

            }
            else
            {
            
                return false;
            }

        }
       
        else
        {
          
            return false;
        }
    }

    public void BeginChain(Vector3 clickPos)
    {
        if (chainCubesList.Count != puzzlePiece.puzzlePieces.Count)
        {
            PlaceCube(clickPos, puzzlePiece.puzzlePieces[chainCubesList.Count]);
        }
        else
        {
            PlaceCube(clickPos, puzzlePiece.puzzlePieces[chainCubesList.Count - 1]);
        }
     


    }

    public void EndChain()
    {

        CheckForPop();

        chainCubesList = new List<Cube>();
        replacedCubesList = new List<Cube>();
        puzzlePiece.NextPiece();

        CheckGameEnded();
    }

    private void RollBack()
    {
        foreach (Cube c in chainCubesList)
        {
            SpawnGrayInstead(c);
        }
        chainCubesList = new List<Cube>();
    }

    public Cube GetClickedGridElement(Vector3 clickPos)
    {
        Vector3 gridPosition = RoundtoGrid(clickPos);
        return cubesList.Where(x => x.position.x == gridPosition.x && x.position.y == gridPosition.y).FirstOrDefault();
    }

    public void CheckForPop()
    {
        treeList = new List<Cube>();

        for (int i = 0; i < chainCubesList.Count; i++)
        {
            FindTree(chainCubesList[i]);

            if (treeList.Count > 2)
            {
                PopChain(treeList);
            }
            treeList = new List<Cube>();
        }



    }

    public Vector3 RoundtoGrid(Vector3 clickPos)
    {
        Vector3 gridPosition = new Vector3(0f, 0f, 0f);
        gridPosition.x = clickPos.x > 0 ? Mathf.Round(clickPos.x) : Mathf.Round(clickPos.x);
        gridPosition.y = clickPos.y > 0 ? Mathf.Round(clickPos.y) : Mathf.Round(clickPos.y);

        return gridPosition;
    }

    public Vector2 GridToOrder(Vector3 grid)
    {
        Vector2 order = new Vector2(3 - grid.y, 3 + grid.x);
        return order;
    }

    public List<Cube> FindSameNeighbours(Cube c)
    {
        List<Cube> neighbourList = new List<Cube>();
        Vector2 order;
        Cube cube;
        //Left
        order = new Vector2(c.order.x, c.order.y - 1);
        cube = GetCubeFromOrder(order);
        if (isSameColored(c, cube))
        {
            neighbourList.Add(cube);
        }
        //Right
        order = new Vector2(c.order.x, c.order.y + 1);
        cube = GetCubeFromOrder(order);
        if (isSameColored(c, cube))
        {
            neighbourList.Add(cube);
        }
        //Top
        order = new Vector2(c.order.x - 1, c.order.y);
        cube = GetCubeFromOrder(order);
        if (isSameColored(c, cube))
        {
            neighbourList.Add(cube);
        }
        //Bottom
        order = new Vector2(c.order.x + 1, c.order.y);
        cube = GetCubeFromOrder(order);
        if (isSameColored(c, cube))
        {
            neighbourList.Add(cube);
        }

        return neighbourList;
    }

    public Cube GetCubeFromOrder(Vector2 order)
    {
        return cubesList.Where(x => x.order == order).FirstOrDefault();
    }

    public bool isSameColored(Cube cube1, Cube cube2)
    {
        if (cube1 != null && cube2 != null)
        {
            if (cube1.cubeColor == cube2.cubeColor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void PopChain(List<Cube> chain)
    {
        int score = 0;
        treeList = new List<Cube>();
        foreach (Cube c in chain)
        {
            score += chain.Count;

            SpawnGrayInstead(c);

          
        }
        gameLogic.AddScore(score);
        
    }

    public void FindTree(Cube c)
    {
        if (!treeList.Contains(c))
        {
            treeList.Add(c);
        }

        foreach (Cube cube in FindSameNeighbours(c).Except(treeList))
        {
            if (!treeList.Contains(cube))
            {
                treeList.Add(cube);
            }

            List<Cube> otherNeighbours = FindSameNeighbours(cube);
            if (otherNeighbours.Except(treeList).FirstOrDefault() != null)
            {
                FindTree(cube);
            }
        }
    }

    public bool isNeighbourTo(Cube cube1, Cube cube2)
    {
        if (Math.Abs(cube1.order.x - cube2.order.x) == 1 && cube1.order.y == cube2.order.y)
        {
            return true;
        }
        else if (Math.Abs(cube1.order.y - cube2.order.y) == 1 && cube1.order.x == cube2.order.x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void RollbackTo(Cube c)
    {
        int index = chainCubesList.FindIndex(x => x.order == c.order);

        for (int i = index + 1; i < chainCubesList.Count; i++)
        {
            Cube cube = chainCubesList[i];
            chainCubesList.Remove(cube);
            SpawnGrayInstead(cube);

        }
    }

    public void isChainOver()
    {
        if (chainCubesList.Count == puzzlePiece.puzzlePieces.Count)
        {
            EndChain();
        }
        else
        {
            RollBack();
        }
    }

    void SpawnGrayInstead(Cube c)
    {
        Vector3 pos = c.transform.position;
        Cube cube = (Cube)Instantiate(grayObject, pos, Quaternion.identity);

        cube.order = c.order;
        cube.position = c.position;
        cube.cubeColor = Cube.CubeColor.Gray;

        cubesList.Remove(c);
        cubesList.Add(cube);
        c.Kill();
    }

    public void CheckGameEnded()
    {
        bool gameEnded = true;
        List<Cube> grayList = cubesList.Where(x => x.cubeColor == Cube.CubeColor.Gray).ToList();
        
        foreach (Cube c in grayList)
        {
            FindTree(c);
            if (treeList.Count >= puzzlePiece.puzzlePieces.Count)
            {
                gameEnded = false;
            }
            treeList = new List<Cube>();
        }

        if (gameEnded)
        {
            gameLogic.EndGame();
        }
    } 
}
