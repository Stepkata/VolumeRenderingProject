using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraMovement : MonoBehaviour
{
   public float moveSpeed = 10f; // Adjust the movement speed as needed
   
   void Start()
    {
        // Set the camera's background color to dark gray
        Camera.main.backgroundColor = new Color(0.2f, 0.2f, 0.2f); // Adjust the RGB values as needed
    }

    void Update()
    {
        // Move the camera backward on the "Q" key press
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }

        // Move the camera forward on the "E" key press
        if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }
}
