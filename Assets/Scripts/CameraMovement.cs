using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
         moveCamera();
         lookAround();
    }

    void fixedUpdate()
    {
        // move the camera
       
    }

    //Function to move the camera from user input
    void moveCamera()
    {
        //Move the camera
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward *2 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back *2 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left *2 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right *2 * Time.deltaTime);
        }
    }

    //functiuon to look around with mouse
    void lookAround()
    {
        //move camera with mouse
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        transform.Rotate(-Vector3.down * mouseX * 2);
        transform.Rotate(-Vector3.right * mouseY *2);
    }



}
