using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isGrounded;
    private Rigidbody2D rb;
    public float groundCheckDistance = 0.1f;
    
    private float maxVelX = 10;

    private Animator anim;

    public ParticleSystem mistps;
    public float mistpsA = 0.2f;

    public float xSpeed;
    public float jumpStrength;

    void Start()
    {
        isGrounded = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    void FixedUpdate()
    {


BoxCollider2D box = GetComponent<BoxCollider2D>();
Vector2 rayOrigin = (Vector2)transform.position + box.offset;

float halfWidth = box.size.x * 0.5f * transform.localScale.x;
float rayDistance = groundCheckDistance;
int groundLayer = LayerMask.GetMask("ground");


Vector2 leftEdge = rayOrigin + Vector2.left * halfWidth;
Vector2 center = rayOrigin;
Vector2 rightEdge = rayOrigin + Vector2.right * halfWidth;

RaycastHit2D leftRay = Physics2D.Raycast(leftEdge, Vector2.down, rayDistance, groundLayer);
RaycastHit2D centerRay = Physics2D.Raycast(center, Vector2.down, rayDistance, groundLayer);
RaycastHit2D rightRay = Physics2D.Raycast(rightEdge, Vector2.down, rayDistance, groundLayer);


Debug.DrawRay(leftEdge, Vector2.down * rayDistance, Color.red);
Debug.DrawRay(center, Vector2.down * rayDistance, Color.green);
Debug.DrawRay(rightEdge, Vector2.down * rayDistance, Color.blue);


isGrounded = leftRay.collider != null || centerRay.collider != null || rightRay.collider != null;




    if (Input.GetKeyDown(KeyCode.Z))
    {
        if (mistpsA == 0.2f){
            mistpsA = 0.05f;
            }
        else{
            mistpsA = 0.2f;
            }
        var main = mistps.main; 
        Color c = main.startColor.color;
        c.a = mistpsA;
        main.startColor = new ParticleSystem.MinMaxGradient(c);
    }


        float xHat = Input.GetAxisRaw("Horizontal");
        float vx = xHat * xSpeed;
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);

        //float horizontal = xHat;
        //if (Mathf.Abs(horizontal) < 0.01f) horizontal = 0f;
        //anim.SetFloat("Horizontal", horizontal);

        float yHat = new Vector2(0, Input.GetAxis("Vertical")).normalized.y;
        if (isGrounded && yHat == 1) {
            float vy = yHat * jumpStrength;
            isGrounded = false;
            rb.AddForce(transform.up * vy);


        }
        anim.SetFloat("ySpeed", rb.linearVelocity.y);
        anim.SetFloat("xSpeed", rb.linearVelocity.x);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
    }
}
