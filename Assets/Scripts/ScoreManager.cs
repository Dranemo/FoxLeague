using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.CodeDom.Compiler;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    private GameManager gameManager;
    private static ScoreManager instance;

    private TextMeshProUGUI textScoreP1;
    private TextMeshProUGUI textScoreP2;

    private int manche = 1;
    private int WinP1 = 0;
    private int WinP2 = 0;


    void Start()
    {
        gameManager = GameManager.GetInstance();

        textScoreP1 = this.transform.Find("ScoreP1").GetComponent<TextMeshProUGUI>();
        textScoreP1.text = gameManager.score1.ToString() + " : P1";

        textScoreP2 = this.transform.Find("ScoreP2").GetComponent<TextMeshProUGUI>();
        textScoreP2.text = "P2 : " + gameManager.score2.ToString();
    }



    public static ScoreManager GetInstance()
    {
        if (instance)
        {
            return instance;
        }
        else
        {
            return instance = FindObjectOfType<ScoreManager>();
        }
    }

    public void addScore(int playerId, GameObject ball)
    {
        switch (playerId)
        {
            case 1:
                gameManager.score1++;
                textScoreP1.text = gameManager.score1.ToString() + " : P1";
                if (gameManager.score1 >= 2)
                {
                    nextManche(playerId);
                }
                break;
            case 2:
                gameManager.score2++;
                textScoreP2.text = "P2 : " + gameManager.score2.ToString();
                if (gameManager.score2 >= 2)
                {
                    nextManche(playerId);
                }
                break;
        }
    }

    public IEnumerator Goal(int playerId, GameObject ball)
    {
        if (!ball.GetComponent<Ball>().isGoaled)
        {
            ball.GetComponent<Ball>().isGoaled = true;
            //Mettre tout en pause
            gameManager.allKinetic(true);



            addScore(playerId, ball);

            //
            for (float i = 1; i >= 0; i -= 0.025f)
            {
                ball.transform.localScale *= i;
                yield return new WaitForSeconds(.05f);
            }


            gameManager.ResetPositions();

        }
    }

    private void nextManche(int playerId)
    {
        manche++;
        gameManager.DeleteAllCreatedItem();

        gameManager.GenerateTerrain();

        gameManager.FindItems();

        gameManager.ResetPositions();

        if (playerId == 1)
        {
            WinP1++;
        }
        else if(playerId == 2)
        {
            WinP2++;
        }



        if(WinP1 == 2 || WinP2 == 2)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("Main_Menu");
        }
    }
}