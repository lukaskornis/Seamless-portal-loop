using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    Vehicle vehicle;

    void Start()
    {
        vehicle = GetComponent<Vehicle>();
    }

    void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");
        vehicle.Turn(h);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            vehicle.TryJump();
        }
    }
}