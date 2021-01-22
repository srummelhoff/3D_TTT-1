using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Variables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using TMPro.Examples;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public EndGameMenu endGameMenu;
    public PauseMenu optionsMenu;
    public GameObject hud;
    public TextMeshProUGUI XScore;
    public TextMeshProUGUI OScore;
    public TextMeshProUGUI playerTurn;
    public TextMeshProUGUI winner;

    public void Awake()
    {
        if (UIManager.instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        //DontDestroyOnLoad(this);
    }

    public void UpdateScoreUI(Team team, int value)
    {
        if (team == Team.O)
        {
            OScore.text = "O's : "  + value;
        }
        else
        {
            XScore.text = "X's : " + value;
        }
        
        Debug.Log($"UI: {team}'s score is now {value}");
    }

    public void UpdateTurn(Team team)
    {
        playerTurn.text = team.ToString() + "'s Turn";
        Debug.Log($"UI: It is now {team}'s turn");
    }

    public void GameOver(Team team)
    {
        if (team == Team.none)
        {
            Debug.Log("UI: Stalemate! No winner!");
        }
        else
        {
            Debug.Log($"UI: {team}'s win!");
        }
        winner.text = team.ToString() + " HAS WON!";
        endGameMenu.gameObject.SetActive(true);
        hud.SetActive(false);
    }

    public void SummonPauseMenu(bool pause)
    {
        if (pause) //unpause
        {
            optionsMenu.gameObject.SetActive(true);
            hud.SetActive(false);
        }

        else //pause
        {
            hud.SetActive(true);
            optionsMenu.gameObject.SetActive(false);
        }
    }

    //always on HUD:
    //x Current Turn (highlight)
    //x Current score


    //End Game
    //x "x Wins!"
    //[Play Again Button]
    //[Main Menu Button]
}
