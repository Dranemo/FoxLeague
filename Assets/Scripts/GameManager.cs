using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform ballPos;
    [SerializeField] private Transform playerPos;
    [SerializeField] private Transform player2Pos;


    [SerializeField] private GameObject birchTree;
    [SerializeField] private GameObject oakTree;
    [SerializeField] private GameObject rock01;
    [SerializeField] private GameObject rock02;
    [SerializeField] private GameObject rock03;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject ballPrefab;


    [SerializeField, Range(1,2)] private int playerNumber = 2;


    [SerializeField] private Transform cam1;
    [SerializeField] private Transform cam2;

    [SerializeField] public int score1;
    [SerializeField] public int score2;
    private static GameManager instance;
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
        GenerateTerrain();
        ResetPositions();
    }




    public void ResetPositions()
    {
        //Reset la balle
        GameObject ball = GameObject.FindWithTag("Ball");
        ball.transform.GetComponent<Rigidbody>().isKinematic = true;
        ball.transform.position = ballPos.transform.position;
        ball.transform.GetComponent<Rigidbody>().isKinematic = false;

        //Reset le joueur 1
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.GetComponent<Rigidbody>().isKinematic = true;

        player.transform.position = playerPos.transform.position;
        player.transform.LookAt(new Vector3(0, 0, 1));

        player.transform.GetComponent<Rigidbody>().isKinematic = false;


        //Reset le joueur 2 ou l'IA
        GameObject player2 = GameObject.FindWithTag("Player2");
        if(player2 == null)
        {
            player2 = GameObject.FindWithTag("AI");
        }
        player2.transform.GetComponent<Rigidbody>().isKinematic = true;

        player2.transform.position = player2Pos.transform.position;
        player2.transform.LookAt(new Vector3(0, 0, -1));

        player2.transform.GetComponent<Rigidbody>().isKinematic = false;

    }


    private void GenerateTerrain()
    {
        GeneratePlayer();
        GenerateBall();
        GenerateRandomObstacle();
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
                position.Add(Random.Range(-24, 25));
                position.Add(Random.Range(-24, 25));

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
            }



            Debug.Log(prefabGenerated);
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
    }

    private void GeneratePlayer()
    {
        GameObject playerGen = null;

        // Joueur 1
        playerGen = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        playerGen.name = "Player1";
        playerGen.layer = 6;
        playerGen.tag = "Player";

        playerGen.transform.Find("PlayerCamera").GetComponent<Camera>().cullingMask &= ~(1 << 6);
        playerGen.transform.Find("PlayerCamera").tag = "MainCamera";

        playerGen.transform.Find("SkinPlayer").gameObject.layer = 6;
        playerGen.transform.Find("ModelPlayer").gameObject.layer = 6;

        // Joueur 2
        playerGen = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        playerGen.layer = 7;

        Rect acutalRectPlayer2 = playerGen.transform.Find("PlayerCamera").GetComponent<Camera>().rect;

        playerGen.transform.Find("PlayerCamera").GetComponent<Camera>().cullingMask &= ~(1 << 7);
        playerGen.transform.Find("PlayerCamera").GetComponent<Camera>().rect = new Rect(0.5f, acutalRectPlayer2.y, acutalRectPlayer2.width, acutalRectPlayer2.height);
        playerGen.transform.Find("PlayerCamera").tag = "MainCamera";

        playerGen.transform.Find("SkinPlayer").gameObject.layer = 7;
        playerGen.transform.Find("ModelPlayer").gameObject.layer = 7;



        switch (playerNumber)
        {
            case 1:
                playerGen.name = "AI";
                playerGen.tag = "AI";
                break;


            case 2:
                playerGen.name = "Player2";
                playerGen.tag = "Player2";
                break;
        }
    }

    private void GenerateBall()
    {
        GameObject ballGen = null;

        ballGen = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
        ballGen.name = "Ball";
        ballGen.tag = "Ball";
    }
}
