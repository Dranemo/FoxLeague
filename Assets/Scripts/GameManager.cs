using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField] public Material redMat;
    [SerializeField] public Material blueMat;


    [SerializeField, Range(1,2)] public int playerNumber = 2;
    public int playerLoaded = 0;
    [SerializeField] private int numberObstacles = 10;
    [SerializeField] private int spread = 5;
    [SerializeField] private int minDistance = 3;


    [SerializeField] private Transform cam1;
    [SerializeField] private Transform cam2;
    private GameObject cameraBallGoal;
    private GameObject canva;


    private static GameManager instance;

    public GameObject ball;
    public GameObject player;
    public GameObject player2;
    public GameObject obstacles;

    public GameObject particle;

    ScoreCanvaManager scoreCanvaManager;
    [SerializeField] public Sprite spriteEmptyCircle;
    [SerializeField] public Sprite spriteFullCircleRed;
    [SerializeField] public Sprite spriteFullCircleBlue;

    [SerializeField] public int score1 = 0;
    [SerializeField] public int score2 = 0;

    public int WinP1 = 0;
    public int WinP2 = 0;
    public bool equality = false;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
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

        ballPos = scene.transform.Find("Positions").Find("BallStartPosition");
        playerPos = scene.transform.Find("Positions").Find("PlayerStartPosition");
        player2Pos = scene.transform.Find("Positions").Find("Player2StartPosition");
        goal1Pos = scene.transform.Find("Positions").Find("GoalStartPosition");
        goal2Pos = scene.transform.Find("Positions").Find("Goal2StartPosition");

        cameraBallGoal = scene.transform.Find("CameraGoal").gameObject;
        particle = scene.transform.Find("ParticleSystem").gameObject;

        GenerateTerrain();

        scoreCanvaManager = ScoreCanvaManager.GetInstance();

        FindItems();

        canva = FindObjectOfType<Canvas>().gameObject;


        scoreCanvaManager.PauseUnpauseTime(true);
        AllKinematic(true);

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
        player.GetComponent<PlayerMovement>().SetBoost(100);

        //Reset le joueur 2 ou l'IA
        player2.transform.position = player2Pos.transform.position;
        player2.transform.LookAt(new Vector3(0, 0, -1));
        player2.GetComponent<PlayerMovement>().SetBoost(100);

        //Reset camera et particules
        particle.transform.position = Vector3.down * 40;
        cameraBallGoal.transform.position = Vector3.down * 40; 

        Rect tempRect = cameraBallGoal.GetComponent<Camera>().rect; // Deplacer le render
        tempRect.x = 1f;
        cameraBallGoal.GetComponent<Camera>().rect = tempRect;


        // Reset éléments sur le canva
        canva.transform.Find("Minimap").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -75);
        canva.transform.Find("BorderTime").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -170);
        canva.transform.Find("ManchesCircle").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -210);

        StartCoroutine(scoreCanvaManager.WriteStartCanva());
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

    public void AllKinematic(bool booleen)
    {
        ball.transform.GetComponent<Rigidbody>().isKinematic = booleen;
        player.transform.GetComponent<Rigidbody>().isKinematic = booleen;
        player2.transform.GetComponent<Rigidbody>().isKinematic = booleen;
    }


    private void CamGoal(Goal.PlayerGoal playerGoal)
    {
        if(playerGoal == Goal.PlayerGoal.Player_1)
        {
            cameraBallGoal.transform.position = goal1Pos.position + new Vector3(10, 10, 20);
        }
        else
        {
            cameraBallGoal.transform.position = goal2Pos.position + new Vector3(-10, 10, -20);
        }

        cameraBallGoal.transform.LookAt(ball.transform.position);
        Rect tempRect = cameraBallGoal.GetComponent<Camera>().rect; // Deplacer le render
        tempRect.x = 0f;
        cameraBallGoal.GetComponent<Camera>().rect = tempRect;

        canva.transform.Find("Minimap").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 200);
        canva.transform.Find("BorderTime").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -20);
        canva.transform.Find("ManchesCircle").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -60);
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

    }


    void GenerateRandomObstacle(bool replaceObstacles = false)
    {
        List<Vector3> position = new();
        List<GameObject> obstaclesList = new();

        List<GameObject> obstacleListAlrThere = new();

        if (!replaceObstacles)
        {
            GameObject obstacles_ = new GameObject("Obstacles");
            obstacles = obstacles_;
        }
        else
        {
            foreach(Transform obstacle in obstacles.transform)
            {
                obstacleListAlrThere.Add(obstacle.gameObject);
            }
        }

        obstaclesList.Add(ballPos.gameObject);
        obstaclesList.Add(player2Pos.gameObject);
        obstaclesList.Add(playerPos.gameObject);
        obstaclesList.Add(goal1Pos.gameObject);
        obstaclesList.Add(goal2Pos.gameObject);

        GameObject item = null;
        int itemMovedIndex = 0;

        Vector3 positionInitiale = new Vector3(-20, 0, -40);
        if (!replaceObstacles)
        {
            item = CreateNewItem(position.Count, positionInitiale);
            item.transform.parent = obstacles.transform;

            numberObstacles = Random.Range(30, 50);
        }
        else

        {
            numberObstacles = obstacleListAlrThere.Count;
            itemMovedIndex = Random.Range(0, obstacleListAlrThere.Count);

            item = obstacleListAlrThere[itemMovedIndex];
            obstacleListAlrThere.RemoveAt(itemMovedIndex);
            item.transform.position = positionInitiale;
        }
        obstaclesList.Add(item);
        position.Add(positionInitiale);



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
                if (newPos.x > 20 || newPos.x < -20 || newPos.z > 40 || newPos.z < -40)
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
                    if (!replaceObstacles)
                    {

                        item = CreateNewItem(position.Count, newPos);

                        item.transform.parent = obstacles.transform;
                    }
                    else
                    {
                        itemMovedIndex = Random.Range(0, obstacleListAlrThere.Count);

                        item = obstacleListAlrThere[itemMovedIndex];
                        obstacleListAlrThere.RemoveAt(itemMovedIndex);
                        item.transform.position = newPos;
                    }
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


    public void ReplaceRandomObstacle()
    {
        GenerateRandomObstacle(true);
    }


    // --------------------------- SCORES --------------------------- //
    public void AddScore(Goal.PlayerGoal playerGoal)
    {
        switch (playerGoal)
        {
            case Goal.PlayerGoal.Player_2:
                score1++;
                break;
            case Goal.PlayerGoal.Player_1:
                score2++;
                break;
            default:
                break;
        }
        
        scoreCanvaManager.WriteCanvaScore(score1, score2);
    }

    public IEnumerator GoalDone(Goal.PlayerGoal playerGoal)
    {
        if (!ball.GetComponent<Ball>().isGoaled)
        {

            scoreCanvaManager.PauseUnpauseTime(true);
            AllKinematic(true);


            ball.GetComponent<Ball>().isGoaled = true;

            // Effets
            CamGoal(playerGoal);


            AddScore(playerGoal);

            //
            for (float i = 1; i >= 0; i -= 0.025f)
            {
                ball.transform.localScale *= i;
                yield return new WaitForSeconds(.05f);
            }


            if (equality)
            {
                NextManche();
            }
            else
            {
                ResetPositions();
            }

            if (playerGoal == Goal.PlayerGoal.endManche)
            {
                equality = true;
            }
        }
    }

    public void NextManche()
    {

        AllKinematic(true);

        if (score1 == score2)
        {
            StartCoroutine(GoalDone(Goal.PlayerGoal.endManche));
            scoreCanvaManager.currentTime = 0;
        }
        else
        {
            StartCoroutine(NextMancheCoroutine());
        }
    }

    private IEnumerator NextMancheCoroutine()
    {
        if (score1 > score2)
        {
            WinP1++;
            scoreCanvaManager.AddMancheWin(Player.PlayerEnum.player1, WinP1);
            scoreCanvaManager.WriteCanvaNextManche(Player.PlayerEnum.player1);
        }
        else if (score1 < score2)
        {
            WinP2++;
            scoreCanvaManager.AddMancheWin(Player.PlayerEnum.player2, WinP2);
            scoreCanvaManager.WriteCanvaNextManche(Player.PlayerEnum.player2);
        }


        score1 = 0;
        score2 = 0;

        yield return new WaitForSeconds(3);


        if (WinP1 >= 2 || WinP2 >= 2)
        {
            EndGame();
        }

        else
        {
            ReplaceRandomObstacle();
            equality = false;
            scoreCanvaManager.ResetCanva();
            ResetPositions();
        }
    }

    public void EndGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Victory_Screen");
    }
}
