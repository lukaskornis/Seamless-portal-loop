using UnityEngine;

[RequireComponent( typeof(Rigidbody) )]
public class Vehicle : MonoBehaviour
{
    public float speed = 5;
    public float turnSpeed = 90;
    public float jumpHeight = 5;
    public float gravity = 9.8f;
    Rigidbody rb;

    // JUMPING
    [SerializeField]float jumpMemory;
    [SerializeField]float jumpTimeout = 0.3f;
    float triedToJumpSecondsAgo = 999;
    float jumpedSecondsAgo;
    bool wannaJump;
    float rayDistance;
    Bounds colliderBounds;
    public LayerMask jumpMask;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity = Vector3.down * gravity;
        colliderBounds = GetComponent<Collider>().bounds;
        rayDistance = colliderBounds.extents.y + 0.01f;
    }

    void Update()
    {
        // TRY TO JUMP
        triedToJumpSecondsAgo += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            triedToJumpSecondsAgo = 0;
        }

        // JUMP
        jumpedSecondsAgo += Time.deltaTime;
        var stillWannaJump = triedToJumpSecondsAgo <= jumpMemory;
        var canJump = jumpedSecondsAgo >= jumpTimeout;
        if (stillWannaJump && canJump && IsGrounded())
        {
            Jump();
        }

        // MOVE
        var vel = transform.forward * speed;
        rb.velocity = new Vector3(vel.x, rb.velocity.y, vel.z);

        // TURN
        var h = Input.GetAxis("Horizontal");
        transform.Rotate(0,turnSpeed * h * Time.deltaTime,0);
    }

    void Jump()
    {
        var jumpSpeed = HeightToSpeed(jumpHeight);
        rb.velocity += Vector3.up * jumpSpeed;
        jumpedSecondsAgo = 0;
        print("jumping with speed " + jumpSpeed);
    }

    float HeightToSpeed(float height)
    {
        return Mathf.Sqrt(2 * height * Physics.gravity.magnitude);
    }

    bool IsGrounded()
    {
        return Physics.BoxCast(
            colliderBounds.center,
            colliderBounds.extents,
            Vector3.down,
            Quaternion.identity,
            rayDistance,
            jumpMask
        );
    }
}