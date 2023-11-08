using NaughtyAttributes;
using Nomnom.RaycastVisualization;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent( typeof(Rigidbody) )]
public class Vehicle : MonoBehaviour
{
    [HideInInspector]public UnityEvent onJump = new();
    [HideInInspector]public UnityEvent onLand = new();
    [HideInInspector]public ChangeEvent<bool> onFly = new();
    [HideInInspector]public ChangeEvent<float> onTurn = new();
    [HideInInspector]public UnityEvent<GameObject> onBump = new();

    public float speed = 5;
    Vector3 velocity;
    [Header("Turning")]
    public float turnSpeed = 90;
    public float turnTime = 1;
    float currentTurnTime;
    float turnDirection;
    [CurveRange(0,0,1,1)]public AnimationCurve turnSpeedCurve;
    [Header("Jumping")]
    public float jumpHeight = 5;
    public float gravity = 9.8f;
    public Rigidbody rb { get; private set; }

    // JUMPING
    [SerializeField]float jumpMemory;
    [SerializeField]float jumpTimeout = 0.3f;
    float triedToJumpSecondsAgo = 999;
    float jumpedSecondsAgo;
    bool wannaJump;
    Collider collider;
    Bounds colliderBounds;
    public LayerMask jumpMask;
    public bool isGrounded;
    public bool isPlayer;
    public static Vehicle Player;

    void Awake()
    {
        if(isPlayer)Player = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity = Vector3.down * gravity;
        collider = GetComponent<Collider>();
        rb.detectCollisions = true;
    }

    void Update()
    {
        colliderBounds = collider.bounds;
        isGrounded = IsGrounded();
        onFly.Invoke(!isGrounded);

        // TRY TO JUMP
        triedToJumpSecondsAgo += Time.deltaTime;
    }

    void FixedUpdate()
    {
        // GRAVITY
        if (!isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0;
            rb.MovePosition( new Vector3(rb.position.x, colliderBounds.extents.y, rb.position.z));
        }

        // JUMP
        jumpedSecondsAgo += Time.deltaTime;
        var stillWannaJump = triedToJumpSecondsAgo <= jumpMemory;
        var canJump = jumpedSecondsAgo >= jumpTimeout;
        if (stillWannaJump && canJump && isGrounded)
        {
            Jump();
        }

        // MOVE
        var vel = transform.forward * speed;
        velocity.x = vel.x;
        velocity.z = vel.z;

        rb.MovePosition( rb.position + velocity * Time.deltaTime);

        // TURN
        currentTurnTime += Time.deltaTime;
        if(turnDirection == 0)currentTurnTime = 0;
        var turnSpeedNow = turnSpeedCurve.Evaluate(currentTurnTime / turnTime);
        rb.MoveRotation( rb.rotation * Quaternion.Euler(0,turnSpeedNow * turnDirection * turnSpeed * Time.deltaTime,0));
    }

    public void Turn(float side)
    {
        turnDirection = side;
        onTurn.Invoke(side);
    }

    public void TryJump()
    {
        triedToJumpSecondsAgo = 0;
    }

    void Jump()
    {
        var jumpSpeed = HeightToSpeed(jumpHeight);
        velocity.y += jumpSpeed;
        jumpedSecondsAgo = 0;
        onJump.Invoke();
    }

    float HeightToSpeed(float height)
    {
        return Mathf.Sqrt(2 * height * Physics.gravity.magnitude);
    }

    bool IsGrounded()
    {
        var ray = new Ray(transform.position, Vector3.down);
        return VisualPhysics.Raycast(ray, 0.51f, jumpMask);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            onLand.Invoke();
        }
        else
        {
            onBump.Invoke(other.gameObject);
        }
    }
}