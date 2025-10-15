using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    
    [Header("Smoothing Settings")]
    [SerializeField] private float smoothSpeed = 2f; // Overall smoothing speed
    [SerializeField] private float yAxisSmoothSpeed = 1.5f; // Specific Y-axis smoothing (slower for more smoothness)
    [SerializeField] private bool useIndependentYSmoothing = true; // Enable separate Y-axis smoothing
    
    [Header("Optional: Deadzone Settings")]
    [SerializeField] private bool useDeadzone = false;
    [SerializeField] private float yDeadzone = 0.5f; // Camera won't move if Y difference is less than this
    
    void Start()
    {
        offset = transform.position - player.transform.position;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
    
    void LateUpdate() // Use LateUpdate for camera movement to avoid jitter
    {
        if (PlayerLive.alive == 1) 
        {
            Vector3 targetPosition = player.transform.position + offset;
            
            if (useIndependentYSmoothing)
            {
                // Smooth Y-axis independently for more control
                Vector3 currentPos = transform.position;
                
                // Handle Y-axis with its own smoothing
                float targetY = targetPosition.y;
                if (useDeadzone)
                {
                    float yDifference = Mathf.Abs(currentPos.y - targetY);
                    if (yDifference > yDeadzone)
                    {
                        currentPos.y = Mathf.Lerp(currentPos.y, targetY, yAxisSmoothSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    currentPos.y = Mathf.Lerp(currentPos.y, targetY, yAxisSmoothSpeed * Time.deltaTime);
                }
                
                // Handle X and Z with normal smoothing
                currentPos.x = Mathf.Lerp(currentPos.x, targetPosition.x, smoothSpeed * Time.deltaTime);
                currentPos.z = Mathf.Lerp(currentPos.z, targetPosition.z, smoothSpeed * Time.deltaTime);
                
                transform.position = currentPos;
            }
            else
            {
                // Simple uniform smoothing for all axes
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            }
        }
    }
}