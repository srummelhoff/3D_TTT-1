using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffLights : MonoBehaviour
{
    public GameObject selection;
    public int counter = 0;

    private void Update()
    {
        if (counter == 0)
        {
            selection.SetActive(false);
        }
        else
        {
            selection.SetActive(true);
        }

        counter--;
        if (counter < 0)
        {
            counter = 0;
        }
    }
}