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

        PlayerGoal goal = transform.parent.GetComponent<Goal>().GetGoal();
        

        if (other.CompareTag("Ball"))
        {
            audioSource.Play();
            if (goal == PlayerGoal.Player_2) 
            {
                StartCoroutine(GameManager.GetInstance().GoalDone(goal));
            }
            else if (goal == PlayerGoal.Player_1)
            {
                StartCoroutine(GameManager.GetInstance().GoalDone(goal));
            }
        }

    }
}
