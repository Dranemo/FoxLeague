using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngineInternal;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    private Transform ballPos;
    private Transform playerPos;
    private Transform player2Pos;
    private Transform goal1Pos;
    private Transform goal2Pos;

    private GameObject scene; 


    [SerializeField] private GameObject birchTree;
    [SerializeField] private GameObject oakTree;
    [SerializeField] private GameObject rock01;
    [SerializeField] private GameObject rock02;
    [SerializeField] private GameObject rock03;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject canvaPrefab;
    [SerializeField] private GameObject goalPrefab;


    [SerializeField, Range(1,2)] public int playerNumber = 2;
    public int playerLoaded = 0;
    [SerializeField] private int numberObstacles = 10;
    [SerializeField] private int spread = 5;
    [SerializeField] private int minDistance = 3;


    [SerializeField] private Transform cam1;
    [SerializeField] private Transform cam2;


    private static GameManager instance;

    private GameObject ball;
    private GameObject player;
    private GameObject player2;
    private GameObject obstacles;

    ScoreCanvaManager scoreCanvaManager;
    [SerializeField] public int score1 = 0;
    [SerializeField] public int score2 = 0;

    private int WinP1 = 0;
    private int WinP2 = 0;

    private bool canAddTime = true;


    public static GameManager GetInstance()
    {
        if (instance)
        {
            return instance;
        }
        else
        {
            return instance = FindObjectOfType<GameManager>();
        }
    }


    private void Start()
    {
        scene = GameObject.Find("Scene");

        ballPos = scene.transform.Find("BallStartPosition");
        playerPos = scene.transform.Find("PlayerStartPosition");
        player2Pos = scene.transform.Find("Player2StartPosition");
        goal1Pos = scene.transform.Find("GoalStartPosition");
        goal2Pos = scene.transform.Find("Goal2StartPosition");


        GenerateTerrain();

        scoreCanvaManager = ScoreCanvaManager.GetInstance();

        FindItems();

        ResetPositions();
    }


    // --------------------------- LOGIQUE POSITIONS --------------------------- //
    public void ResetPositions()
    {
        //Reset la balle
        ball.transform.localScale = Vector3.one * 20;
        ball.transform.position = ballPos.transform.position;
        ball.GetComponent<Ball>().isGoaled = false;

        //Reset le joueur 1
        player.transform.position = playerPos.transform.position;
        player.transform.LookAt(new Vector3(0, 0, 1));
        player.GetComponent<PlayerMovement>().SetFlyBoost(100);

        //Reset le joueur 2 ou l'IA
        player2.transform.position = player2Pos.transform.position;
        player2.transform.LookAt(new Vector3(0, 0, -1));
        player2.GetComponent<PlayerMovement>().SetFlyBoost(100);


        allKinetic(false);
        scoreCanvaManager.PauseUnpauseTime(false);
    }

    public void RepositionItems()
    {
        playerLoaded = 0;
    }

    private void FindItems()
    {
        player = GameObject.FindWithTag("Player");

        player2 = GameObject.FindWithTag("Player2");
        if (player2 == null)
        {
            player2 = GameObject.FindWithTag("AI");
        }
        ball = FindObjectOfType<Ball>().gameObject;
    }

    public void allKinetic(bool booleen)
    {
        ball.transform.GetComponent<Rigidbody>().isKinematic = booleen;
        player.transform.GetComponent<Rigidbody>().isKinematic = booleen;
        player2.transform.GetComponent<Rigidbody>().isKinematic = booleen;
    }

    public void ParticleSystem(Vector3 ballPosition)
    {
        GameObject particle = new GameObject();
        particle=GameObject.FindObjectOfType<ParticleSystem>().gameObject;
        particle.transform.position = ballPosition;
        particle.GetComponent<ParticleSystem>().Play();
    }

    private void GenerateTerrain()
    {
        GenerateOverlay();
        GenerateGoal();
        GeneratePlayer();
        GenerateBall();
        GenerateRandomObstacle();







        void GenerateOverlay()
        {
            GameObject OverlayGen = null;

            OverlayGen = Instantiate(canvaPrefab, Vector3.zero, Quaternion.identity);
            OverlayGen.name = "Overlay";
            OverlayGen.layer = 5;

            foreach (Transform childTransform in OverlayGen.transform)
            {
                childTransform.gameObject.layer = 5;
            }
        }

        void GenerateGoal()
        {
            GameObject goalGen = null;

            Quaternion inversion = Quaternion.Euler(0, 180, 0);
            goalGen = Instantiate(goalPrefab, goal1Pos.position, inversion);
            goalGen.name = "Goal1";
            goalGen.GetComponent<Goal>().SetGoal(Goal.PlayerGoal.Player_1);

            goalGen = Instantiate(goalPrefab, goal2Pos.position, Quaternion.identity);
            goalGen.name = "Goal2";
            goalGen.GetComponent<Goal>().SetGoal(Goal.PlayerGoal.Player_2);
        }

        void GeneratePlayer()
        {
            GameObject playerGen = null;

            // Joueur 1
            playerGen = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

            // Joueur 2
            playerGen = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }

        void GenerateBall()
        {
            GameObject ballGen = null;

            ballGen = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
            ballGen.name = "Ball";
        }

        void GenerateRandomObstacle()
        {
            List<Vector3> position = new();
            List<GameObject> obstaclesList = new();

            GameObject obstacles_ = new GameObject("Obstacles");
            obstacles = obstacles_;

            obstaclesList.Add(ballPos.gameObject);
            obstaclesList.Add(player2Pos.gameObject);
            obstaclesList.Add(playerPos.gameObject);



            Vector3 positionInitiale = new Vector3(-20, 0, -40);
            GameObject item = CreateNewItem(position.Count, positionInitiale);
            item.transform.parent = obstacles.transform;
            obstaclesList.Add(item);
            position.Add(positionInitiale);


            numberObstacles = Random.Range(30, 50);
            Debug.Log(numberObstacles);

            int safeCount = 0;
            while (position.Count > 0 && safeCount++ < numberObstacles)
            {

                int selectedIndex = Random.Range(0, position.Count);
                Vector3 point = position[selectedIndex];

                int tries = 400;
                while (tries-- > 0)
                {

                    Vector3 newPos = point + GetRandomPoint();

                    bool valid = true;
                    if(newPos.x >  20 || newPos.x < -20 || newPos.z > 40 || newPos.z < -40)
                    {
                        valid = false;
                    }

                    for (int i = 0; i < position.Count && valid; i++)
                    {
                        if (Vector3.Distance(newPos, new Vector3(obstaclesList[i].transform.position.x, 0, obstaclesList[i].transform.position.z)) < minDistance)
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (valid)
                    {
                        item = CreateNewItem(position.Count, newPos);

                        item.transform.parent = obstacles.transform;
                        obstaclesList.Add(item);
                        position.Add(newPos);
                        break;
                    }
                }

            }



            Vector3 GetRandomPoint()
            {
                Vector2 randomPoint = Random.insideUnitCircle;
                return new Vector3(randomPoint.x, 0, randomPoint.y) * spread;
            }

            GameObject GetPrefab()
            {
                int prefabGenerated = Random.Range(0, 5); // Quel prefab ?

                switch (prefabGenerated)
                {
                    case 0: // Birch tree
                        return birchTree;
                    case 1: // Oak tree
                        return oakTree;
                    case 2: // Rock01
                        return rock01;
                    case 3: // Rock02
                        return rock02;
                    case 4: // Rock03
                        return rock03;
                    default:
                        return null;
                }
            }

            GameObject CreateNewItem(int i, Vector3 newPos)
            {
                GameObject itemCreated = null;

                itemCreated = Instantiate(GetPrefab(), newPos, Quaternion.Euler(0, Random.Range(0, 360), 0));
                itemCreated.tag = "Obstacle";
                itemCreated.name = "item" + i;

                return itemCreated;
            }
        }
    }

    

    public void ReplaceRandomObstacle()
    {

    }


    // --------------------------- SCORES --------------------------- //
    public void AddScore(int playerId)
    {
        switch (playerId)
        {
            case 1:
                score1++;
                break;
            case 2:
                score2++;
                break;
            default:
                break;
        }

        scoreCanvaManager.WriteCanvaScore(score1, score2);
    }

    public IEnumerator GoalDone(int playerId)
    {
        if (!ball.GetComponent<Ball>().isGoaled)
        {
            ball.GetComponent<Ball>().isGoaled = true;
            //Mettre tout en pause
            allKinetic(true);
            //ParticleSystem(ball.transform.position);
            scoreCanvaManager.timePause = true;

            AddScore(playerId);

            //
            for (float i = 1; i >= 0; i -= 0.025f)
            {
                ball.transform.localScale *= i;
                yield return new WaitForSeconds(.05f);
            }

            ResetPositions();
        }
    }

    public void nextManche()
    {
        if(score1 == score2 && canAddTime)
        {
            scoreCanvaManager.PauseUnpauseTime(true);

            StartCoroutine(GoalDone(0));

            scoreCanvaManager.currentTime = 120;

            canAddTime = false;
        }
        else
        {
            if (score1 > score2)
            {
                WinP1++;
            }
            else if (score1 < score2)
            {
                WinP2++;
            }


            score1 = 0;
            score2 = 0;

            



            if (WinP1 >= 2 || WinP2 >= 2)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene("Victory_Screen");
            }

            else
            {
                RepositionItems();
                scoreCanvaManager.ResetCanva();
            }
        }
    }

    public void EndGame()
    {

    }
}
