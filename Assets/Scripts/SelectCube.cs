using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Variables;

public class SelectCube : MonoBehaviour
{
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            GameObject go = hit.transform.gameObject;

            if (go != null)
            {
                TurnOffLights lights = go.GetComponent<TurnOffLights>();

                if (lights != null)
                {
                    if (go.GetComponent<Cube>().GetTeam() == Team.none)
                    {
                        lights.counter = 1;
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    cubeClicked(go);
                }
            }
        }

    }

    private void cubeClicked(GameObject go)
    {
        GameManager.instance.CubeClick(go.GetComponent<Cube>());

    }
}
