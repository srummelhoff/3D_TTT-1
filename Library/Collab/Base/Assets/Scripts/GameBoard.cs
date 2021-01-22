using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Variables;

public class GameBoard : MonoBehaviour
{
    public static GameBoard instance;

    [SerializeField]
    private GameObject cubePrefab;
    [SerializeField]
    private int boardEdgeX = 4;  // number of cubes on an edge
    [SerializeField]
    private int boardEdgeY = 4;
    [SerializeField]
    private int boardEdgeZ = 4;
    
    private Vector3 spacing;

    private Vector3 v3Center;

    private GameObject[,,] wireFrame;

    private void Awake()
    {
        if (GameBoard.instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        
        wireFrame = new GameObject[boardEdgeX, boardEdgeY, boardEdgeZ];
        spacing = new Vector3(transform.localScale.x / boardEdgeX, transform.localScale.y / boardEdgeY, transform.localScale.z / boardEdgeZ);
        v3Center = transform.position;

        GetComponent<MeshRenderer>().enabled = false; //turns off mesh renderer
    }

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {

        for (int i = 0; i < boardEdgeX; i++) //spawns a 3d grid of cubes
        {
            for (int j = 0; j < boardEdgeY; j++)
            {
                for (int k = 0; k < boardEdgeZ; k++)
                {
                    float x = v3Center.x - (transform.localScale.x / 2.0f) + (0.5f * spacing.x) + (i * spacing.x);
                    float y = v3Center.y - (transform.localScale.y / 2.0f) + (0.5f * spacing.y) + (j * spacing.y);
                    float z = v3Center.z - (transform.localScale.z / 2.0f) + (0.5f * spacing.z) + (k * spacing.z);

                    GameObject newCube = Instantiate(cubePrefab, new Vector3(x, y, z), Quaternion.identity);
                    newCube.transform.SetParent(this.transform);
                    newCube.GetComponent<Cube>().SetPosInGrid(new Vector3Int(i, j, k));
                    wireFrame[i, j, k] = newCube;
                }
            }
        }
    }

    public void CheckWin()
    {

        for (int y = 0; y < wireFrame.GetLength(0); y++) //everything at X=0
        {
            for (int z = 0; z < wireFrame.GetLength(0); z++)
            {
                Cube cube = wireFrame[0, y, z].GetComponent<Cube>();
                Team team = cube.GetTeam();

                if (team != Team.none) //if it has a team
                {
                    for (int x = 1; x < wireFrame.GetLength(0); x++) //for each item on the x axis
                    {
                        if (team != wireFrame[x, y, z].GetComponent<Cube>().GetTeam())
                        {
                            break;
                        }

                        if (x == wireFrame.GetLength(0) - 1) //if we've looped to the end of the row and the teams were all the same,
                        {
                            Debug.Log($"Win at 0,{y},{z}");
                        }
                    }
                }
            }
        }

        for (int x = 0; x < wireFrame.GetLength(0); x++) //everything at Y=0
        {
            for (int z = 0; z < wireFrame.GetLength(0); z++)
            {
                Cube cube = wireFrame[x, 0, z].GetComponent<Cube>();
                Team team = cube.GetTeam();

                if (team != Team.none) //if it has a team
                {
                    for (int y = 1; y < wireFrame.GetLength(0); y++) //for each item on the y axis
                    {
                        if (team != wireFrame[x, y, z].GetComponent<Cube>().GetTeam())
                        {
                            break;
                        }

                        if (y == wireFrame.GetLength(1) - 1) //if we've looped to the end of the row and the teams were all the same,
                        {
                            Debug.Log($"Win at {x},0,{z}");
                        }
                    }
                }
            }
        }

        for (int x = 0; x < wireFrame.GetLength(0); x++) //everything at Z=0
        {
            for (int y = 0; y < wireFrame.GetLength(0); y++)
            {
                Cube cube = wireFrame[x, y, 0].GetComponent<Cube>();
                Team team = cube.GetTeam();

                if (team != Team.none) //if it has a team
                {
                    for (int z = 1; z < wireFrame.GetLength(0); z++) //for each item on the z axis
                    {
                        if (team != wireFrame[x, y, z].GetComponent<Cube>().GetTeam())
                        {
                            break;
                        }

                        if (z == wireFrame.GetLength(2) - 1) //if we've looped to the end of the row and the teams were all the same,
                        {
                            Debug.Log($"Win at {x},{y},0");
                        }
                    }
                }
            }
        }

        if (boardEdgeX == boardEdgeY && boardEdgeX == boardEdgeZ)
        {


        }




        //for (int x = 0; x < wireFrame.GetLength(0); x++) //everything at Y=0
        //{
        //    for (int z = 0; z < wireFrame.GetLength(0); z++)
        //    {
        //        Debug.Log(wireFrame[x, 0, z].GetComponent<Cube>().GetPosInGrid());
        //    }
        //}

        //for (int x = 0; x < wireFrame.GetLength(0); x++) //everything at Z=0
        //{
        //    for (int y = 0; y < wireFrame.GetLength(0); y++)
        //    {
        //        Debug.Log(wireFrame[x, y, 0].GetComponent<Cube>().GetPosInGrid());
        //    }
        //}


        //foreach (GameObject g in wireFrame)
        //{
        //    Cube cube = g.GetComponent<Cube>();
        //    Team team = cube.GetTeam();
        //    Vector3Int pos = cube.GetPosInGrid();

        //    if (team != Team.none) //if it has a team
        //    {
        //        for (int i = 0; i < wireFrame.GetLength(0); i++) //for each item on the x axis
        //        {
        //            Cube nextCube = wireFrame[i, pos.y, pos.z].GetComponent<Cube>();

        //            if (team != nextCube.GetTeam())
        //            {
        //                break;
        //            }

        //            if (i == wireFrame.GetLength(0) - 1) //if we've looped to the end of the row and the teams were all the same,
        //            {
        //                Debug.Log("Win between " + cube + " and " + nextCube + " along the X Axis");
        //            }
        //        }

        //        for (int i = 0; i < wireFrame.GetLength(1); i++) //for each item on the y axis
        //        {
        //            Cube nextCube = wireFrame[pos.x, i, pos.z].GetComponent<Cube>();

        //            if (team != nextCube.GetTeam())
        //            {
        //                break;
        //            }

        //            if (i == wireFrame.GetLength(1) - 1) //if we've looped to the end of the row and the teams were all the same,
        //            {
        //                Debug.Log("Win between " + cube + " and " + nextCube + " along the Y Axis");
        //            }
        //        }

        //        for (int i = 0; i < wireFrame.GetLength(2); i++) //for each item on the z axis
        //        {
        //            Cube nextCube = wireFrame[pos.x, pos.y, i].GetComponent<Cube>();

        //            if (team != nextCube.GetTeam())
        //            {
        //                break;
        //            }

        //            if (i == wireFrame.GetLength(2) - 1) //if we've looped to the end of the row and the teams were all the same,
        //            {
        //                Debug.Log("Win between " + cube + " and " + nextCube + " along the Z Axis");
        //            }
        //        }
        //    }
        //}

        CheckNoMoreCubes();

        //does it have a team?
        //check 1 block over on X axis
        //exists?
        //same team?
        //proceed to next block until they don't exist

    }

    public void CheckNoMoreCubes()
    {
        int cubesClickable = 0;

        foreach (GameObject g in wireFrame)
        {
            if (g.GetComponent<Cube>().GetTeam() == Team.none) //if we find a single cube that is available for clicking,
            {
                ++cubesClickable;
            }
        }

        if (cubesClickable == 0)
        {
            Debug.Log("Game Over, no more clickable cubes"); //check for even score as well
        }
    }

    //public void setX3() //i thought this said "s3xt"
    //{
    //    boardEdgeX = 3;
    //}
    //public void setX4()
    //{
    //    boardEdgeX = 3;
    //}
    //public void setX5()
    //{
    //    boardEdgeX = 5;
    //}
    //public void setY3()
    //{
    //    boardEdgeY = 3;
    //}
    //public void setY4()
    //{
    //    boardEdgeY = 3;
    //}
    //public void setY5()
    //{
    //    boardEdgeY = 5;
    //}
    //public void setZ3()
    //{
    //    boardEdgeZ = 3;
    //}
    //public void setZ4()
    //{
    //    boardEdgeZ = 3;
    //}
    //public void setZ5()
    //{
    //    boardEdgeZ = 5;
    //}
}