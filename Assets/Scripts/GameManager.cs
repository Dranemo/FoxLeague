using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
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
        GenerateRandomObstacle(true);
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
    }
    /*void GenerateRandomObstacle(bool itemAlreadyThere = false)
    {
        Debug.Log("Coucou");

        if (!itemAlreadyThere)
        {
            //instancier un gameobject vide
            GameObject obstacles_ = new GameObject("Obstacles");
            numberObstacles = Random.Range(10, 40);


            obstacles = obstacles_;
        }
        

        //Creer une liste de toutes les positions des obstacles
        List<List<float>> positionList = new();

        //Creer une liste avec toutes les positions des start des items
        List<List<float>> listAlreadyItems = new();
        List<float> listAlreadyOneItem = new();


        listAlreadyOneItem.Add(ballPos.position.x);
        listAlreadyOneItem.Add(ballPos.position.z);
        listAlreadyItems.Add(listAlreadyOneItem);

        listAlreadyOneItem.Add(playerPos.position.x);
        listAlreadyOneItem.Add(playerPos.position.z);
        listAlreadyItems.Add(listAlreadyOneItem);

        listAlreadyOneItem.Add(player2Pos.position.x);
        listAlreadyOneItem.Add(player2Pos.position.z);
        listAlreadyItems.Add(listAlreadyOneItem);



        // début de la boucle de générations de tous les items
        for (int i = 0; i < numberObstacles && i < 50; i++)
        {
            List<float> position = new List<float>();

            bool positionCorrect = false; // Verifier si on peut le faire spawn
            while (!positionCorrect)
            {
                position.Clear();
                position.Add(Random.Range(-24, 25)); // X
                position.Add(Random.Range(-40, 41)); // Z
                


                //Check si c'est pas sur un point de spawn
                bool spawnPositionItem = checkPosition(listAlreadyItems, position, 10);

                if (spawnPositionItem)
                {
                    //check s'il n'y a pas deja un obstacle
                    bool canSpawn = checkPosition(positionList, position, 5);

                    if (canSpawn)
                    {
                        positionCorrect = true;
                    }
                }
            }

            // Ajouter à la liste des obstacles
            positionList.Add(position);


            if (!itemAlreadyThere)
            {
                //Instentier l'item
                GameObject itemCreated = null;

                int prefabGenerated = Random.Range(0, 5); // Quel prefab ?
                itemCreated = Instantiate(GetPrefab(prefabGenerated), new Vector3(positionList[i][0], 0, positionList[i][1]), Quaternion.Euler(0, Random.Range(0, 360), 0));
                itemCreated.tag = "Obstacle";
                itemCreated.name = "item" + i;
                itemCreated.transform.parent = obstacles.transform;
            }
            else
            {
                Transform itemMoving = obstacles.transform.Find ("item" + i);

                Vector3 newPos = new Vector3(positionList[i][0], 0, positionList[i][1]);


                Debug.Log("Position avant du item" + i + " " + itemMoving.position);
                Debug.Log("Nouvelle pos" +  positionList[i][0] + "; " + positionList[i][1]);
                itemMoving.position = newPos;
                Debug.Log("Position apres du item" + i + " " + itemMoving.position);
            }
        }




        GameObject GetPrefab(int prefabIndex)
        {
            switch (prefabIndex)
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

        bool checkPosition(List<List<float>> list, List<float> newList, int distance)
        {
            bool temp = true;

            foreach (List<float> item in list)
            {
                if ((newList[0] < item[0] + distance && newList[0] > item[0] - distance) && (newList[1] < item[1] + distance && newList[1] > item[1] - distance))
                {
                    temp = false;
                }
            }

            return temp;
        }
    }*/

    void GenerateRandomObstacle(bool itemAlreadyThere = false)
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
            ParticleSystem(ball.transform.position);
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
