using DG.Tweening;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    void Start()
    {
        Vehicle.Player.onLand.AddListener(Shake);
        Vehicle.Player.onBump.AddListener(_ => ShakeBig());
    }

    public void Shake()
    {
        transform.DOShakeRotation(0.3f, 3);
    }

    public void ShakeBig()
    {
        transform.DOShakeRotation( 0.4f, 5);
    }
}