using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isGrounded;
    private Rigidbody2D rb;
    public float groundCheckDistance = 0.1f;
    
    private float maxVelX = 10;

    public float xSpeed;
    public float jumpStrength;

    void Start()
    {
        isGrounded = false;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
    RaycastHit2D ground = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance);

    if (ground && ground.collider.Gameobject.CompareTag("ground")) {
        isGrounded = true;
    } else {
        isGrounded = false;
    }

        float xHat = Input.GetAxisRaw("Horizontal");
        float vx = xHat * xSpeed;
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);

        float yHat = new Vector2(0, Input.GetAxis("Vertical")).normalized.y;
        if (isGrounded && yHat == 1) {
            float vy = yHat * jumpStrength;
            isGrounded = false;
            rb.AddForce(transform.up * vy);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
    }
}
