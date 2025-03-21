using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreCanvaManager : MonoBehaviour
{
    private static ScoreCanvaManager instance;

    private TextMeshProUGUI textScoreP1;
    private TextMeshProUGUI textScoreP2;

    private TextMeshProUGUI textCenter;

    private List<GameObject> manchesCircles;

    GameManager gameManager;



    private TextMeshProUGUI textTime;

    [SerializeField] public float countdownTime = 120; // Temps en secondes
    public float currentTime;

    private bool timePause = false;



    void Awake()
    {
        gameManager = GameManager.GetInstance();

        textScoreP1 = this.transform.Find("BorderScore1").Find("ScoreP1").GetComponent<TextMeshProUGUI>();
        textScoreP2 = this.transform.Find("BorderScore2").Find("ScoreP2").GetComponent<TextMeshProUGUI>();

        textTime = this.transform.Find("BorderTime").Find("Time").GetComponent<TextMeshProUGUI>();
        textCenter = transform.Find("BigTextCenter").GetComponent<TextMeshProUGUI>();

        manchesCircles = new List<GameObject>();
        manchesCircles.Add(transform.Find("ManchesCircle").Find("Circle1").gameObject);
        manchesCircles.Add(transform.Find("ManchesCircle").Find("Circle2").gameObject);
        manchesCircles.Add(transform.Find("ManchesCircle").Find("Circle3").gameObject);

        currentTime = countdownTime;
        ResetCanva();
    }

    public static ScoreCanvaManager GetInstance()
    {
        if (instance)
        {
            return instance;
        }
        else
        {
            return instance = FindObjectOfType<ScoreCanvaManager>();
        }
    }

    public void ResetCanva()
    {
        WriteCanvaScore(0, 0);
        WriteCanvaTime(currentTime);
        textCenter.text = "";
        currentTime = countdownTime;
    }

    public void WriteCanvaScore(int scoreP1, int scoreP2)
    {
        textScoreP1.text = scoreP1.ToString() + " : P1";
        textScoreP2.text = "P2 : " + scoreP2.ToString();
    }

    private void WriteCanvaTime(float time)
    {
        int minutes, secondes;

        minutes = Mathf.FloorToInt(time / 60);
        secondes = Mathf.FloorToInt(time);
        secondes -= minutes * 60;

        string minutesStr = "";
        string secondesStr = "";

        for(int i = secondes.ToString().Length; i < 2; i++)
        {
            secondesStr += "0";
        }
        for (int i = minutes.ToString().Length; i < 2; i++)
        {
            minutesStr += " ";
        }

        secondesStr += secondes.ToString();
        minutesStr += minutes.ToString();

        

        textTime.text = minutesStr + ":" + secondesStr;

    }

    private void Update()
    {
        if (!timePause)
        {
            if (gameManager.equality)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                currentTime -= Time.deltaTime;
            }

            if (currentTime <= 0)
            {
                GameManager.GetInstance().NextManche();
            }
            else
            {
                WriteCanvaTime(currentTime);
            }
        }
    }

    public void PauseUnpauseTime(bool pause)
    {
        timePause = pause;
    }

    public void AddMancheWin(Player.PlayerEnum player, int WinPlayer)
    {
        switch (player)
        {
            case Player.PlayerEnum.player1:
                if(WinPlayer == 1)
                {
                    manchesCircles[0].GetComponent<Image>().sprite = gameManager.spriteFullCircleBlue;
                }
                else
                {
                    manchesCircles[1].GetComponent<Image>().sprite = gameManager.spriteFullCircleBlue;
                }
                break;
            case Player.PlayerEnum.player2:
                if (WinPlayer == 1)
                {
                    manchesCircles[2].GetComponent<Image>().sprite = gameManager.spriteFullCircleRed;
                }
                else
                {
                    manchesCircles[1].GetComponent<Image>().sprite = gameManager.spriteFullCircleRed;
                }
                break;
        }
    }

    public void WriteCanvaNextManche(Player.PlayerEnum playerEnum, bool overtime = false)
    {
        string text = "";
        if (!overtime)
        {
            switch (playerEnum)
            {
                case Player.PlayerEnum.player1:
                    text += "Joueur 1";
                    break;
                case Player.PlayerEnum.player2:
                    text += "Joueur 2";
                    break;
                case Player.PlayerEnum.AI:
                    text += "AI";
                    break;
            }

            text += " a gagn� la manche !";
        }
        else
        {
            text = "Overtime";
        }

        textCenter.text = text;
    }

    public IEnumerator WriteStartCanva()
    {
        WriteCanvaTime(currentTime);

        for (int i = 3; i > 0; i--)
        {
            textCenter.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        textCenter.text = "";


        gameManager.AllKinematic(false);
        PauseUnpauseTime(false);
    }
}