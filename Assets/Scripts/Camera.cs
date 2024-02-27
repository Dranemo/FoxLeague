using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform playerBody;

    private float xRotationCamera = 0f;

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
    }
}
