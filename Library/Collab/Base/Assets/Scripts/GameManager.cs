using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Variables;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    public void CubeClick(Cube cube) //player has clicked a cube
    {
        Debug.Log(cube + " hit");

        if (cube.setTeam(turn)) //if team setting is successful
        {
            //prompt dialogue
            //press enter
            //check win/stalemate

            if (turn == Team.X) //switch players
            {
                turn = Team.O;
            }
            else
            {
                turn = Team.X;
            }
            Debug.Log("it is now " + turn + "'s turn");
        }

    }

    //public void changeteam(cube)
    //{
    //  if(cube.getTeam == none){
    //  cube.setteam = whosever turn
   
}