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
    public float mistpsA = 0.1f;

    public bool buringtin = false;
    public bool buringpewter = false;
    public bool flaring = false;

    public float xSpeed;
    public float jumpStrength;
    private int key;

    public int tinbarpercent=500;
    public int pewterbarpercent=100;





    void Start()
    {
        isGrounded = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        key = 0;
        StartCoroutine(DoEverySecond());
        xSpeed = 3.5f;
        jumpStrength = 300;

    }

    IEnumerator DoEverySecond()
    {
        while (true)
        {

            if (buringtin && tinbarpercent>0)
            {
            if (flaring)
            {
            tinbarpercent -= 2;
            }
            else
            {
            tinbarpercent -= 1;
            }
            }
            if (buringpewter && pewterbarpercent>0)
            {
            if (flaring)
            {
            pewterbarpercent -= 2;
            }
            else
            {
            pewterbarpercent -= 1;
            }
            }
            yield return new WaitForSeconds(1f);  // wait 1 second
        }
    }

    void Update()
{
    if (Input.GetKeyDown(KeyCode.Z))
    {
        if (buringtin){
            mistpsA = 0.075f;
            buringtin = false;
            }
        else{
            mistpsA = 0.015f;
            buringtin = true;
            }
        
    }

    if (Input.GetKeyDown(KeyCode.X))
    {
        if (buringpewter){
            xSpeed = 3.5f;
            jumpStrength = 300;
            buringpewter = false;
            }
        else{
            xSpeed = 7;
            jumpStrength = 450;
            buringpewter = true;
            }
        
    }
        if (Input.GetKeyDown(KeyCode.F))
    {
        if (flaring){
            flaring = false;
            }
        else{
            flaring = true;
            }
        
    }

}


    void FixedUpdate()
    {


    
    if (flaring){
        if (buringtin){
            mistpsA = 0.002f;
            }
        if (buringpewter){
            xSpeed = 10;
            jumpStrength = 650;
            }            
        }
        else{
        if (buringtin){
            mistpsA = 0.015f;
            }
        if (buringpewter){
            xSpeed = 7;
            jumpStrength = 450;
            }
        }

    if (tinbarpercent < 1)
        {
        buringtin = false;  
        mistpsA = 0.075f; 
        }   
        if (pewterbarpercent < 1)
        {
        buringpewter = false;  
        xSpeed = 3.5f;
        jumpStrength = 300;
        }  

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

        var main = mistps.main; 
        Color c = main.startColor.color;
        c.a = mistpsA;
        main.startColor = new ParticleSystem.MinMaxGradient(c);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tin"))
        {
            Destroy(collision.gameObject);
            tinbarpercent += 250;
            if (tinbarpercent > 500)
            {
            tinbarpercent = 500;    
            }
        }
        if (collision.gameObject.CompareTag("Pewter"))
        {
            Destroy(collision.gameObject);
            pewterbarpercent += 50;
            if (pewterbarpercent > 100)
            {
            pewterbarpercent = 100;    
            }
        }
    }
}
