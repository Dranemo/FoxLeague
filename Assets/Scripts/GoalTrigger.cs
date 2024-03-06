using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Goal;

public class GoalTrigger : MonoBehaviour
{
    public AudioClip goalSound;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = goalSound;
    }
    private void OnTriggerEnter(Collider other)
    {
        audioSource.Play();
        if (other.CompareTag("Ball"))
        {
            if (transform.parent.GetComponent<Goal>().GetGoal() == PlayerGoal.Player_2) 
            {
                StartCoroutine(GameManager.GetInstance().GoalDone(1));
            }
            else if (transform.parent.GetComponent<Goal>().GetGoal() == PlayerGoal.Player_1)
            {
                StartCoroutine(GameManager.GetInstance().GoalDone(2));
            }
        }

    }
}
