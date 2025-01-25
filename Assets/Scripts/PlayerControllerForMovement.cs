using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerForMovement : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public float moveSpeed;
    public float xInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        rigidBody.velocity = new Vector2(xInput * moveSpeed, rigidBody.velocity.y);
    }
}
