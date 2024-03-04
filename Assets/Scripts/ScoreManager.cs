using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    private GameManager gameManager;
    private static ScoreManager instance;

    private TextMeshProUGUI textScoreP1;
    private TextMeshProUGUI textScoreP2;


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
                if (gameManager.score1>=2)
                {
                    Cursor.lockState = CursorLockMode.None;
                    SceneManager.LoadScene("Main_Menu");
                    Cursor.visible = true;
                }
                break;
            case 2:
                gameManager.score2++;
                textScoreP2.text = "P2 : " + gameManager.score2.ToString();
                if (gameManager.score2 >= 2)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    SceneManager.LoadScene("Main_Menu");
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
}