using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Vitesse de d�placement du joueur
    [SerializeField] private float angularSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = 0f;
        float verticalInput = 0f;

        // V�rifie quel joueur est en train de jouer et ajuste les entr�es en cons�quence
        if (gameObject.CompareTag("Player"))
        {
            horizontalInput = Input.GetAxis("Horizontal2");
            verticalInput = Input.GetAxis("Vertical2");
        }
        else
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }


        Vector3 movement = transform.position + transform.TransformDirection(Vector3.forward) * verticalInput * speed * Time.deltaTime;
        Vector3 rotation = transform.up * horizontalInput * angularSpeed * Time.deltaTime;

        
        Debug.Log(rotation);

        transform.Rotate(rotation);
        transform.position = movement;
    }
}


