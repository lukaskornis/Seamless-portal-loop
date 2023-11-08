using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleDebug : MonoBehaviour
{
    void Start()
    {
        Vehicle.Player.onBump.AddListener( (obj) => print( $"Collided with {obj}") );
        Vehicle.Player.onFly.AddListener( (isFlying) => print( $"Flying {isFlying}"));
        Vehicle.Player.onLand.AddListener( () => print( $"Landed"));
        Vehicle.Player.onJump.AddListener( () => print( $"Jumped"));
        Vehicle.Player.onTurn.AddListener( (side) => print( $"Turned {side}"));
    }
}