using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
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


    [SerializeField, Range(1,2)] private int playerNumber = 2;
    [SerializeField] private int numberObstacles = 10;


    [SerializeField] private Transform cam1;
    [SerializeField] private Transform cam2;

    [SerializeField] public int score1 = 0;
    [SerializeField] public int score2 = 0;

    private static GameManager instance;

    private GameObject ball;
    private GameObject player;
    private GameObject player2;
    private GameObject goal1;
    private GameObject goal2;
    private GameObject obstacles;
    private GameObject canva;


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

        Debug.Log(goal1Pos.position + " " + goal2Pos.position);


        GenerateTerrain();

        FindItems();

        ResetPositions();
    }


    private void Awake()
    {
        // Trouver le GameObject "Cube" à partir de l'objet actuel (ce script est attaché à un GameObject)
        GameObject cubeTransform = GameObject.Find("Scene/WallZ");

        // Vérifier si le GameObject "Cube" a été trouvé
        if (cubeTransform != null)
        {
            // Accéder au composant MeshRenderer du GameObject "Cube"
            MeshRenderer cubeRenderer = cubeTransform.GetComponent<MeshRenderer>();

            // Vérifier si le composant MeshRenderer a été trouvé
            if (cubeRenderer != null)
            {
                // Accéder à la taille du Bounds du MeshRenderer
                Vector3 size = cubeRenderer.bounds.size;

                // Afficher la taille dans la console
                Debug.Log("Taille du Cube : " + size);
            }
            else
            {
                Debug.LogError("Le composant MeshRenderer n'a pas été trouvé sur le Cube.");
            }
        }
        else
        {
            Debug.LogError("Le GameObject 'Cube' n'a pas été trouvé sous 'GOAL'.");
        }
    }

    public void ResetPositions()
    {
        //Reset la balle
        Debug.Log(ball);

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

        Debug.Log("reseted");
    }

    public void DeleteAllCreatedItem()
    {
        GameObject.Destroy(player);
        GameObject.Destroy(player2);
        GameObject.Destroy(ball);
        GameObject.Destroy(goal1);
        GameObject.Destroy(goal2);
        GameObject.Destroy(obstacles);
        GameObject.Destroy(canva);
    }

    public void FindItems()
    {
        player = GameObject.FindWithTag("Player");
        player2 = GameObject.FindWithTag("Player2");
        if (player2 == null)
        {
            player2 = GameObject.FindWithTag("AI");
        }
        ball = GameObject.FindObjectOfType<Ball>().gameObject;

        obstacles = GameObject.Find("Obstacles");

        obstacles = GameObject.Find("Goal1");
        obstacles = GameObject.Find("Goal2");

        canva = GameObject.FindObjectOfType<Canvas>().gameObject;
    }

    public void allKinetic(bool booleen)
    {
        ball.transform.GetComponent<Rigidbody>().isKinematic = booleen;
        player.transform.GetComponent<Rigidbody>().isKinematic = booleen;
        player2.transform.GetComponent<Rigidbody>().isKinematic = booleen;
    }






    public void GenerateTerrain()
    {
        GenerateGoal();
        GeneratePlayer();
        GenerateBall();
        GenerateRandomObstacle();
        GenerateOverlay();
    }

    private void GenerateRandomObstacle()
    {
        //instancier un gameobject vide
        GameObject obstacles_ = new GameObject("Obstacles");

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

                if(spawnPositionItem)
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


            //Instentier l'item
            GameObject itemCreated = null;

            int prefabGenerated = Random.Range(0, 5); // Quel prefab ?
            itemCreated = Instantiate(GetPrefab(prefabGenerated), new Vector3(positionList[i][0], 0, positionList[i][1]), Quaternion.Euler(0, Random.Range(0, 360), 0));
            itemCreated.tag = "Obstacle";
            itemCreated.name = "item" + i;
            itemCreated.transform.parent = obstacles_.transform;
        }

        Debug.Log(positionList.Count);





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
    }
    private void GeneratePlayer()
    {
        GameObject playerGen = null;

        // Joueur 1
        playerGen = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        playerGen.GetComponent<Player>().SetPlayerEnum(Player.PlayerEnum.player1);
        playerGen.tag = "Player";

        // Joueur 2
        playerGen = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        switch (playerNumber)
        {
            case 1:
                playerGen.GetComponent<Player>().SetPlayerEnum(Player.PlayerEnum.player2);
                playerGen.tag = "Player2";
                break;


            case 2:
                playerGen.GetComponent<Player>().SetPlayerEnum(Player.PlayerEnum.AI);
                playerGen.tag = "AI";
                break;
        }
    }

    private void GenerateBall()
    {
        GameObject ballGen = null;

        ballGen = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
        ballGen.name = "Ball";
    }

    private void GenerateOverlay()
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

    private void GenerateGoal()
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
