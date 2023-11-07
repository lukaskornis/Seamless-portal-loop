using Nomnom.RaycastVisualization;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent( typeof(Rigidbody) )]
public class Vehicle : MonoBehaviour
{
    public static UnityEvent onJump = new();
    public static UnityEvent onLand = new();
    public static UnityEvent<bool> onFly = new();
    public static UnityEvent<float> onTurn = new();
    public static UnityEvent<GameObject> onBump = new();

    public float speed = 5;
    public float turnSpeed = 90;
    [Header("Jumping")]
    public float jumpHeight = 5;
    public float gravity = 9.8f;
    Rigidbody rb;

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


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity = Vector3.down * gravity;
        collider = GetComponent<Collider>();
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
        rb.velocity = new Vector3(vel.x, rb.velocity.y, vel.z);
    }

    public void Turn(float side)
    {
        rb.MoveRotation( rb.rotation * Quaternion.Euler(0,turnSpeed * side * Time.deltaTime,0));
        onTurn.Invoke(side);
    }

    public void TryJump()
    {
        triedToJumpSecondsAgo = 0;
    }

    void Jump()
    {
        var jumpSpeed = HeightToSpeed(jumpHeight);
        rb.velocity += Vector3.up * jumpSpeed;
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