using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    public bool isGoaled = false;

    [SerializeField] private float forceCollision = 10.0f;

    private Material redTrail;
    private Material blueTrail;
    [SerializeField] private Material grayTrail;
    public AudioClip ballSound;
    private AudioSource audioSource;

    GameManager gameManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = GameManager.GetInstance();

        redTrail = gameManager.redMat;
        blueTrail = gameManager.blueMat;
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
            GetComponent<TrailRenderer>().material = redTrail;
        }
        else if (transform.position.z <= -15) // Gauche
        {
            GetComponent<TrailRenderer>().material = blueTrail;
        }
        else // Mid
        {
            GetComponent<TrailRenderer>().material = grayTrail;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        audioSource.Play();

        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Player2"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();


            Vector3 direction = (collision.transform.position - transform.position).normalized;

            Vector3 force = direction * playerRb.velocity.magnitude * Time.deltaTime * forceCollision;
            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}