using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This lets the player Move andd Jump.
 * 
 * Source of part of this code: Alex Dev from
 * https://www.udemy.com/share/1095fO3@MrVZ1eq21fecOb_xzjFOwlDfpSbpHd5dtC5WOnvUAx4NlqA4DjGeZEEj9j1vIP2b/
 * I used his following videos: "7. Example of using rigidbody and collider", 
 * "8. First script ,input and movement", "9. Jump of a charcater", "10. Collision detection".
 */
public class PlayerControllerForMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    private float xInput;

    public float groundCheckRadius;
    public Transform groundCheck;
    public bool groundDetected;
    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CollisionChecks();

        xInput = Input.GetAxisRaw("Horizontal");
        Movement();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void CollisionChecks()
    {
        groundDetected = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    private void Jump()
    {
        if(groundDetected)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
    }

    private void Movement()
    {
        rigidBody.velocity = new Vector2(xInput * moveSpeed, rigidBody.velocity.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

}
