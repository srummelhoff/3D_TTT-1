using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject camTilt;
    [SerializeField]
    private GameObject camOrbit;
    [SerializeField]
    private float rotateSpeedX = 1f;
    [SerializeField]
    private float rotateSpeedY = 0.5f;
    [SerializeField]
    private float tiltMin = -33f;
    [SerializeField]
    private float tiltMax = 29f;
    [SerializeField]
    private float scrollSpeed = 1f;
    [SerializeField]
    private float zoomOutMax = -15;
    [SerializeField]
    private float zoomInMax = -8;

    private float xRotation = 0f;

    void LateUpdate()
    {
        if (!GameManager.instance.paused)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) //zoom in
            {
                float zoomDistance = scrollSpeed * Time.deltaTime;

                transform.Translate(new Vector3(0, 0, zoomDistance));
                if (transform.localPosition.z > zoomInMax)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zoomInMax);
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) //zoom out
            {
                float zoomDistance = scrollSpeed * Time.deltaTime;

                transform.Translate(new Vector3(0, 0, -zoomDistance));
                if (transform.localPosition.z < zoomOutMax)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zoomOutMax);
                }
            }
            if (Input.GetMouseButton(2))
            {
                float mouseX = Input.GetAxis("Mouse X") * rotateSpeedX;
                float mouseY = Input.GetAxis("Mouse Y") * rotateSpeedY;

                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, tiltMin, tiltMax);

                camTilt.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                camOrbit.transform.Rotate(Vector3.up * mouseX);
            }
        }

        //if (Input.GetMouseButton(2)) //old, broken mouse code
        //{
        //    camOrbit.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * rotateSpeedX, 0), Space.World);
        //    camTilt.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * rotateSpeedY, 0, 0), Space.Self);
        //}
    }
}