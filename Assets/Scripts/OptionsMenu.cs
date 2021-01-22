using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Variables;

public class OptionsMenu : MonoBehaviour
{
    public void XAxis3(bool active) //x axis
    {
        if (active)
        {
            ChangeCubeSize(Axis.x, 3);
        }
    }
    public void XAxis4(bool active)
    {
        if (active)
        {
            ChangeCubeSize(Axis.x, 4);
        }
    }
    public void XAxis5(bool active)
    {
        if (active)
        {
            ChangeCubeSize(Axis.x, 5);
        }
    }

    public void YAxis3(bool active) //y axis
    {
        if (active)
        {
            ChangeCubeSize(Axis.y, 3);
        }
    }
    public void YAxis4(bool active)
    {
        if (active)
        {
            ChangeCubeSize(Axis.y, 4);
        }
    }
    public void YAxis5(bool active)
    {
        if (active)
        {
            ChangeCubeSize(Axis.y, 5);
        }
    }

    public void ZAxis3(bool active) //z axis
    {
        if (active)
        {
            ChangeCubeSize(Axis.z, 3);
        }
    }
    public void ZAxis4(bool active)
    {
        if (active)
        {
            ChangeCubeSize(Axis.z, 4);
        }
    }
    public void ZAxis5(bool active)
    {
        if (active)
        {
            ChangeCubeSize(Axis.z, 5);
        }
    }

    public void Full(bool active)
    {
        if (active)
        {
            GameSettings.instance.gameMode = GameMode.FullCubeFromStart;
        }
    }
    public void Layer(bool active)
    {
        if (active)
        {
            GameSettings.instance.gameMode = GameMode.BuildCubeBottomToTop;
        }
    }
    public void ChangeCubeSize(Axis axis, int value)
    {
        Debug.Log("Cube Size Changed");

        switch (axis)
        {
            case Axis.x:
                GameSettings.instance.BoardEdgeX = value;
                break;
            case Axis.y:
                GameSettings.instance.BoardEdgeY = value;
                break;
            case Axis.z:
                GameSettings.instance.BoardEdgeZ = value;
                break;
        }
    }
}
