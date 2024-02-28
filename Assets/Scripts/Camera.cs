using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Vector3 cameraBody;

    private float xRotationCamera = 0f;
    bool camInput = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (playerBody.CompareTag("Player"))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotationCamera -= mouseY;
            xRotationCamera = Mathf.Clamp(xRotationCamera, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotationCamera, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }


        camInput = Input.GetButtonDown("CameraView");
        if (camInput && CompareTag("MainCamera"))
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
        }
    }
}
