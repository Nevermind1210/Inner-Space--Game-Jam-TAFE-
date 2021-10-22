using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    public float zSpeed;
    void Update()
    {
        transform.Rotate(xSpeed * Time.deltaTime,ySpeed * Time.deltaTime, 0);
    }
}
