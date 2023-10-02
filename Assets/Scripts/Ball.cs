using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody rb;
    public float moveSpeed = 5;
    public Transform startPipe;
    public Transform endPipe;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * moveSpeed;
    }

    void Update()
    {
        // accelerate forward to movespeed
        if (rb.velocity.magnitude < moveSpeed)
        {
            rb.AddForce(rb.velocity.normalized * moveSpeed, ForceMode.Acceleration);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "StartCollider")
        {
            // teleport to start pipe with same local pos, rot as  end pipe
            var localPos = endPipe.InverseTransformPoint(transform.position);
            var localRot = endPipe.InverseTransformDirection(transform.forward);
            var localVel = endPipe.InverseTransformDirection(rb.velocity);
            transform.position = startPipe.TransformPoint(localPos);
            transform.forward = startPipe.TransformDirection(localRot);
            rb.velocity = startPipe.TransformDirection(localVel);

        }
    }
}
