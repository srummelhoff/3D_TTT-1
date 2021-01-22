using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Variables;

public class Cube : MonoBehaviour
{
    public bool clickable;

    private Team team = Team.none;
    [SerializeField]
    private Vector3Int posInGrid;
    [SerializeField]
    private Material matX;
    [SerializeField]
    private Material matO;
    [SerializeField]
    private Material matNone;
    [SerializeField]
    private Material matUnclickable;

    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    public bool SetTeam(Team tm)
    {
        if(team == Team.none)
        {
            team = tm;
            SetColor(team);
            return true;
        }
        return false;
    }

    public Vector3Int GetPosInGrid()
    {
        return posInGrid;
    }
    public void SetPosInGrid(Vector3Int newPos)
    {
        posInGrid = newPos;
    }

    public Team GetTeam()
    {
        return team;
    }

    private void SetColor(Team teamColor)
    {
        if (teamColor == Team.X)
        {
            meshRenderer.material = matX;
        }
        else if(teamColor == Team.O)
        {
            meshRenderer.material = matO;
        }
        else if (teamColor == Team.none)
        {
            meshRenderer.material = matNone;
        }
    }

    public void MakeClickable(bool value)
    {
        clickable = value;
        boxCollider.enabled = value;

        

        meshRenderer.enabled = value; //makes cubes invisible when disabled

        //if (value) //makes cubes change color when disabled
        //{
        //    SetColor(team);
        //}
        //else
        //{
        //    meshRenderer.material = matUnclickable;
        //}
    }
}