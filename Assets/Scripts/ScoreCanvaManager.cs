using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.CodeDom.Compiler;
using UnityEditor;

public class ScoreCanvaManager : MonoBehaviour
{
    private static ScoreCanvaManager instance;

    private TextMeshProUGUI textScoreP1;
    private TextMeshProUGUI textScoreP2;

    private TextMeshProUGUI textTime;

    [SerializeField] public float countdownTime = 120; // Temps en secondes
    public float currentTime;

    public bool timePause = false;



    void Awake()
    {
        textScoreP1 = this.transform.Find("ScoreP1").GetComponent<TextMeshProUGUI>();
        textScoreP2 = this.transform.Find("ScoreP2").GetComponent<TextMeshProUGUI>();

        textTime = this.transform.Find("Time").GetComponent<TextMeshProUGUI>();

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
        PauseUnpauseTime(false);
        WriteCanvaScore(0, 0);
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
            currentTime -= Time.deltaTime;

            if(currentTime <= 0)
            {
                GameManager.GetInstance().nextManche();
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
}