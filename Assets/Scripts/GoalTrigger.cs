using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Goal;

public class GoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (transform.parent.GetComponent<Goal>().GetGoal() == PlayerGoal.Player_2) 
            {
                StartCoroutine(ScoreManager.GetInstance().Goal(1, other.gameObject));
            }
            else if (transform.parent.GetComponent<Goal>().GetGoal() == PlayerGoal.Player_1)
            {
                StartCoroutine(ScoreManager.GetInstance().Goal(2, other.gameObject));
            }
        }

    }
}
