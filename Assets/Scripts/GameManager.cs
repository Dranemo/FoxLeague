using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform ballPos;
    [SerializeField] private Transform playerPos;
    [SerializeField] private Transform player2Pos;
    private static GameManager instance;
    public static GameManager GetInstance()
    {
        if (instance)
        {
            return instance;
        }
        else
        {
            return instance = FindObjectOfType<GameManager>();
        }
    }
    private void Start()
    {
        ResetPositions();
    }

    public void ResetPositions()
    {
        GameObject ball = GameObject.FindWithTag("Ball");
        ball.transform.position = ballPos.transform.position;

        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = playerPos.transform.position;
        
        GameObject player2 = GameObject.FindWithTag("Player2");
        player2.transform.position = player2Pos.transform.position;
        ball.transform.GetComponent<Rigidbody>().isKinematic = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
