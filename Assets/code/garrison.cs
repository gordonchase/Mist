using UnityEngine;
using System.Collections;
using System;

public class garrison : MonoBehaviour
{

    public GameObject enemyreward;
    public GameObject enemyreward1;
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
    public float enemyhelth=100;
    private bool takiningdamage = true;
    private bool dead=false;
    public bool last = false;
    public bool canattack = false;
    public System.Random randosando = new System.Random();
    private byte randothingy;

    
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
    Vector2 reltivtorotation = enemyobject.transform.InverseTransformPoint(playerPos); 
    float xposthingy = enemyobject.transform.position.x - playerobject.transform.position.x;
    float yposthingy = enemyobject.transform.position.y - playerobject.transform.position.y;
    if (player.delingdamage && takiningdamage)
        {
        if (xposthingy < 0.5f && xposthingy > -2f && !player.lastmove && yposthingy < 1.5f && yposthingy > -3f && anim.GetBool("attaking")){
            enemyhelth -= player.pewterdivby * 20;    
            Debug.Log("damage from kell" + enemyhelth);
            byte randothingy=(byte)(255-(player.pewterdivby * 50));
            StartCoroutine(takingdamage(randothingy));
            }
        if (xposthingy > 0f && xposthingy < 2.5f && player.lastmove && yposthingy < 1.5f && yposthingy > -3f && anim.GetBool("attaking")){
            enemyhelth -= player.pewterdivby * 20;   
            Debug.Log("damage from kell" + enemyhelth);
            byte randothingy=(byte)(255-(player.pewterdivby *50));
            StartCoroutine(takingdamage(randothingy));
            }
        takiningdamage = false;
        StartCoroutine(interferance());
        }
    if (enemyhelth<0)
    {
    StartCoroutine(dethco());
    }
    playerPos = playerobject.transform.position;
    enemyPos = enemyobject.transform.position;
    checkdistance = Vector2.Distance(playerPos, enemyPos);
    // TODO : fix! :
    float currentspeed = rb.linearVelocity.x;
    if (checkdistance < 2 && !attaking && !dead)
        {
        if (playerPos.x-enemyPos.x>0)
        {
        StartCoroutine(attakingco(true));
        }
        if (playerPos.x-enemyPos.x<0 && currentspeed<speedcap)
        {
        StartCoroutine(attakingco(false));
        }
        }
    else if (checkdistance<seedistance){
        if (playerPos.x-enemyPos.x>0 && currentspeed<speedcap && ! jumping)
        {
        rb.AddForce(Vector2.right * speed, ForceMode2D.Force);
        last=true;
        }
        if (playerPos.x-enemyPos.x<0 && currentspeed>-1*speedcap && ! jumping)
        {
        rb.AddForce(Vector2.left * speed, ForceMode2D.Force);
        last=false;
        }
        if (playerPos.y-enemyPos.y>1 && isgrounded && !jumping)
        {
        jumping = true;
        isgrounded = false;
        StartCoroutine(jumpdelay());
        }
    }
    if (!dead){
    anim.SetFloat("hor", Mathf.Abs(rb.linearVelocity.x));
    anim.SetBool("last", last);
    }
    }


    void OnCollisionEnter2D(Collision2D collision) 
    {

    if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("boxing")){
    Vector2 falldamage = collision.relativeVelocity;
    float damageonimpact = falldamage.magnitude;
    if (damageonimpact > 10f){
        enemyhelth -= damageonimpact*5;
        Debug.Log("damage from fall" + enemyhelth);
        byte randothingy=(byte)(225-(damageonimpact*5));
        StartCoroutine(takingdamage(randothingy));
    }
    }
    if (collision.gameObject.CompareTag("boxing")){
    Vector2 falldamage = collision.relativeVelocity;
    float damageonimpact = falldamage.magnitude;
    if (damageonimpact > 7.7f  && (collision.gameObject.transform.position.x-enemyPos.x>0==!last) || anim.GetBool("attaking")){
        int chink = randosando.Next(1,3);
        if (chink == 2){enemyhelth -= damageonimpact*4;}
        else{enemyhelth -= damageonimpact*2;}
        Debug.Log("damage from coin" + enemyhelth);
        if (chink == 2){randothingy=(byte)(255-(damageonimpact*4));}
        else{randothingy=(byte)(255-(damageonimpact*2));}
        StartCoroutine(takingdamage(randothingy));
    }
    else if (damageonimpact > 7.7f){Debug.Log("blooced boxing with sheild " + enemyhelth);}
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

    IEnumerator interferance()
    {
    yield return new WaitForSeconds(0.1f);
    player.delingdamage=false;
    takiningdamage = true;
    }
    IEnumerator jumpdelay()  
    {
    yield return new WaitForSeconds(0.5f); 
    rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
    yield return new WaitForSeconds(0.5f);
    jumping = false;
    }

    IEnumerator stopkill()  
    {
    yield return new WaitForSeconds(0.5f); 
    jumping = false;
    }
    IEnumerator attakingco(bool yes)  
    {
    attaking=true;
    anim.SetBool("attaking", true);
    yield return new WaitForSeconds(0.3f); 
    anim.SetBool("attaking", false);
    Vector2 reltivtorotation = enemyobject.transform.InverseTransformPoint(playerPos);
    if (yes && playerPos.x-enemyPos.x>0 && playerPos.x-enemyPos.x<3)
            {
            player.helth -= 20-player.pewterdivby;
            }
    if (!yes && playerPos.x-enemyPos.x<0 && playerPos.x-enemyPos.x>-3)
            {
            player.helth -= 20-player.pewterdivby;
            }
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
    Instantiate(enemyreward, enemyobject.transform.position, Quaternion.identity);
    Instantiate(enemyreward1, enemyobject.transform.position, Quaternion.identity);
    Destroy(enemyobject);
    }
}

