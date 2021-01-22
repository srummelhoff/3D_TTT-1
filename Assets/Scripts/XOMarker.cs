using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Variables;

public class XOMarker : MonoBehaviour
{
    private Cube cube;
    public GameObject X;
    public GameObject O;

    private void Start()
    {
        cube = GetComponent<Cube>();
    }
    private void Update()
    {
        if (cube.GetTeam() == Team.none)
        {
            X.SetActive(false);
            O.SetActive(false);
        }
        else if (cube.GetTeam() == Team.X)
        {
            X.SetActive(true);
            O.SetActive(false);
        }
        else if (cube.GetTeam() == Team.O)
        {
            X.SetActive(false);
            O.SetActive(true);
        }
    }
}
