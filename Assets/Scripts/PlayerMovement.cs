using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Vitesse de d�placement du joueur
    [SerializeField] private float angularSpeed = 5f;

    private Rigidbody rb;
    float horizontalInput = 0f;
    float verticalInput = 0f;
    Vector3 rotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        // V�rifie quel joueur est en train de jouer et ajuste les entr�es en cons�quence
        if (gameObject.CompareTag("Player2"))
        {
            horizontalInput = Input.GetAxis("Horizontal2");
            verticalInput = Input.GetAxis("Vertical2");
        }
        else
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }
        rotation = transform.up * horizontalInput * angularSpeed * Time.deltaTime;
    }
    private void FixedUpdate()
        {
        transform.Rotate(rotation);
        rb.AddForce(transform.TransformDirection(Vector3.forward) * verticalInput * speed);
        }
}


