using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FPSCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] Transform playerBody;
    [SerializeField] private float xRotationSensitivity;
    [SerializeField] private float yRotationSensitivity;
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;

    //Variables
    private float horizontalInput;
    private float verticalInput;
    private float verticalAngle;
    private float horizontalAngle;

    //Bools
    private bool canRotate;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        canRotate = true;
    }
    void Update()
    {
        MoveCamera();
    }
    /// <summary>
    /// Kamera hareketini kontrol eder.
    /// </summary>
    private void MoveCamera()
    {
        if(canRotate)
        {
            horizontalInput = Input.GetAxis("Mouse X") * Time.deltaTime * xRotationSensitivity;
            verticalInput = Input.GetAxis("Mouse Y") * Time.deltaTime * yRotationSensitivity;

            playerBody.Rotate(Vector3.up * horizontalInput);

            verticalAngle -= verticalInput;
            horizontalAngle += horizontalInput;

            verticalAngle = Mathf.Clamp(verticalAngle, minAngle, maxAngle);

            transform.rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0f);
        }
    }
}