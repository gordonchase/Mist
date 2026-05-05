using UnityEngine;
using System.Collections;
using System;

public class tineye : MonoBehaviour
{

    public GameObject enemyreward;
    public GameObject enemyreward1;
    public PlayerController player;
    public GameObject playerobject;
    public GameObject enemyobject;
    public Rigidbody2D rb;
    private float checkdistance = 0;
    public int seedistance = 0;
    public int speed = 10;
    public int jump = 6;
    private Vector2 playerPos;
    private Vector2 enemyPos;
    private bool attaking = false;
    public bool isgrounded = true;
    private float currentspeed;
    public bool canmove = true;
    public float speedcap = 5;
    public Animator anim;
    public bool jumping=false;
    public float enemyhelth=20;
    private bool takiningdamage = true;
    private bool dead=false;
    public bool last = false;
    public bool canattack = false;
    public System.Random randosando = new System.Random();
    private byte randothingy;
    private bool canspawn=true; 
    private bool waytomove = true; 
    public bool emmiter=false;
    private bool leftRay;
    private bool rightRay;
    private bool wallray1;
    private bool wallray2;


    
    void Start()
    {
    Vector2 playerPos = playerobject.transform.position;
    Vector2 enemyPos = enemyobject.transform.position;
    rb = enemyobject.GetComponent<Rigidbody2D>();
    checkdistance = Vector2.Distance(playerPos, enemyPos);
    anim = GetComponent<Animator>();
    }
    void Update()
    {


    CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
    Vector2 rayOrigin = (Vector2)transform.position + capsule.offset;
    float halfWidth = capsule.size.x * 0.6f * transform.localScale.x;
    int groundLayer = LayerMask.GetMask("ground");
    float halfHeight = capsule.size.y * 0.5f * transform.localScale.y;

    Vector2 leftEdge = rayOrigin + Vector2.left * halfWidth;
    Vector2 rightEdge = rayOrigin + Vector2.right * halfWidth;

    RaycastHit2D leftfall = Physics2D.Raycast(leftEdge, Vector2.down, 6f, groundLayer);
    RaycastHit2D rightfall = Physics2D.Raycast(rightEdge, Vector2.down, 6f, groundLayer);
    Debug.DrawRay(leftEdge, Vector2.down * 7f, new Color32(0, 128, 255, 255));
    Debug.DrawRay(rightEdge, Vector2.down * 7f, new Color32(0, 128, 255, 255));

    float groundOffset = halfHeight - (halfHeight/2); 

    Vector2 lowerLeft = leftEdge + Vector2.down * groundOffset;
    Vector2 lowerRight = rightEdge + Vector2.down * groundOffset;

    RaycastHit2D wallrayr = Physics2D.Raycast(lowerLeft, Vector2.left, -0.3f, groundLayer);    
    RaycastHit2D wallrayl = Physics2D.Raycast(lowerRight, Vector2.right, 0.3f, groundLayer); 


    leftRay = leftfall.collider != null;
    rightRay = rightfall.collider != null;
    wallray1 = wallrayr.collider != null;
    wallray2 = wallrayl.collider != null;




        if (playerPos.x-enemyPos.x>0)
        {
            last=true;
        }
        else if (playerPos.x-enemyPos.x<0)
        {
            last=false;
        }
        playerPos = playerobject.transform.position;
        enemyPos = enemyobject.transform.position;
        checkdistance = Vector2.Distance(playerPos, enemyPos);

        anim.SetFloat("hor", rb.linearVelocity.x);
        anim.SetBool("last", last);


    
    Vector2 reltivtorotation = enemyobject.transform.InverseTransformPoint(playerPos); 
    float xposthingy = enemyobject.transform.position.x - playerobject.transform.position.x;
    float yposthingy = enemyobject.transform.position.y - playerobject.transform.position.y;
    if (player.delingdamage && takiningdamage)
        {
            enemyhelth -= player.pewterdivby * 20;    
            takiningdamage = false;
            StartCoroutine(interference());
        }
    if (enemyhelth<0 && !dead)
    {
    StartCoroutine(dethco());
    }

    float currentspeed = rb.linearVelocity.x;
    // if (checkdistance < 1.5 && !attaking && !dead)
    //     {
    //     if (playerPos.x-enemyPos.x>0)
    //     {
    //     StartCoroutine(attakingco());
    //     }
    //     if (playerPos.x-enemyPos.x<0)
    //     {
    //     StartCoroutine(attakingco());
    //     }
    //     }
    if (checkdistance<seedistance && !dead){
        emmiter = true;
    }

    if (checkdistance>seedistance && !dead){ 
    }
    
    
    }


    void FixedUpdate()
    {
        if (checkdistance<seedistance && !dead){

        if (playerPos.x-enemyPos.x>0 && currentspeed<speedcap && !wallray1)
        {

        if (leftRay){
            rb.AddForce(Vector2.left * speed * Time.deltaTime *100 , ForceMode2D.Force);
        }
        if (!leftRay)
            {
            rb.AddForce(Vector2.right * 1.5f * Time.deltaTime *100 , ForceMode2D.Force);       
            }
        }
        if (playerPos.x-enemyPos.x<0 && currentspeed>-1*speedcap && ! jumping && !wallray2)
        {

        if (rightRay){
            rb.AddForce(Vector2.right * speed * Time.deltaTime *100 , ForceMode2D.Force);

        }
        if (!rightRay)
            {
            rb.AddForce(Vector2.left * 1.5f * Time.deltaTime *100 , ForceMode2D.Force);   
  
            }
        }
        if (wallray1 && isgrounded && playerPos.x-enemyPos.x>0)
        {
        isgrounded = false;
        rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse); 
            if (playerPos.x - enemyPos.x > 0)
                {
                rb.AddForce(Vector2.right * 1f* Time.deltaTime *100 , ForceMode2D.Impulse);
                }
            if (playerPos.x - enemyPos.x < 0)
                {
                rb.AddForce(Vector2.left * 1f* Time.deltaTime *100 , ForceMode2D.Impulse);
                }
        }
        if (wallray2 && isgrounded && playerPos.x-enemyPos.x<0)
        {
        isgrounded = false;
        rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            if (playerPos.x - enemyPos.x > 0)
                {
                rb.AddForce(Vector2.right * 1f* Time.deltaTime *100 , ForceMode2D.Impulse);
                }
            if (playerPos.x - enemyPos.x < 0)
                {
                rb.AddForce(Vector2.left * 1f* Time.deltaTime *100 , ForceMode2D.Impulse);
                }
        }
    } 
    }



    void OnCollisionEnter2D(Collision2D collision) 
    {

    if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("boxing")){
    Vector2 falldamage = collision.relativeVelocity;
    float damageonimpact = falldamage.magnitude;
    if (damageonimpact > 20f){
        enemyhelth -= damageonimpact*5;
        byte randothingy=(byte)(225-(damageonimpact*15));
        StartCoroutine(takingdamage(randothingy));
    }
    }
    if (collision.gameObject.CompareTag("boxing")){
    Vector2 falldamage = collision.relativeVelocity;
    float damageonimpact = falldamage.magnitude;
    if (damageonimpact > 7f){
        enemyhelth -= damageonimpact*5;
        randothingy=(byte)(255-(damageonimpact*5));
        StartCoroutine(takingdamage(randothingy));
    }
    }
    if (collision.gameObject.CompareTag("ground")&& !jumping)  
        {  
        isgrounded = true;
        }  
    }
    IEnumerator takingdamage(byte red)
    {
        enemyobject.GetComponent<SpriteRenderer>().color = new Color32(255, red, red, 255);
        yield return new WaitForSeconds(0.2f);
        enemyobject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }

    IEnumerator interference()
    {
    yield return new WaitForSeconds(0.1f);
    player.delingdamage=false;
    takiningdamage = true;
    }


    IEnumerator attakingco()  
    {
    attaking=true;
    anim.SetBool("attaking", true);
    yield return new WaitForSeconds(0.3f); 
    anim.SetBool("attaking", false);
    Vector2 reltivtorotation = enemyobject.transform.InverseTransformPoint(playerPos);
    yield return new WaitForSeconds(2f); 
    attaking=false;

    }
    IEnumerator dethco()  
    {
    dead=true;
    rb.constraints = RigidbodyConstraints2D.FreezePositionX;
    rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    anim.SetFloat("hor", 0f);
    anim.SetBool("last", false);
    anim.SetBool("attacking", false);
    anim.SetBool("death", true);
    yield return new WaitForSeconds(2.25f); 
        if (canspawn){
    Instantiate(enemyreward, enemyobject.transform.position, Quaternion.identity);
    Instantiate(enemyreward1, enemyobject.transform.position, Quaternion.identity);
    canspawn = false;
    }
    Destroy(enemyobject);
    }
}


