using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform playerTransform;
    public float rotationSpeed = 1;

    // the vertical angle of the camera
    private float pitch = 0.0f;
    private float yaw = 0.0f;

    private Vector3 offset;
    private float currentCameraDistance;

    void Start()
    {
        offset = transform.position - playerTransform.position;
        currentCameraDistance = offset.magnitude;
    }

    void Update()
    {
        // Compute the yaw and pitch from mouse movement
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;

        // Limit the pitch of the camera to avoid flipping
        pitch = Mathf.Clamp(pitch, -80f, 80f);

        // Compute the rotation using the yaw and the pitch
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0.0f);

        // Compute the new camera position
        Vector3 newPos = playerTransform.position + rotation * offset;

        // Raycast from the player to the camera
        RaycastHit hit;
        if (Physics.Raycast(playerTransform.position, newPos - playerTransform.position, out hit, currentCameraDistance))
        {
            // If something is in the way, move the camera to the hit point
            newPos = hit.point;
        }

        // Update the position and rotation of the camera
        transform.position = newPos;
        transform.LookAt(playerTransform);
    }
}
