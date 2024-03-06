using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public enum PlayerGoal { Player_1,Player_2 }
    [SerializeField] private PlayerGoal playerGoal;

    [SerializeField] Material redMat;
    [SerializeField] Material blueMat;

    public void SetGoal(PlayerGoal _playerGoal)
    {
        playerGoal = _playerGoal;
    }

    public PlayerGoal GetGoal()
    {
        return playerGoal;
    }

    private void Start()
    {
        GameObject barres = transform.Find("GOAL").Find("Cube").gameObject;

        if(playerGoal == PlayerGoal.Player_2)
        {
            barres.GetComponent<SkinnedMeshRenderer>().material = redMat;
        }
        else
        {
            barres.GetComponent<SkinnedMeshRenderer>().material = blueMat;
        }
    }
}
