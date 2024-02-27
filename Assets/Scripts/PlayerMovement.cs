using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1000f; 
    [SerializeField] private float angularSpeed = 150f;
    [SerializeField] private float jumpSpeed = 100.0f;
    private bool canJump = false;


    private Rigidbody rb;

    Vector3 movement;
    Vector3 rotation;
    Vector3 jumpVector = Vector3.zero;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Floor"))
        {
            canJump = true;
            Debug.Log(canJump);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        float horizontalInput = 0f;
        float verticalInput = 0f;
        bool jumpInput = false;


        if (gameObject.CompareTag("Player2"))
        {
            //horizontalInput = Input.GetAxis("Horizontal2");
            //verticalInput = Input.GetAxis("Vertical2");
        }
        else
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            jumpInput = Input.GetButton("Jump");
        }

        // Mouvement
        movement = transform.TransformDirection(Vector3.forward) * verticalInput * speed;
        

        if (jumpInput && canJump)
        {
            jumpVector = Vector3.up * jumpSpeed;
            Debug.Log("pk tu saute pa");
        }

        // Rotation
        rotation = transform.up * horizontalInput * angularSpeed * Time.deltaTime;



        
    }

    private void FixedUpdate()
    {
        transform.Rotate(rotation);
        rb.AddForce(movement);
        if(jumpVector != Vector3.zero)
        {
            rb.AddForce(jumpVector, ForceMode.Impulse);
            jumpVector = Vector3.zero;
            canJump = false;
        }
    }
}

