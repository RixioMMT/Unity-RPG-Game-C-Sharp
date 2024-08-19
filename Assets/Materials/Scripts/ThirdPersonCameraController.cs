using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    public Transform target; // Reference to the target (player)
    public Vector3 offset = new Vector3(0f,0f,0f); // Fixed offset from the target

    void LateUpdate()
    {
        // Directly set the camera's position to be the target's position plus the offset
        transform.position = target.position + offset;

        // Make the camera look at the target
        transform.LookAt(target);
    }
}