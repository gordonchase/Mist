using UnityEngine;
using System.Collections;

public class sm_mistwrath : MonoBehaviour
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
        if (xposthingy < 0.5f && xposthingy > -1.5f && !player.lastmove && yposthingy < 1.5f && yposthingy > -3f){
            enemyhelth -= player.pewterdivby * 5;    
            Debug.Log("damage from kell" + enemyhelth);
            byte randothingy=(byte)(255-(player.pewterdivby * 50));
            StartCoroutine(takingdamage(randothingy));
            }
        if (xposthingy > 0f && xposthingy < 1.5f && player.lastmove && yposthingy < 1.5f && yposthingy > -3f){
            enemyhelth -= player.pewterdivby * 5;   
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
    if (checkdistance < 2 && !attaking && !dead)
        {
        if (reltivtorotation.y > 0 && reltivtorotation.x < 0.2f && reltivtorotation.x > 0f)
            {
            StartCoroutine(attakingco());
            }

        else
            {
            enemyobject.transform.Rotate(0, 0, 0.5f);
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
        if (playerPos.y-enemyPos.y>1 && isgrounded && !jumping)
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

    if (!collision.gameObject.CompareTag("Player")){
    Vector2 falldamage = collision.relativeVelocity;
    float damageonimpact = falldamage.magnitude;
    if (damageonimpact > 12.5f){
        enemyhelth -= damageonimpact;
        Debug.Log("damage from fall" + enemyhelth);
        byte randothingy=(byte)(225-(damageonimpact*5));
        StartCoroutine(takingdamage(randothingy));
    }
    }
    if (collision.gameObject.CompareTag("boxing")){
    Vector2 falldamage = collision.relativeVelocity;
    float damageonimpact = falldamage.magnitude;
    if (damageonimpact > 7.7f){
        enemyhelth -= damageonimpact*2;
        Debug.Log("damage from fall" + enemyhelth);
        byte randothingy=(byte)(255-(damageonimpact*5));
        StartCoroutine(takingdamage(randothingy));
    }
    }
    if (collision.gameObject.CompareTag("ground"))  
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
    // Debug.Log("finished attacing");
    if (reltivtorotation.y > 0 && reltivtorotation.x < 0.2f && reltivtorotation.x > 0f)
            {
                // Debug.Log("DAMAGE!");
            player.helth -= 10-player.pewterdivby;
            }

    }
    IEnumerator dethco()  
    {
    dead=true;
    rb.constraints = RigidbodyConstraints2D.FreezePositionX;
    rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    anim.SetBool("jumping", false);
    anim.SetBool("attaking", false);
    anim.SetBool("deth", true);
    yield return new WaitForSeconds(4.55f); 
    Instantiate(enemyreward, enemyobject.transform.position, Quaternion.identity);
    Instantiate(enemyreward1, enemyobject.transform.position, Quaternion.identity);
    Destroy(enemyobject);
    }
}


