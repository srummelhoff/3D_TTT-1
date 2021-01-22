using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    [SerializeField]
    private GameObject camTilt;
    [SerializeField]
    private float rotateSpeedX = 1f;
    [SerializeField]
    private float rotateSpeedY = 0.5f;
    [SerializeField]
    private int topConstraint = 50;
    [SerializeField]
    private int botConstraint = 50;

    private Transform camRotation;

    void LateUpdate()
    {

        if (Input.GetMouseButton(2))
        {
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * rotateSpeedX, 0), Space.World);

            camTilt.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * rotateSpeedY, 0, 0), Space.Self);
            float newAngle = 0f;
            Debug.Log(new Vector3(-Input.GetAxis("Mouse Y") * rotateSpeedY, 0, 0));
            if (camTilt.transform.eulerAngles.x > (360 - botConstraint))
            {
                newAngle = (360 - botConstraint);
            }
            else if (camTilt.transform.eulerAngles.x > topConstraint)
            {
                newAngle = topConstraint;
            }
            else
            {
                newAngle = camTilt.transform.eulerAngles.x;
            }

            camTilt.transform.eulerAngles = new Vector3(newAngle, camTilt.transform.eulerAngles.y, camTilt.transform.eulerAngles.z);
        }
    }
}