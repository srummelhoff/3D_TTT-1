using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Variables;

public class CubeHover : MonoBehaviour
{
    public bool lightsOn;
    public GameObject selection;
    private Cube cube;

    private void Awake()
    {
        cube = GetComponent<Cube>();
    }

    public void Hovering()
    {
        if (cube.GetTeam() == Team.none)
        {
            selection.SetActive(true);
        }
    }
}