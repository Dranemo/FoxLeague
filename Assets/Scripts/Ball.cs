using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float forceCollision = 10.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {        


        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Player2"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();


            Vector3 direction = (collision.transform.position - transform.position).normalized;

            Vector3 force = direction * playerRb.velocity.magnitude * Time.deltaTime * forceCollision;
            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}