using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Variables;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player playerO;
    public Player playerX;
    public bool paused = false;
    public int level = 0;

    [SerializeField]
    private GameObject player;

    private GameMode gameMode;
    private Team turn; //whose turn it is

    void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        turn = Team.X; //eventually make start random
    }

    private void Start()
    {
        playerO = Instantiate(player).GetComponent<Player>();
        playerO.team = Team.O;

        playerX = Instantiate(player).GetComponent<Player>();
        playerX.team = Team.X;

        if (FindObjectOfType<GameSettings>() != null && GameSettings.instance != null) //just checking for the instance returns an error if one doesn't exist, so we search it first
        {
            gameMode = GameSettings.instance.gameMode;
        }
        else
        {
            Debug.Log($"Missing GameSettings.instance! Using default game mode: {gameMode}");
        }
        UIManager.instance.UpdateTurn(turn);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause(!paused);
        }
    }


    public void Pause(bool state)
    {
        
        if (paused) //unpause
        {
            UIManager.instance.SummonPauseMenu(false);
            Time.timeScale = 1;
            paused = false;
        }
        else //pause
        {
            UIManager.instance.SummonPauseMenu(true);
            Time.timeScale = 0;
            paused = true;
        }
    }

    public void CubeClick(Cube cube) //player has clicked a cube
    {
        if (!paused)
        {
            if (cube.SetTeam(turn)) //if team setting is successful
            {
                if (turn == Team.X) //switch players
                {
                    turn = Team.O;
                }
                else
                {
                    turn = Team.X;
                }
                UIManager.instance.UpdateTurn(turn);
            }

            UpdateScore();

            if (gameMode == GameMode.BuildCubeBottomToTop)
            {
                int disabledCubes = GameBoard.instance.NumberOfEnabledCubes("disabled");

                if (disabledCubes != 0) //if there are some cubes still disabled
                {
                    if (disabledCubes == GameBoard.instance.NumberOfTeamlessCubes()) //if all disabled cubes have no team
                    {
                        GameBoard.instance.ClickableLayer(++level); //increment level and make that new layer clickable
                    }
                }
            }
        }
    }


    private void UpdateScore()
    {
        playerO.score = GameBoard.instance.TallyScore()[0]; //O's
        playerX.score = GameBoard.instance.TallyScore()[1]; //X's
        UIManager.instance.UpdateScoreUI(playerO.team, playerO.score);
        UIManager.instance.UpdateScoreUI(playerX.team, playerX.score);

        if (GameBoard.instance.NumberOfTeamlessCubes() <= 0)
        {
            Debug.Log($"Game Over! Winning team: {GameOver()}");
        }
    }

    private Team GameOver()
    {
        Team team;

        if (playerO.score == playerX.score) //stalemate
        {
            team = Team.none;
        }
        else if (playerO.score > playerX.score) //p1 wins
        {
            team = playerO.team;
        }
        else //p2 wins
        {
            team = playerX.team;
        }
        
        UIManager.instance.GameOver(team);
        return team;
    }

}