using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public enum PlayerGoal { Player_1,Player_2 }
    [SerializeField] private PlayerGoal playerGoal;

    public void SetGoal(PlayerGoal _playerGoal)
    {
        playerGoal = _playerGoal;
    }

    private void OnTriggerEnter(Collider other)
    {

        //Debug.Log("1 uo 2 ???");
        if (other.CompareTag("Ball"))
        {
            if (playerGoal == PlayerGoal.Player_2)
            {
                StartCoroutine(ScoreManager.GetInstance().Goal(1, other.gameObject));
            }
            else if (playerGoal == PlayerGoal.Player_1)
            {
                StartCoroutine(ScoreManager.GetInstance().Goal(2, other.gameObject));
            }
        }

    }
}
