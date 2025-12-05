using UnityEngine;
using System.Collections;

public class sm_mistwrath : MonoBehaviour
{

    public PlayerController player;
    public GameObject playerobject;
    public GameObject enemyobject;
    public Rigidbody2D rb;
    public float checkdistance = 0;
    public int seedistance = 0;
    public int speed = 10;
    public int jump = 20;
    private Vector2 playerPos;
    private Vector2 enemyPos;
    private bool attaking = false;
    public bool isgrounded = true;
    private float currentspeed;
    public bool canmove = true;
    public float speedcap = 5;
    public Animator anim;
    public bool jumping=false;
    
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
    Vector2 playerPos = playerobject.transform.position;
    Vector2 enemyPos = enemyobject.transform.position;
    checkdistance = Vector2.Distance(playerPos, enemyPos);
    if (checkdistance < 2 && !attaking)
        {
         Vector2 reltivtorotation = enemyobject.transform.InverseTransformPoint(playerPos); 
        if (reltivtorotation.y > 0 && reltivtorotation.x < 0.2f && reltivtorotation.x > 0f)
            {
            StartCoroutine(attakingco());
            }

        else
        {
        enemyobject.transform.Rotate(0, 0, 1f);
        }
        }
    else if (checkdistance<seedistance){
        if (playerPos.x-enemyPos.x>0 && canmove && ! jumping)
        {
        rb.AddForce(Vector2.right * speed, ForceMode2D.Force);
        }
        if (playerPos.x-enemyPos.x<0 && canmove && ! jumping)
        {
        rb.AddForce(Vector2.left * speed, ForceMode2D.Force);
        }
        if (playerPos.y-enemyPos.y>1 && isgrounded)
        {
        enemyobject.transform.rotation = Quaternion.Euler(0, 0, 0);
        jumping = true;
        isgrounded = false;
        anim.SetBool("jumping", true);
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        StartCoroutine(jumpdelay());
        }
    }
    float currentspeed = rb.linearVelocity.x;
    if (Mathf.Abs(currentspeed)>speedcap){
        canmove = false;
    }
        else{
        canmove = true;
    }
    }


    void OnCollisionEnter2D(Collision2D collision) 
    {
    if (collision.gameObject.CompareTag("ground"))  
        {  
        isgrounded = true;
        }  
    }
    IEnumerator jumpdelay()  
    {
    yield return new WaitForSeconds(2f); 
    rb.constraints = RigidbodyConstraints2D.None;
    rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
    anim.SetBool("jumping", false);
    jumping = false;
    }
    IEnumerator attakingco()  
    {
    attaking=true;
    anim.SetBool("attaking", true);
    yield return new WaitForSeconds(0.5f); 
    attaking=false;
    anim.SetBool("attaking", false);
    Vector2 reltivtorotation = enemyobject.transform.InverseTransformPoint(playerPos); 
    // // if (reltivtorotation.y > 0 && reltivtorotation.x < 0)
    //     {
        player.helth -= 10;
        // }

    }
}

