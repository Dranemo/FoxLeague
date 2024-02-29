using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        textScoreP1.text = "P2 : " + gameManager.score1.ToString();

        textScoreP2 = this.transform.Find("ScoreP2").GetComponent<TextMeshProUGUI>();
        textScoreP2.text = gameManager.score1.ToString() + " : P1";
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
                gameManager.score2++;
                textScoreP2.text = gameManager.score2.ToString() + " : P1";
                break;
            case 2:
                gameManager.score1++;
                textScoreP1.text = "P2 : " + gameManager.score1.ToString();
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