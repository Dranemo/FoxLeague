using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void DestroyGameObject()
    {
        Destroy(GameObject.FindWithTag("Ball"));
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            Debug.Log("goal !");
            other.transform.GetComponent<Rigidbody>().isKinematic = true;


            GameManager.GetInstance().ResetPositions();

            if (CompareTag("Goal1"))
            {
                GameManager.GetInstance().score1 += 1;
            }
            else if (CompareTag("Goal2"))
            {
                GameManager.GetInstance().score2 += 1;
            }
        }
    }
}
