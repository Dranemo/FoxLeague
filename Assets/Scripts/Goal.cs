using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Ball"))
        {
            if (CompareTag("Goal1"))
            {
                StartCoroutine(ScoreManager.GetInstance().Goal(1, other.gameObject));
            }
            else if (CompareTag("Goal2"))
            {
                StartCoroutine(ScoreManager.GetInstance().Goal(2, other.gameObject));
            }
        }
    }
}
