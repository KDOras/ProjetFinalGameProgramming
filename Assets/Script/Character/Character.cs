using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Rigidbody rb;
    public float bulletSpeed = 20f;
    public float speed = 100f;
    public Camera PlayerCamera;
    public void Move(Vector3 velocity)
    {
        rb.MovePosition(rb.position + velocity * speed);
    }

    public void Jump(float jumpForce)
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
