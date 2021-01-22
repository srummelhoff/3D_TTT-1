using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Game.Variables;

public class GameSettings : MonoBehaviour
{
    public static GameSettings instance;
    public GameMode gameMode;
    public int BoardEdgeX = 3;
    public int BoardEdgeY = 3;
    public int BoardEdgeZ = 3;

    public void Awake()
    {
        if(GameSettings.instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this);
    }

}