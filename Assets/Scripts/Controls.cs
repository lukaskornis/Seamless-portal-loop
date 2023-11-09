using System;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class Controls : MonoBehaviour
{
    Vehicle vehicle;
    public static ChangeEvent<int> onTurn = new();

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


        MobileControls();
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
    int leftTicks;
    int rightTicks;
    TouchPhase lastLeftPhase;
    TouchPhase lastRightPhase;

    void MobileControls()
    {
        bool wannaJump = false;
        var leftTouch = Input.touches.FirstOrDefault(t => t.position.x < Screen.width / 2f);
        var rightTouch = Input.touches.FirstOrDefault(t => t.position.x > Screen.width / 2f);

        if (leftTouch.phase is TouchPhase.Stationary or TouchPhase.Moved)
        {
            leftTouchDuration += Time.deltaTime;
            leftTicks++;
        }
        if (leftTouch.phase == TouchPhase.Ended || leftTouch.phase == TouchPhase.Canceled)
        {
            leftTouchDuration = 0;
            leftTicks = 0;
            wannaJump = true;
        }


        if (rightTouch.phase is TouchPhase.Stationary or TouchPhase.Moved)
        {
            rightTouchDuration += Time.deltaTime;
            rightTicks++;
        }
        if (rightTouch.phase == TouchPhase.Ended || rightTouch.phase == TouchPhase.Canceled)
        {
            rightTouchDuration = 0;
            rightTicks = 0;
            wannaJump = true;
        }



        var leftJustPressed = leftTicks == 1;
        var rightJustPressed = rightTicks == 1;

        var leftTap = leftTouchDuration > 0f &&  leftTouchDuration < tapTime;
        var rightTap = rightTouchDuration > 0f && rightTouchDuration < tapTime;
        var leftHold = leftTouchDuration >= tapTime;
        var rightHold = rightTouchDuration >= tapTime;

        if(leftTicks > 0)onTurn.Invoke(-1);
        else if(rightTicks > 0)onTurn.Invoke(1);


        if((leftTap && rightTap && (leftJustPressed || rightJustPressed)))
        {
            vehicle.TryJump();
            print("Touch delta : " + (leftTouchDuration - rightTouchDuration));
        }

        if (leftHold)
        {
            if(wannaJump) vehicle.TryJump();

            if (rightHold && rightTouchDuration < leftTouchDuration)
            {
                vehicle.Turn(1);
            }
            else
            {
                vehicle.Turn(-1);
            }
        }
        else if (rightHold)
        {
            if (wannaJump) vehicle.TryJump();
            vehicle.Turn(1);
        }
    }
}