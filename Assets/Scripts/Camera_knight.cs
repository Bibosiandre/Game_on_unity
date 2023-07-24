using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        if (!player)
            player = FindObjectOfType<Knight>().transform;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = player.position;
        targetPosition.z = transform.position.z;
        targetPosition.y += 1.5f;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}