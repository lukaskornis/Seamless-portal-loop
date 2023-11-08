using System.Linq;
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

        if (Input.GetButtonDown("Jump"))
        {
            vehicle.TryJump();
        }


        // if started touching left side of screen with one finger
        // and started touching right side of screen with another finger

        // if any finger is touching left side of screen
        // if touchscreen supported
        if (!Input.touchSupported) return;
        var left = Input.touches.Any(t => t.position.x < Screen.width / 2f);
        var right = Input.touches.Any(t => t.position.x > Screen.width / 2f);


        if (left && right)
        {
            vehicle.TryJump();
        }
        else if (left)
        {
            vehicle.Turn(-1);
        }
        else if (right)
        {
            vehicle.Turn(1);
        }
        else
        {
            vehicle.Turn(0);
        }
    }
}