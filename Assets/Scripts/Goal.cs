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
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
