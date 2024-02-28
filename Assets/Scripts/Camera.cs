using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Vector3 cameraBody;

    private GameObject ball;

    private float xRotationCamera = 0f;
    bool camOnBall = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ball = GameObject.FindWithTag("Ball");
    }

    void Update()
    {
        if (playerBody.CompareTag("Player") && !camOnBall)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotationCamera -= mouseY;
            xRotationCamera = Mathf.Clamp(xRotationCamera, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotationCamera, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }


        if (camOnBall)
        {
            // LookAt pour le joueur (rotation seulement autour de l'axe Y)
            this.transform.LookAt(ball.transform);

            // Copier la rotation autour de l'axe Y de la caméra vers le joueur en utilisant Time.deltaTime
            float newYRotation = Mathf.LerpAngle(playerBody.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.y, 5 * Time.deltaTime);
            playerBody.rotation = Quaternion.Euler(0f, newYRotation, 0f);
        }



        if (Input.GetButtonDown("CameraView"))
        {
            if(!camOnBall)
            {
                camOnBall = true;
            }
            else
            {
                camOnBall = false;
            }
        }





        /*if (camInput && CompareTag("MainCamera"))
        {
            GameObject player = GameObject.FindWithTag("Player");
            if ((transform.position-player.transform.position).magnitude < 2.1f)
            {
                transform.localPosition = cameraBody;
            }
            else
            {
                transform.localPosition = new Vector3(0,1f,0);
            }
        }*/


    }
}
