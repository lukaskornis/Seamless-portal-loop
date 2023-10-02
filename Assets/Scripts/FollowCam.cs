using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public float distance;
    Vector3 targetPos;
    Vector3 targetRot;
    [Range(0f, 1f)] public float smoothness = 0.9f;

    void LateUpdate()
    {
        targetPos = target.position;

        // rotate to target rb velocity
        var rb = target.GetComponent<Rigidbody>();
        targetRot = rb.velocity.normalized;

        targetPos -= transform.forward * distance;
        targetPos += Vector3.up * distance / 2;




        // if distance to target is greater than 5 , instantly move to target
        if (Vector3.Distance(transform.position, targetPos) > 5)
        {
            transform.position = targetPos;
            transform.rotation = Quaternion.LookRotation(targetRot);
            print("instant");
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, 1 - 0.1f);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetRot), 1 - 0.1f);
        }

    }
}
