using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        try
        {
            GameObject gameManager = GameObject.FindObjectOfType<GameManager>().gameObject;
            if (gameManager != null)
            {
                Destroy(gameManager);
            }
        }
        catch (Exception e)
        {

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
