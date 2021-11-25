using UnityEngine;
using System.Collections;

// This script moves the character controller forward
// and sideways based on the arrow keys.
// It also jumps when pressing space.
// Make sure to attach a character controller to the same game object.
// It is recommended that you make only one call to Move or SimpleMove per frame.

public class CharacterMovement : MonoBehaviour
{
    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;

    public float mouseSensitivity = 40.0f;
     public float clampAngle = 80.0f;
 
     private float rotY = 0.0f; // rotation around the up/y axis
     private float rotX = 0.0f; // rotation around the right/x axis

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Vector3 rot = transform.localRotation.eulerAngles;
         rotY = rot.y;
         rotX = rot.x;
    }

    void Update()
    {
        
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes and rotation



            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        RotationUpdate();
    }

    void RotationUpdate(){
        float mouseX = Input.GetAxis("Mouse X");
 
         rotY += mouseX * mouseSensitivity * Time.deltaTime;
 
         Quaternion localRotation = Quaternion.Euler(0, rotY, 0.0f);
         transform.rotation = localRotation;
    }
}