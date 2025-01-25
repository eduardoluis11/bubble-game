using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerForMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    private float xInput;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        rigidBody.velocity = new Vector2(xInput * moveSpeed, rigidBody.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        }
    }
}
