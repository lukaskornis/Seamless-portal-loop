using UnityEngine;

public class SlideVisuals : MonoBehaviour
{
    public GameObject landParticles;
    public AudioClip jumpSound;
    public AudioClip bumpSound;
    public AudioSource slideSound;
    public AudioSource carveSound;
    public ParticleSystem slideParticles;
    public ParticleSystem carveParticles;
    public TrailRenderer trail;

    public float leanAngle = 10;
    float forwardAngle = 0;
    public float targetLeanAngle;
    public Transform model;

    void Start()
    {
        Vehicle.Player.onJump.AddListener(OnJump);
        Vehicle.Player.onLand.AddListener(OnLand);
        Vehicle.Player.onTurn.AddListener(OnTurn);
        Vehicle.Player.onBump.AddListener(OnBump);
        Vehicle.Player.onFly.AddListener(OnFly);
        Controls.onTurn.AddListener(OnTurnControls);
    }

    void OnTurnControls(int side)
    {
        targetLeanAngle = -side * leanAngle;
    }

    void Update()
    {
        var angle = Mathf.LerpAngle(model.localEulerAngles.z, targetLeanAngle, Time.deltaTime * 10);
        forwardAngle = Mathf.MoveTowards(forwardAngle, 0,30 * Time.deltaTime);
        model.localEulerAngles = new Vector3(forwardAngle,0,angle);
    }

    void OnTurn(float side)
    {
        targetLeanAngle = -side * leanAngle;

        if (side != 0f)
        {
            // rotating
            carveSound.volume = 0.5f;
            slideSound.volume = 0;
            carveParticles.Emit(true);
            var shape = carveParticles.shape;
            shape.rotation = new Vector3(-47,-side * 90,0);
        }
        else
        {
            // not rotating
            carveSound.volume = 0;
            slideSound.volume = 1;
            carveParticles.Emit(false);
        }
    }

    void OnLand()
    {
        forwardAngle = 0;
        Instantiate(landParticles, transform.position, transform.rotation);
    }

    void OnJump()
    {
        forwardAngle = -30;
        jumpSound.Play();
    }

    void OnFly(bool isFlying)
    {
        if(!isFlying)
        {
            slideParticles.Emit( true);
            slideSound.UnPause();
            trail.emitting = true;
        }
        else
        {
            slideParticles.Emit( false);
            slideSound.Pause();
            trail.emitting = false;
        }
    }

    void OnBump(GameObject obj)
    {
        bumpSound.Play();
    }
}