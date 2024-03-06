using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speedNormal = 1250;
    [SerializeField] private float speedFlying = 700;
    [SerializeField] private float speedActual = 1250;

    [SerializeField] private float jumpSpeed = 100f;
    [SerializeField] private float boost = 100.0f;
    [SerializeField] private float flySpeed = 1500f;
    [SerializeField] private int distanceFrappe = 8;
    private bool canJump = false;
    bool canBoost = true;
    bool isBoostring = false;


    float horizontalInput = 0f;
    float verticalInput = 0f;
    bool jumpInput = false;
    bool boostInput = false;
    bool frappeInput = false;

    bool boostInputReleased = true;


    private Rigidbody rb;
    private GameObject ball;

    Vector3 movementX;
    Vector3 movementZ;
    Vector3 jumpVector = Vector3.zero;
    Vector3 jumpBoostVector = Vector3.zero;

    public float GetBoost()
    {
        return boost;
    }
    public void SetBoost(float set)
    {
        boost = set;
    }



    private float cameraDistortionBoost = 90f;
    private float cameraDistortionNormal = 60f;
    private float cameraDistortionActual = 60f;
    private bool speedChangedBool = false;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        ball = FindObjectOfType<Ball>().gameObject;
    }


    private void Update()
    {
        float speedAnim = GetComponent<Rigidbody>().velocity.magnitude;
        this.GetComponent<Animator>().SetFloat("MoveSpeed", speedAnim);


        // Get input
        if (gameObject.GetComponent<Player>().GetPlayerEnum() == Player.PlayerEnum.player2)
        {
            horizontalInput = Input.GetAxis("Horizontal2");
            verticalInput = Input.GetAxis("Vertical2");
            jumpInput = Input.GetButton("Jump2");
            boostInput = Input.GetButton("Boost2");
            frappeInput = Input.GetButtonDown("Frappe2");

            boostInputReleased = Input.GetButtonUp("Boost2");
        }
        else if (gameObject.GetComponent<Player>().GetPlayerEnum() == Player.PlayerEnum.player1)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            jumpInput = Input.GetButton("Jump");
            boostInput = Input.GetButton("Boost");
            frappeInput = Input.GetButtonDown("Frappe");

            boostInputReleased = Input.GetButtonUp("Boost");
        }


        // Mouvement
        movementZ = transform.TransformDirection(Vector3.forward) * verticalInput * speedActual;
        movementX = transform.TransformDirection(Vector3.right) * horizontalInput * speedActual;




        // Vecteurs déplacements saut & fly
        if (jumpInput && canJump)
        {
            jumpVector = Vector3.up * jumpSpeed;
            canJump = false;
        }
        else if (!canJump && jumpInput && boost > 0)
        {
            jumpBoostVector = Vector3.up * flySpeed;
        }


        // Changement de vitesse
        if (canJump && speedActual == speedFlying)
        {
            speedActual = speedNormal;
        }
        if(!canJump && speedActual == speedNormal)
        {
            speedActual = speedFlying;
        }


        // Frappe
        if (frappeInput)
        {
            if (Vector3.Distance(transform.position, ball.transform.position) <= distanceFrappe)
            {
                ball.GetComponent<Rigidbody>().AddForce((ball.transform.position - transform.position).normalized * 100);
                boost -= 30;
            }
            else
            {
                Debug.Log(Vector3.Distance(transform.position, ball.transform.position));
            }
        }



        // Sprint
        if (boostInput && boost > 0 && canBoost && !speedChangedBool && (movementX + movementZ != Vector3.zero))
        {
            speedActual *= 2;
            speedChangedBool = true;

            isBoostring = true;

        }
        else
        {
            if((boost <= 0 || !boostInput) && speedChangedBool)
            {
                Debug.Log("aa");
                canBoost = false;
            }

            if (!canBoost && speedChangedBool && isBoostring)
            {
                speedActual /= 2;
                speedChangedBool = false;
            }
        }

        // Savoir si la touche de sprint est relachée
        if (boostInputReleased)
        {
            canBoost = true;
        }

        // Collision avec un objet en dessous
        RaycastHit hit;
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hit, 1f))
        {
            if ((hit.collider.CompareTag("Floor") || hit.collider.CompareTag("Obstacle")) && !speedChangedBool)
            {
                // Refill the jetpack fuel
                RefillBoost();
                if (!canJump)
                {
                    canJump = true;
                }
                GetComponent<Rigidbody>().drag = 1.5f;
                this.GetComponent<Animator>().SetBool("Jump", false);
            }
        }





    }


    void RefillBoost()
    {
        boost += 100 * Time.deltaTime;
        boost = Mathf.Clamp(boost, 0f, 100);
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

            GetComponent<Rigidbody>().drag = 0.5f;
        }

        //Fly
        if (jumpBoostVector != Vector3.zero)
        {
            rb.AddForce(jumpBoostVector, ForceMode.Force);
            boost -= 1 * Time.deltaTime * 100;
            jumpBoostVector = Vector3.zero;
        }

        // Boost Camera Distortion
        if(speedChangedBool)
        {
            if (cameraDistortionActual < cameraDistortionBoost)
            {
                cameraDistortionActual += Time.deltaTime * 100;
            }
            else
            {
                cameraDistortionActual = cameraDistortionBoost;
            }
            boost -= Time.deltaTime * 50;
        }
        else if (!speedChangedBool)
        {
            if (cameraDistortionActual > cameraDistortionNormal)
            {
                cameraDistortionActual -= Time.deltaTime * 100;
            }
            else
            {
                cameraDistortionActual = cameraDistortionNormal;
            }
        }
        transform.Find("PlayerCamera").GetComponent<Camera>().fieldOfView = cameraDistortionActual;
    }
}

