using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip goalSound;
    private AudioSource audioSource;

    private GameObject particle;

    private GameManager gameManager;
    private Material redMat;
    private Material blueMat;

    private GameObject ball;


    private void Awake()
    {
        gameManager = GameManager.GetInstance();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = goalSound;


        particle = gameManager.particle;
        redMat = gameManager.redMat;
        blueMat = gameManager.blueMat;
        ball = gameManager.ball;
    }

    private void OnTriggerEnter(Collider other)
    {

        Goal.PlayerGoal goal = transform.parent.GetComponent<Goal>().GetGoal();

        if (other.gameObject == ball)
        {
            audioSource.Play();

            if (goal == Goal.PlayerGoal.Player_2) 
            {
                ParticleSystem(goal);
                StartCoroutine(GameManager.GetInstance().GoalDone(goal));
            }
            else if (goal == Goal.PlayerGoal.Player_1)
            {
                ParticleSystem(goal);
                StartCoroutine(GameManager.GetInstance().GoalDone(goal));
            }
        }

    }


    private void ParticleSystem(Goal.PlayerGoal playerGoal)
    {
        particle.transform.position = ball.transform.position;

        if (playerGoal == Goal.PlayerGoal.Player_2)
        {
            particle.GetComponent<ParticleSystemRenderer>().material = blueMat;
        }
        else
        {
            particle.GetComponent<ParticleSystemRenderer>().material = redMat;
        }

        particle.GetComponent<ParticleSystem>().Play();
    }
}
