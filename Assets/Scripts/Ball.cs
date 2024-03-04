using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    public bool isGoaled = false;

    [SerializeField] private float forceCollision = 10.0f;

    [SerializeField] private Material RedTrail;
    [SerializeField] private Material BlueTrail;
    [SerializeField] private Material GrayTrail;
    public AudioClip ballSound;
    private AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = ballSound;
    }
    // Update is called once per frame
    void Update()
    {
        if(transform.position.z >= 15) // Droite
        {
            GetComponent<TrailRenderer>().material = RedTrail;
        }
        else if (transform.position.z <= -15) // Gauche
        {
            GetComponent<TrailRenderer>().material = BlueTrail;
        }
        else // Mid
        {
            GetComponent<TrailRenderer>().material = GrayTrail;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            audioSource.Play();
        }

        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Player2"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();


            Vector3 direction = (collision.transform.position - transform.position).normalized;

            Vector3 force = direction * playerRb.velocity.magnitude * Time.deltaTime * forceCollision;
            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}