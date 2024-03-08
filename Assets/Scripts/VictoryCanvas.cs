using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class WinTag : MonoBehaviour
{
    private TextMeshProUGUI victoryStatus;
    private TextMeshProUGUI winner;
    GameManager instance;

    GameObject player;
    GameObject player2;
    Animator animatorP1;
    Animator animatorP2;

    private void Awake()
    {
        instance = GameManager.GetInstance();
    }
    void Start()
    {
        victoryStatus = this.transform.Find("VictoryStatus").GetComponent<TextMeshProUGUI>();
        winner = this.transform.Find("Winner").GetComponent<TextMeshProUGUI>();



        // Faire vvenir les joueurs sur l'écran de fin
        Debug.Log(instance);
        instance.GeneratePlayer();

        player = GameObject.FindWithTag("Player");
        player2 = GameObject.FindWithTag("Player2");
        if (player2 == null)
        {
            player2 = GameObject.FindWithTag("AI");
        }

        Destroy(player.transform.Find("PlayerCamera").gameObject);
        Destroy(player2.transform.Find("PlayerCamera").gameObject);

        List<MonoBehaviour> listScripts = new();
        listScripts.Add(player.GetComponent("Player") as MonoBehaviour);
        listScripts.Add(player2.GetComponent("Player") as MonoBehaviour);
        listScripts.Add(player.GetComponent("PlayerMovement") as MonoBehaviour);
        listScripts.Add(player2.GetComponent("PlayerMovement") as MonoBehaviour);
        foreach (MonoBehaviour script in listScripts)
        {
            script.enabled = false;
        }

        player.transform.localScale = new Vector3(5,5,5);
        player2.transform.localScale = new Vector3(5,5,5);
        player.transform.position = new Vector3(505, 210, -320);
        player2.transform.position = new Vector3(515, 210, -320);
        player.transform.rotation = Quaternion.Euler(0, 180, 0);
        player2.transform.rotation = Quaternion.Euler(0, 180, 0);

        animatorP1 = player.GetComponent<Animator>();
        animatorP2 = player2.GetComponent<Animator>();



        if (winner != null)
        {
            winner.text = DetermineWinnerTag();
        }
        if (victoryStatus != null)
        {
            victoryStatus.text = DetermineVictoryStatus();
        }


    }

    private string DetermineWinnerTag()
    {
        if (instance.WinP1>instance.WinP2)
        {
            animatorP1.Play("wave");
            return "Joueur 1";
        }
        else if (instance.WinP2 > instance.WinP1)
        {
            animatorP2.Play("wave");
            return "Joueur 2";
        }
        else
        {
            return "";
        }
    }

    private string DetermineVictoryStatus()
    {
        if (instance.WinP1 != instance.WinP2)
        {
            return "Victoire !";
        }
        else
        {
            return "Match nul !";
        }
    }
}