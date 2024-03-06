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

    public PlayerGoal GetGoal()
    {
        return playerGoal;
    }
}
