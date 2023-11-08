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
        if(Input.touchSupported)MobileControls();
    }

    public enum FingerState
    {
        None,
        Tap,
        Hold,
    }

    public float tapTime = 0.2f;
    float leftTouchDuration;
    float rightTouchDuration;

    void MobileControls()
    {
        bool couldJump = false;
        var leftTouch = Input.touches.FirstOrDefault(t => t.position.x < Screen.width / 2f);
        var rightTouch = Input.touches.FirstOrDefault(t => t.position.x > Screen.width / 2f);


        if(leftTouch.phase is TouchPhase.Stationary or TouchPhase.Moved)leftTouchDuration += Time.deltaTime;
        if (leftTouch.phase == TouchPhase.Ended || leftTouch.phase == TouchPhase.Canceled)
        {
            leftTouchDuration = 0;
            couldJump = true;
        }

        if(rightTouch.phase is TouchPhase.Stationary or TouchPhase.Moved)rightTouchDuration += Time.deltaTime;
        if (rightTouch.phase == TouchPhase.Ended || rightTouch.phase == TouchPhase.Canceled)
        {
            rightTouchDuration = 0;
            couldJump = true;
        }

        print( $"Could jump:{leftTouch.phase} Left: {leftTouchDuration} Right: {rightTouchDuration}");

        var combined = leftTouchDuration + rightTouchDuration;
        if(couldJump && combined > 0f && combined < tapTime*2)
        {
            vehicle.TryJump();
        }


        if (leftTouchDuration > rightTouchDuration)
        {
            // turn left
            if (leftTouchDuration > tapTime)
            {
                vehicle.Turn( -1 );
                if(couldJump)vehicle.TryJump();
            }
        }
        else
        {
            // turn right
            if (rightTouchDuration > tapTime)
            {
                vehicle.Turn( 1 );
                if(couldJump)vehicle.TryJump();
            }
        }
    }
}