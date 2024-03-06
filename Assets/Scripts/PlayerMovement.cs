using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1000f; 
    [SerializeField] private float jumpSpeed = 100.0f;
    [SerializeField] private float flyBoost = 100.0f;
    [SerializeField] private float flySpeed = 100.0f;
    private bool canJump = false;


    private Rigidbody rb;

    Vector3 movementX;
    Vector3 movementZ;
    Vector3 jumpVector = Vector3.zero;
    Vector3 jumpBoostVector = Vector3.zero;

    public float GetFlyBoost()
    {
        return flyBoost;
    }
    public void SetFlyBoost(float set)
    {
        flyBoost = set;
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
        float speedAnim = GetComponent<Rigidbody>().velocity.magnitude;
        this.GetComponent<Animator>().SetFloat("MoveSpeed", speedAnim);


        // Get input
        if (gameObject.GetComponent<Player>().GetPlayerEnum() == Player.PlayerEnum.player2)
        {
            horizontalInput = Input.GetAxis("Horizontal2");
            verticalInput = Input.GetAxis("Vertical2");
            jumpInput = Input.GetButton("Jump2");
        }
        else if (gameObject.GetComponent<Player>().GetPlayerEnum() == Player.PlayerEnum.player1)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            jumpInput = Input.GetButton("Jump");
        }

        // Mouvement
        movementZ = transform.TransformDirection(Vector3.forward) * verticalInput * speed;
        movementX = transform.TransformDirection(Vector3.right) * horizontalInput * speed;


        if (jumpInput && canJump)
        {
            jumpVector = Vector3.up * jumpSpeed;
            this.GetComponent<Animator>().SetBool("Jump", true);
        }

        else if (canJump)
        {
            speed = 1250f;
        }

        else if (!canJump && jumpInput && flyBoost > 0)
        {
            jumpBoostVector = Vector3.up * flySpeed;
        }




        
    }


    void RefillJetpack()
    {
        flyBoost += 100 * Time.deltaTime;
        flyBoost = Mathf.Clamp(flyBoost, 0f, 100);
    }

    private void FixedUpdate()
    {
        rb.AddForce(movementX);
        rb.AddForce(movementZ);
        

        //Saut
        if (jumpVector != Vector3.zero)
        {
            rb.AddForce(jumpVector, ForceMode.Impulse);
            jumpVector = Vector3.zero;
            canJump = false;

            GetComponent<Rigidbody>().drag = 0.5f;
            speed = 625f;
        }

        //Fly
        if (jumpBoostVector != Vector3.zero)
        {
            rb.AddForce(jumpBoostVector, ForceMode.Force);
            flyBoost -= 1;
            jumpBoostVector = Vector3.zero;
        }





        // Collision avec sol/obstacle en dessous
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            if (hit.collider.CompareTag("Floor") || hit.collider.CompareTag("Obstacle"))
            {
                // Refill the jetpack fuel
                RefillJetpack();
                if (!canJump)
                {
                    canJump = true;
                }
                GetComponent<Rigidbody>().drag = 1.5f;
                this.GetComponent<Animator>().SetBool("Jump", false);
            }
        }


    }
}

