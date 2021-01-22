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
    [SerializeField]
    private float cubeScaleModifier = 2f; // divide spacing by this number to get cube scale

    private Vector3 spacing;
    private Vector3 v3Center;
    private float cubeScale = 1;
    private GameMode gameMode;

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
    }

    private void Start()
    {
        if (FindObjectOfType<GameSettings>() != null && GameSettings.instance != null) //just checking for the instance returns an error if one doesn't exist, so we search it first
        {
            boardEdgeX = GameSettings.instance.BoardEdgeX;
            boardEdgeY = GameSettings.instance.BoardEdgeY;
            boardEdgeZ = GameSettings.instance.BoardEdgeZ;
            gameMode = GameSettings.instance.gameMode;
        }
        else
        {
            Debug.Log($"Missing GameSettings.instance! Using default values: {boardEdgeX},{boardEdgeY},{boardEdgeZ}");
            Debug.Log($"Missing GameSettings.instance! Using default game mode: {gameMode}");

        }

        wireFrame = new GameObject[boardEdgeX, boardEdgeY, boardEdgeZ];
        spacing = new Vector3(transform.localScale.x / boardEdgeX, transform.localScale.y / boardEdgeY, transform.localScale.z / boardEdgeZ);

        if (cubeScaleModifier <= 1)
        {
            Debug.Log("Warning, cube scale set to 1 or larger. center cubes may not be clickable!");
        }
            cubeScale = Mathf.Min(spacing.x / cubeScaleModifier, spacing.y / cubeScaleModifier, spacing.z / cubeScaleModifier);
        


        v3Center = transform.position;

        GetComponent<MeshRenderer>().enabled = false; //turns off mesh renderer
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

                    newCube.transform.localScale = new Vector3(cubeScale, cubeScale, cubeScale);
                    newCube.transform.SetParent(this.transform);
                    newCube.GetComponent<Cube>().SetPosInGrid(new Vector3Int(i, j, k));
                    wireFrame[i, j, k] = newCube;
                }
            }
        }

        if (gameMode == GameMode.FullCubeFromStart)
        {
            foreach (GameObject go in wireFrame)
            {
                go.GetComponent<Cube>().MakeClickable(true);
            }
        }
        else if (gameMode == GameMode.BuildCubeBottomToTop)
        {
            ClickableLayer(0);
        }
    }

    public void ClickableLayer(int layer) //input the HIGHEST CLICKABLE layer (vertically)
    {
        for (int x = 0; x < boardEdgeX; x++)
        {
            for (int y = 0; y < boardEdgeY; y++)
            {
                for (int z = 0; z < boardEdgeZ; z++)
                {
                    Cube cube = wireFrame[x, y, z].GetComponent<Cube>();

                    if (y <= layer) //if on or below clickable layer, enable. if above clickable layer, disable
                    {
                        cube.MakeClickable(true);
                    }
                    else
                    {
                        cube.MakeClickable(false);
                    }

                }
            }
        }
    }

    public int[] TallyScore()
    {
        int[] score = { 0, 0 }; // O's = pos 0    // X's = pos 1

        //straight lines
        for (int y = 0; y < wireFrame.GetLength(1); y++) //everything at X=0
        {
            for (int z = 0; z < wireFrame.GetLength(2); z++)
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
                            score[0] += AddScoreO(team);
                            score[1] += AddScoreX(team);

                            //Debug.Log($"Point for {team}'s at 0,{y},{z}");
                        }
                    }
                }
            }
        }

        for (int x = 0; x < wireFrame.GetLength(0); x++) //everything at Y=0
        {
            for (int z = 0; z < wireFrame.GetLength(2); z++)
            {
                Cube cube = wireFrame[x, 0, z].GetComponent<Cube>();
                Team team = cube.GetTeam();

                if (team != Team.none) //if it has a team
                {
                    for (int y = 1; y < wireFrame.GetLength(1); y++) //for each item on the y axis
                    {
                        if (team != wireFrame[x, y, z].GetComponent<Cube>().GetTeam())
                        {
                            break;
                        }

                        if (y == wireFrame.GetLength(1) - 1) //if we've looped to the end of the row and the teams were all the same,
                        {
                            score[0] += AddScoreO(team);
                            score[1] += AddScoreX(team);

                            //Debug.Log($"Point for {team}'s at {x},0,{z}");
                        }
                    }
                }
            }
        }

        for (int x = 0; x < wireFrame.GetLength(0); x++) //everything at Z=0
        {
            for (int y = 0; y < wireFrame.GetLength(1); y++)
            {
                Cube cube = wireFrame[x, y, 0].GetComponent<Cube>();
                Team team = cube.GetTeam();

                if (team != Team.none) //if it has a team
                {
                    for (int z = 1; z < wireFrame.GetLength(2); z++) //for each item on the z axis
                    {
                        if (team != wireFrame[x, y, z].GetComponent<Cube>().GetTeam())
                        {
                            break;
                        }

                        if (z == wireFrame.GetLength(2) - 1) //if we've looped to the end of the row and the teams were all the same,
                        {
                            score[0] += AddScoreO(team);
                            score[1] += AddScoreX(team);

                            //Debug.Log($"Point for {team}'s at {x},{y},0");
                        }
                    }
                }
            }
        }
        
        //Diagonals
        //XY same
        if (boardEdgeX == boardEdgeY)
        {
            for (int z = 0; z < wireFrame.GetLength(2); z++) //increment along z axis
            {
                Cube cube = wireFrame[0, 0, z].GetComponent<Cube>();
                Team team = cube.GetTeam();

                if (team != Team.none) //if it has a team
                {
                    for (int i = 1; i < wireFrame.GetLength(0); i++) //for each item on both X and Y axes
                    {
                        if (team != wireFrame[i, i, z].GetComponent<Cube>().GetTeam())
                        {
                            break;
                        }

                        if (i == wireFrame.GetLength(0) - 1) //if we've looped to the end of the diagonal and the teams were all the same
                        {
                            score[0] += AddScoreO(team);
                            score[1] += AddScoreX(team);

                            //Debug.Log($"Point for {team}'s at 0,0,{z}");
                        }
                    }
                }

                cube = wireFrame[0, wireFrame.GetLength(1) - 1, z].GetComponent<Cube>(); //restart at the new cube
                team = cube.GetTeam();

                if (team != Team.none) //if it has a team
                {
                    for (int i = 1; i < wireFrame.GetLength(0); i++) //for each item on both X and Y axes
                    {
                        if (team != wireFrame[i, wireFrame.GetLength(1) - 1 - i, z].GetComponent<Cube>().GetTeam()) //subtract the incrememnted number from the max to count DOWN instead
                        {
                            break;
                        }

                        if (i == wireFrame.GetLength(0) - 1) //if we've looped to the end of the diagonal and the teams were all the same
                        {
                            score[0] += AddScoreO(team);
                            score[1] += AddScoreX(team);

                            //Debug.Log($"Point for {team}'s at 0,{wireFrame.GetLength(1) - 1},{z}");
                        }
                    }
                }
            }
        }

        //XZ same
        if (boardEdgeX == boardEdgeZ)
        {
            for (int y = 0; y < wireFrame.GetLength(1); y++) //increment along y axis
            {
                Cube cube = wireFrame[0, y, 0].GetComponent<Cube>();
                Team team = cube.GetTeam();

                if (team != Team.none) //if it has a team
                {
                    for (int i = 1; i < wireFrame.GetLength(0); i++) //for each item on both x and z axes
                    {
                        if (team != wireFrame[i, y, i].GetComponent<Cube>().GetTeam())
                        {
                            break;
                        }

                        if (i == wireFrame.GetLength(0) - 1) //if we've looped to the end of the diagonal and the teams were all the same
                        {
                            score[0] += AddScoreO(team);
                            score[1] += AddScoreX(team);

                            //Debug.Log($"Point for {team}'s at 0,{y},0");
                        }
                    }
                }

                cube = wireFrame[0, y, wireFrame.GetLength(2) - 1].GetComponent<Cube>(); //restart at the new cube
                team = cube.GetTeam();

                if (team != Team.none) //if it has a team
                {
                    for (int i = 1; i < wireFrame.GetLength(0); i++) //for each item on both X and Z axes
                    {
                        if (team != wireFrame[i, y, wireFrame.GetLength(2) - 1 - i].GetComponent<Cube>().GetTeam()) //subtract the incrememnted number from the max to count DOWN instead
                        {
                            break;
                        }

                        if (i == wireFrame.GetLength(0) - 1) //if we've looped to the end of the diagonal and the teams were all the same
                        {
                            score[0] += AddScoreO(team);
                            score[1] += AddScoreX(team);

                            //Debug.Log($"Point for {team}'s at 0,{y},{wireFrame.GetLength(2) - 1}");
                        }
                    }
                }
            }
        }

        //YZ same
        if (boardEdgeY == boardEdgeZ)
        {
            for (int x = 0; x < wireFrame.GetLength(0); x++) //increment along x axis
            {
                Cube cube = wireFrame[x, 0, 0].GetComponent<Cube>();
                Team team = cube.GetTeam();

                if (team != Team.none) //if it has a team
                {
                    for (int i = 1; i < wireFrame.GetLength(1); i++) //for each item on both y and z axes
                    {
                        if (team != wireFrame[x, i, i].GetComponent<Cube>().GetTeam())
                        {
                            break;
                        }

                        if (i == wireFrame.GetLength(1) - 1) //if we've looped to the end of the diagonal and the teams were all the same
                        {
                            score[0] += AddScoreO(team);
                            score[1] += AddScoreX(team);

                            //Debug.Log($"Point for {team}'s at {x},0,0");
                        }
                    }
                }

                cube = wireFrame[x, 0, wireFrame.GetLength(2) - 1].GetComponent<Cube>(); //restart at the new cube
                team = cube.GetTeam();

                if (team != Team.none) //if it has a team
                {
                    for (int i = 1; i < wireFrame.GetLength(1); i++) //for each item on both X and Z axes
                    {
                        if (team != wireFrame[x, i, wireFrame.GetLength(2) - 1 - i].GetComponent<Cube>().GetTeam()) //subtract the incrememnted number from the max to count DOWN instead
                        {
                            break;
                        }

                        if (i == wireFrame.GetLength(1) - 1) //if we've looped to the end of the diagonal and the teams were all the same
                        {
                            score[0] += AddScoreO(team);
                            score[1] += AddScoreX(team);

                            //Debug.Log($"Point for {team}'s at {x},0,{wireFrame.GetLength(2) - 1}");
                        }
                    }
                }
            }
        }

        //all sides same
        if (boardEdgeX == boardEdgeY && boardEdgeX == boardEdgeZ)
        {
            //all permutations:
            //0,0,0
            //Max,0,0
            //0,Max,0
            //0,0,Max

            Cube cube = wireFrame[0, 0, 0].GetComponent<Cube>(); //0,0,0 incrementing on all axes every step
            Team team = cube.GetTeam();
            if (team != Team.none) //if it has a team
            {
                for (int i = 1; i < wireFrame.GetLength(0); i++) //for each item on all axes
                {
                    if (team != wireFrame[i, i, i].GetComponent<Cube>().GetTeam())
                    {
                        break;
                    }

                    if (i == wireFrame.GetLength(0) - 1) //if we've looped to the end of the diagonal and the teams were all the same
                    {
                        score[0] += AddScoreO(team);
                        score[1] += AddScoreX(team);

                        //Debug.Log($"Point for {team}'s at 0,0,0");
                    }
                }
            }

            cube = wireFrame[wireFrame.GetLength(0) - 1, 0, 0].GetComponent<Cube>(); //Max,0,0 incrementing on all axes every step
            team = cube.GetTeam();
            if (team != Team.none) //if it has a team
            {
                for (int i = 1; i < wireFrame.GetLength(0); i++) //for each item on all axes
                {
                    if (team != wireFrame[wireFrame.GetLength(0) - 1 - i, i, i].GetComponent<Cube>().GetTeam())
                    {
                        break;
                    }

                    if (i == wireFrame.GetLength(0) - 1) //if we've looped to the end of the diagonal and the teams were all the same
                    {
                        score[0] += AddScoreO(team);
                        score[1] += AddScoreX(team);

                        //Debug.Log($"Point for {team}'s at {wireFrame.GetLength(0) - 1},0,0");
                    }
                }
            }

            cube = wireFrame[0, wireFrame.GetLength(1) - 1, 0].GetComponent<Cube>(); //0,Max,0 incrementing on all axes every step
            team = cube.GetTeam();
            if (team != Team.none) //if it has a team
            {
                for (int i = 1; i < wireFrame.GetLength(0); i++) //for each item on all axes
                {
                    if (team != wireFrame[i, wireFrame.GetLength(1) - 1 - i, i].GetComponent<Cube>().GetTeam())
                    {
                        break;
                    }

                    if (i == wireFrame.GetLength(0) - 1) //if we've looped to the end of the diagonal and the teams were all the same
                    {
                        score[0] += AddScoreO(team);
                        score[1] += AddScoreX(team);

                        //Debug.Log($"Point for {team}'s at 0,{wireFrame.GetLength(1) - 1},0");
                    }
                }
            }

            cube = wireFrame[0, 0, wireFrame.GetLength(2) - 1].GetComponent<Cube>(); //0,0,Max incrementing on all axes every step
            team = cube.GetTeam();
            if (team != Team.none) //if it has a team
            {
                for (int i = 1; i < wireFrame.GetLength(0); i++) //for each item on all axes
                {
                    if (team != wireFrame[i, i, wireFrame.GetLength(2) - 1 -i].GetComponent<Cube>().GetTeam())
                    {
                        break;
                    }

                    if (i == wireFrame.GetLength(0) - 1) //if we've looped to the end of the diagonal and the teams were all the same
                    {
                        score[0] += AddScoreO(team);
                        score[1] += AddScoreX(team);

                        //Debug.Log($"Point for {team}'s at 0,0,{wireFrame.GetLength(2) - 1}");
                    }
                }
            }
        }
        return score;
    }

    private int AddScoreO(Team team) //only add if the team matches
    {
        if (team == Team.O)
        {
            return 1;
        }
        return 0;
    }
    private int AddScoreX(Team team) //only add if the team matches
    {
        if (team == Team.X)
        {
            return 1;
        }
        return 0;
    }

    public int NumberOfTeamlessCubes()
    {
        int teamlessCubes = 0;

        foreach (GameObject g in wireFrame)
        {
            if (g.GetComponent<Cube>().GetTeam() == Team.none)
            {
                ++teamlessCubes;
            }
        }
        return teamlessCubes;
    }

    public int NumberOfEnabledCubes(string enabled = "Enabled")
    {
        int cubes = 0;

        foreach (GameObject g in wireFrame)
        {
            if (g.GetComponent<Cube>().clickable)
            {
                ++cubes;
            }
        }

        if (Equals(enabled.ToLower(), "disabled"))
        {
            cubes = wireFrame.Length - cubes;
        }

        return cubes;
    }
}