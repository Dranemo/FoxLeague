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

    public int manche = 1;
    private int WinP1 = 0;
    private int WinP2 = 0;


    void Start()
    {
        gameManager = GameManager.GetInstance();

        ResetCanvaScore();
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

    public void ResetCanvaScore()
    {
        textScoreP1 = this.transform.Find("ScoreP1").GetComponent<TextMeshProUGUI>();
        textScoreP1.text = gameManager.score1.ToString() + " : P1";

        textScoreP2 = this.transform.Find("ScoreP2").GetComponent<TextMeshProUGUI>();
        textScoreP2.text = "P2 : " + gameManager.score2.ToString();
    }

    public void AddScore(int playerId, GameObject ball)
    {
        switch (playerId)
        {
            case 1:
                gameManager.score1++;
                textScoreP1.text = gameManager.score1.ToString() + " : P1";
                break;
            case 2:
                gameManager.score2++;
                textScoreP2.text = "P2 : " + gameManager.score2.ToString();
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



            AddScore(playerId, ball);

            //
            for (float i = 1; i >= 0; i -= 0.025f)
            {
                ball.transform.localScale *= i;
                yield return new WaitForSeconds(.05f);
            }

            Debug.Log("1 " + gameManager.score1 + " 2 " + gameManager.score2);
            if (gameManager.score2 >= 1)
            {
                nextManche(playerId);
            }
            else if (gameManager.score1 >= 1)
            {
                nextManche(playerId);
            }

            gameManager.ResetPositions();
        }
    }

    private void nextManche(int playerId)
    {
        manche++;
        gameManager.score1 = 0;
        gameManager.score2 = 0;

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

        else
        {
            gameManager.RepositionItems();
            ResetCanvaScore();
        }
    }

}