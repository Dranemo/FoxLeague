using UnityEngine;

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

    ScoreCanvaManager scoreCanvaManager;
    float horizontalInput = 0f;
    float verticalInput = 0f;
    bool jumpInput = false;
    bool boostInput = false;
    bool frappeInput = false;
    bool pauseInput = false;
    bool boostInputReleased = true;


    private Rigidbody rb;
    private GameObject ball;
    private GameManager gameManager;

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
    private Animator animator;

    [SerializeField] private AudioClip hitSound;
    private AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        gameManager = GameManager.GetInstance();
    }

    private void Start()
    {
        ball = gameManager.ball;
        animator=GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = hitSound;
        scoreCanvaManager = ScoreCanvaManager.GetInstance();
    }

    private void Update()
    {
        float speedAnim = rb.velocity.magnitude;
        animator.SetFloat("MoveSpeed", speedAnim);


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
            pauseInput = Input.GetButtonDown("Echap");
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
            animator.SetBool("Jump", true);
            animator.Play("Jump");
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
            if (Vector3.Distance(transform.position, ball.transform.position) <= distanceFrappe && boost > 0)
            {
                audioSource.Play();
                ball.GetComponent<Rigidbody>().AddForce((ball.transform.position - transform.position).normalized * boost * 2);
                boost = 0;
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

        //Pause

        if (pauseInput)
        {
            gameManager.allKinetic(true);
            scoreCanvaManager.PauseUnpauseTime(true);
        }
        else
        {
            gameManager.allKinetic(false);
            scoreCanvaManager.PauseUnpauseTime(false);
        }

        // Savoir si la touche de sprint est relachée
        if (boostInputReleased)
        {
            canBoost = true;
        }

        // Collision avec un objet en dessous
        RaycastHit hit;
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hit, 0.2f))
        {
            if ((hit.collider.CompareTag("Floor") || hit.collider.CompareTag("Obstacle")) && !speedChangedBool)
            {
                animator.SetBool("Grounded", true);
                animator.Play("Movement");
                // Refill the jetpack fuel
                RefillBoost();
                if (!canJump)
                {
                    canJump = true;
                }
                GetComponent<Rigidbody>().drag = 1.5f;
                animator.SetBool("Jump", false);
            }
        }
        else
        {
            animator.SetBool("Grounded", false);
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

