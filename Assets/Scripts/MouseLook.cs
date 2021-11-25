 using System;
 using UnityEngine;
 
 public class MouseLook : MonoBehaviour
 {
     public float mouseSensitivity = 100.0f;
     public float clampAngle = 80.0f;
 
     private float rotY = 0.0f; // rotation around the up/y axis
     private float rotX = 0.0f; // rotation around the right/x axis
 
     void Start ()
     {
         Vector3 rot = transform.localRotation.eulerAngles;
         rotY = rot.y;
         rotX = rot.x;
     }
 
     void Update ()
     {
         Vector3 rot = transform.localRotation.eulerAngles;
         rotY = rot.y;
         rotX = rot.x;

         float mouseY = -Input.GetAxis("Mouse Y");
 
         rotX += mouseY * mouseSensitivity * Time.deltaTime;

        //ger parent rotation
        Quaternion parentRot = transform.parent.rotation;

        rotY = parentRot.eulerAngles.y;
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);

        transform.rotation = localRotation;
     }
     
 }