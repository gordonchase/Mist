using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{

    public int helth = 100;
    private bool thymething1 = false;

    private bool isGrounded;  
    private Rigidbody2D rb;  
    public float groundCheckDistance = 0.1f;  

    private float maxVelX = 10;  

    private Animator anim;  

    public ParticleSystem mistps;  
    public float mistpsA = 0.1f;  

    public Tilemap notreal;  
    public float notrealA = 1.0f;  

    public bool buringtin = false;  
    public bool buringpewter = false;  
    public bool buringsteel = false;  
    public bool buringiron = false;  
    public bool flaring = false;  

    public float xSpeed;  
    public float jumpStrength;  
    private int key;  

    public int tinbarpercent=500;  
    public int pewterbarpercent=100;  
    public int steelbarpercent=200;  
    public int ironbarpercent=200;  

    public Image pweterhelth;
    private float newAlphaValue = 0.0f;

    public bool canattk=true;
    public bool lastmove=true;
    public bool delingdamage=false;

    public int pewterdivby = 1;
    public int numboxings=0;
    public GameObject boxingfab;
    private float xOffsetAmount=0f;
    public bool superanoyingjumpthing = false;

    public int pullForce = 0;
    public int pushForce = 0;
    public float slowMotionScale = 1;
    public float metalDetectRange = 30;
    public LayerMask meatelayer;
    private Collider2D[] metalsinarea = null;
    public bool spacetoogle=false;
    private Dictionary<GameObject, Vector2[]> objectLineMap = new Dictionary<GameObject, Vector2[]>();



    void Start()  
    {  
        isGrounded = false;  
        rb = GetComponent<Rigidbody2D>();  
        anim = GetComponent<Animator>();  
        key = 0;  
        StartCoroutine(DoEverySecond());  
        xSpeed = 3.5f;  
        jumpStrength = 6.1f;  
        notrealA = 1.0f;  
    }  


    IEnumerator attaking()
    {
    anim.SetBool("slicing", true);
    delingdamage=true;
    yield return new WaitForSeconds(0.4f);  
    anim.SetBool("slicing", false);
    yield return new WaitForSeconds(0.2f);
    canattk = true;
    }

    IEnumerator DoEverySecond()  
    {  
        while (true)  
        {  
            if (buringtin && tinbarpercent > 0)  
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

            if (buringpewter && pewterbarpercent > 0)  
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

            if (buringsteel && steelbarpercent > 0)  
            {  
                if (flaring)  
                {  
                    steelbarpercent -= 2;  
                }  
                else  
                {  
                    steelbarpercent -= 1;  
                }  
            }  

            if (buringiron && ironbarpercent > 0)  
            {  
                if (flaring)  
                {  
                    ironbarpercent -= 2;  
                }  
                else  
                {  
                    ironbarpercent -= 1;  
                }  
            }  

            thymething1 = true;

            yield return new WaitForSeconds(1f);  
        }  
    }  

    void Update()  
    {  
        if (Input.GetKeyDown(KeyCode.Z))  
        {  
            if (buringtin)  
            {  
                mistpsA = 0.075f;  
                buringtin = false;  
                notrealA = 1.0f;  
            }  
            else  
            {  
                mistpsA = 0.015f;  
                buringtin = true;  
                notrealA = 1.0f;  
            }  
        } 
        if (Input.GetKeyDown(KeyCode.S) && canattk)  
        {  
            canattk = false;
            StartCoroutine(attaking());
        }  

        if (Input.GetKeyDown(KeyCode.X))  
        {  
            if (buringpewter)  
            {  
                pewterdivby =1;
                xSpeed = 3.5f;  
                jumpStrength = 6.1f;  
                buringpewter = false;  
            }  
            else  
            {  
                xSpeed = 7;  
                jumpStrength = 10;  
                buringpewter = true;  
            }  
        }  

        if (Input.GetKeyDown(KeyCode.C))  
        {  
            if (buringsteel)  
            {  
                buringsteel = false;   
            }  
            else  
            {  
                buringsteel = true;  
            }  
        }  

        if (Input.GetKeyDown(KeyCode.V))  
        {  
            if (buringiron)  
            {  
                buringiron = false;  
            }  
            else  
            {  
                buringiron = true;    
            }  
        }  

        if (Input.GetKeyDown(KeyCode.F))  
        {  
            if (flaring)  
            {  
                flaring = false;  
            }  
            else  
            {  
                flaring = true;  
            }  
        }  
        if (Input.GetKeyDown(KeyCode.Space))  
        {  
        spacetoogle = !spacetoogle;
            if (spacetoogle){Time.timeScale = slowMotionScale;}
            else{Time.timeScale = 1.0f;}
        }  

        if (Input.GetKeyDown(KeyCode.E))  
        {  
            if (numboxings>0)  
            {  
            numboxings-=1;
                if (lastmove){xOffsetAmount=1f;}
                else{xOffsetAmount=-1f;}
            Instantiate(boxingfab, transform.position + new Vector3(xOffsetAmount, 0, 0), Quaternion.identity);
            }
        }  
        if (Input.GetKeyDown(KeyCode.Q))  
        {  
            if (numboxings>0)  
            {  
            numboxings-=1;
                if (lastmove){xOffsetAmount=1f;}
                else{xOffsetAmount=-1f;}
            buringsteel = true;
            }
        } 
        if (Input.GetKeyUp(KeyCode.W)){superanoyingjumpthing = false;}
        if (Input.GetKeyDown(KeyCode.W)&&isGrounded)
        {
        rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.R))  
        {  
        }
    }  


    void FixedUpdate()  
    {


        if (helth>100){helth=100;}

        if (flaring)  
        {  
            if (buringtin)  
            {  
                mistpsA = 0.002f;  
                notrealA = 0.3f;  
            }  
            if (buringpewter)  
            {  
                pewterdivby =3;
                xSpeed = 10;  
                jumpStrength = 13;  
            }  
            if (buringsteel)  
            {  
                pushForce = 3500;
            } 
            if (buringiron)  
            {  
                pullForce = 4000;
            } 
            metalDetectRange = 30;
            slowMotionScale = 0.05f;
        }  
        else  
        {  
            if (buringtin)  
            {  
                mistpsA = 0.015f;  
                notrealA = 1.0f;  
            }  
            if (buringpewter)  
            {  
                pewterdivby=2;
                xSpeed = 7;  
                jumpStrength = 10;  
            }  
            if (buringsteel)  
            {  
                pushForce = 1750;
            } 
            if (buringiron)  
            {  
                pullForce = 2000;
            } 
            metalDetectRange = 20;
            slowMotionScale = 0.1f;
        }  

        if (tinbarpercent < 1)  
        {  
            buringtin = false;  
            mistpsA = 0.075f;  
        }  
        if (pewterbarpercent < 1)  
        {  
            buringpewter = false;
            pewterdivby = 1;  
            xSpeed = 3.5f;  
            jumpStrength = 6.1f;  
        }  
        if (steelbarpercent < 1)  
        {  
            buringsteel = false;  
        }  
        if (ironbarpercent < 1)  
        {  
            buringiron = false; 
        }  

        // Ground check  
        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();  
        Vector2 rayOrigin = (Vector2)transform.position + capsule.offset;  
        float halfWidth = capsule.size.x * 0.5f * transform.localScale.x;  
        float rayDistance = groundCheckDistance;  
        int groundLayer = LayerMask.GetMask("ground");  

        Vector2 leftEdge = rayOrigin + Vector2.left * halfWidth;  
        Vector2 center = rayOrigin;  
        Vector2 rightEdge = rayOrigin + Vector2.right * halfWidth;  

        RaycastHit2D leftRay = Physics2D.Raycast(leftEdge, Vector2.down, rayDistance, groundLayer);  
        RaycastHit2D centerRay = Physics2D.Raycast(center, Vector2.down, rayDistance, groundLayer);  
        RaycastHit2D rightRay = Physics2D.Raycast(rightEdge, Vector2.down, rayDistance, groundLayer);  
       
        Debug.DrawRay(leftEdge, Vector2.down * rayDistance, new Color32(13, 0, 120, 255)); // steel-blue
        Debug.DrawRay(center, Vector2.down * rayDistance, new Color32(16, 157, 192, 255));     // iron-blue
        Debug.DrawRay(rightEdge, Vector2.down * rayDistance, new Color32(0, 128, 255, 255)); // cyan-blue

        isGrounded = leftRay.collider != null || centerRay.collider != null || rightRay.collider != null;  

            float xHat = Input.GetAxisRaw("Horizontal");  
            float vx = xHat * xSpeed;  
            rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);  


        anim.SetFloat("ySpeed", rb.linearVelocity.y);  
        anim.SetFloat("xSpeed", rb.linearVelocity.x);  
        if (rb.linearVelocity.x > 0.1)
        {
            lastmove = true;
            anim.SetBool("lastmove", true);
        }
        if (rb.linearVelocity.x < -0.1)
        {
            lastmove = false;
            anim.SetBool("lastmove", false);
        }

        var main = mistps.main;  
        Color c = main.startColor.color;  
        c.a = mistpsA;  
        main.startColor = new ParticleSystem.MinMaxGradient(c);  

        Color tileColor = notreal.color;  
        tileColor.a = notrealA;  
        notreal.color = tileColor;  






                if (buringpewter)
                {
                    if (helth>0)
                    {
                        newAlphaValue=1.0f;
                    }
                    else
                    {
                        newAlphaValue = (float)((helth*4) + 255) / 255;
                    }
                }
                else
                {
                    newAlphaValue=0.0f;
                }
                pweterhelth.color = new Color(
                    pweterhelth.color.r,
                    pweterhelth.color.g,
                    pweterhelth.color.b,
                    newAlphaValue
                );



                if (helth<0 && !buringpewter) 
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                else if(buringpewter && !flaring && helth<-25)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }












        metalsinarea = Physics2D.OverlapCircleAll(transform.position, metalDetectRange, meatelayer);
        if (spacetoogle && metalsinarea.Length > 0)
        {
        Debug.Log("Hits found: " + metalsinarea.Length);
        objectLineMap.Clear();
        foreach (Collider2D col in metalsinarea)
        {
        Debug.DrawLine(transform.position, col.transform.position, Color.red);
        Debug.Log("line");
        }
        }



    }

    void OnCollisionEnter2D(Collision2D collision)
    {
    if (!collision.gameObject.CompareTag("boxing")) {
    Vector2 falldamage = collision.relativeVelocity;
    float damageonimpact = falldamage.magnitude;
    if (damageonimpact > 15f){
        helth -= (int)Math.Round(damageonimpact/pewterdivby);
        Debug.Log("damage from fall" + helth);
    }
    }
        if (collision.gameObject.CompareTag("Tin"))
        {
            Destroy(collision.gameObject);
            tinbarpercent += 250;
            if (tinbarpercent > 500) tinbarpercent = 500;
        }
        if (collision.gameObject.CompareTag("Pewter"))  
        {  
            Destroy(collision.gameObject);  
            pewterbarpercent += 50;  
            if (pewterbarpercent > 100) pewterbarpercent = 100;  
        }  
        if (collision.gameObject.CompareTag("Steel"))  
        {  
            Destroy(collision.gameObject);  
            steelbarpercent += 100;  
            if (steelbarpercent > 200) steelbarpercent = 200;  
        }  
        if (collision.gameObject.CompareTag("Iron"))  
        {  
            Destroy(collision.gameObject);  
            ironbarpercent += 100;  
            if (ironbarpercent > 200) ironbarpercent = 200;  
        }  
        if (collision.gameObject.CompareTag("Aitum"))  
        {  
            Destroy(collision.gameObject);  
            pewterbarpercent += 25;  
            if (pewterbarpercent > 100) pewterbarpercent = 100;  
            tinbarpercent += 125;  
            if (tinbarpercent > 500) tinbarpercent = 500;  
            steelbarpercent += 50;  
            if (steelbarpercent > 200) steelbarpercent = 200;  
            ironbarpercent += 50;  
            if (ironbarpercent > 200) ironbarpercent = 200;  
        }  
        if (collision.gameObject.CompareTag("helth"))  
        {  
            Destroy(collision.gameObject);  
            helth+= 50;  
            if (helth > 100) helth = 100;  
        }  
        if (collision.gameObject.CompareTag("boxing"))  
        {  
            Destroy(collision.gameObject);  
            numboxings += 1; 
        }  

    }  

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider != null && collision.collider.CompareTag("enemy") && thymething1)
        {
            helth -= 4-pewterdivby;
            thymething1 = false;
        }
    }



}


public class TileTargetInfo : MonoBehaviour
{
    public Tilemap tilemap;
    public Vector3Int cell;
}

    
