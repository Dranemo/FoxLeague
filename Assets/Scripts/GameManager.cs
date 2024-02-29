using System.Collections;
using System.Collections.Generic;
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


    [SerializeField] private Transform cam1;
    [SerializeField] private Transform cam2;

    [SerializeField] public int score1 = 0;
    [SerializeField] public int score2 = 0;

    private static GameManager instance;

    private GameObject ball;
    private GameObject player;
    private GameObject player2;


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

        ball = FindObjectOfType<Ball>().gameObject;

        player = GameObject.FindWithTag("Player");
        player2 = GameObject.FindWithTag("Player2");
        if (player2 == null)
        {
            player2 = GameObject.FindWithTag("AI");
        }


        ResetPositions();
    }




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
    }

    public void allKinetic(bool booleen)
    {
        ball.transform.GetComponent<Rigidbody>().isKinematic = booleen;
        player.transform.GetComponent<Rigidbody>().isKinematic = booleen;
        player2.transform.GetComponent<Rigidbody>().isKinematic = booleen;
    }


    private void GenerateTerrain()
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
        GameObject obstacles = new GameObject("Obstacles");



        List<List<int>> positionList = new List<List<int>>();

        for (int i = 0; i < 10; i++)
        {
            int prefabGenerated = Random.Range(0, 5); // Quel prefab ?


            List<int> position = new List<int>();

            bool positionCorrect = false; // Verifier si on peut le faire spawn
            while (!positionCorrect)
            {
                position.Clear();
                position.Add(Random.Range(-24, 25)); // X
                position.Add(Random.Range(-35, 36)); // Z


                // Verif que c'est pas dans un truc qui spawn
                bool PositionOnTheWay = false;

                if ((position[0] < ballPos.position.x + 5 && position[0] > ballPos.position.x - 5) && (position[1] < ballPos.position.y + 5 && position[1] > ballPos.position.y - 5))
                {
                    PositionOnTheWay = true;
                    positionCorrect = true;
                }
                else if((position[0] < playerPos.position.x + 5 && position[0] > playerPos.position.x - 5) && (position[1] < playerPos.position.y + 5 && position[1] > playerPos.position.y - 5))
                {
                    PositionOnTheWay = true;
                    positionCorrect = true;
                }
                else if ((position[0] < player2Pos.position.x + 5 && position[0] > player2Pos.position.x - 5) && (position[1] < player2Pos.position.y + 5 && position[1] > player2Pos.position.y - 5))
                {
                    PositionOnTheWay = true;
                    positionCorrect = true;
                }



                if (!PositionOnTheWay)
                {
                    if (positionList.Count > 0)
                    {
                        bool positionCheckedGood = true; // Vérifier s'il n'y a pas d'objet à sa place ou dans un range
                        foreach (List<int> positionAlr in positionList)
                        {
                            if ((position[0] < positionAlr[0] + 5 && position[0] > positionAlr[0] - 5) && (position[1] < positionAlr[1] + 5 && position[1] > positionAlr[1] - 5))
                            {
                                positionCheckedGood = false;
                            }
                        }

                        if (positionCheckedGood)
                        {
                            positionCorrect = true;
                        }
                    }
                    else
                    {
                        positionCorrect = true;
                    }



                    positionList.Add(position);


                    GameObject itemCreated = null;
                    switch (prefabGenerated)
                    {
                        case 0: // Birch tree
                            itemCreated = Instantiate(birchTree, new Vector3(positionList[i][0], 0, positionList[i][1]), Quaternion.Euler(0, Random.Range(0, 360), 0));
                            break;
                        case 1: // Oak tree
                            itemCreated = Instantiate(oakTree, new Vector3(positionList[i][0], 0, positionList[i][1]), Quaternion.Euler(0, Random.Range(0, 360), 0));
                            break;
                        case 2: // Rock01
                            itemCreated = Instantiate(rock01, new Vector3(positionList[i][0], 0, positionList[i][1]), Quaternion.Euler(0, Random.Range(0, 360), 0));
                            break;
                        case 3: // Rock02
                            itemCreated = Instantiate(rock02, new Vector3(positionList[i][0], 0, positionList[i][1]), Quaternion.Euler(0, Random.Range(0, 360), 0));
                            break;
                        case 4: // Rock03
                            itemCreated = Instantiate(rock03, new Vector3(positionList[i][0], 0, positionList[i][1]), Quaternion.Euler(0, Random.Range(0, 360), 0));
                            break;
                    }

                    itemCreated.name = "item" + i;
                    itemCreated.transform.parent = obstacles.transform;
                }


                Debug.Log(i);
            }
            
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

        goalGen = Instantiate(goalPrefab, goal1Pos.position, Quaternion.identity);
        goalGen.name = "Goal1";
        goalGen.GetComponent<Goal>().SetGoal(Goal.PlayerGoal.Player_1);

        goalGen = Instantiate(goalPrefab, goal2Pos.position, Quaternion.identity);
        goalGen.name = "Goal2";
        goalGen.GetComponent<Goal>().SetGoal(Goal.PlayerGoal.Player_2);
    }
}
