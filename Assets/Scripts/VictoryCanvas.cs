using TMPro;
using UnityEngine;

public class WinTag : MonoBehaviour
{
    private TextMeshProUGUI victoryStatus;
    private TextMeshProUGUI winner;
    GameManager instance;
    private void Awake()
    {
        instance = GameManager.GetInstance();
        Debug.Log(instance);
    }
    void Start()
    {
        victoryStatus = this.transform.Find("VictoryStatus").GetComponent<TextMeshProUGUI>();
        winner = this.transform.Find("Winner").GetComponent<TextMeshProUGUI>();

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
            return "Joueur 1";
        }
        else if (instance.WinP2 > instance.WinP1)
        {
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