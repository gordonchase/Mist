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

    private float maxVelX;  

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
    public int steelbarpercent=400;  
    public int ironbarpercent=400;  

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
    public List<Collider2D> metalsinarea = null;
    public List<Collider2D> pushmetals = null;
    public List<Collider2D> pullmetals = null;
    public bool spacetoogle=false;
    public LineRenderer lr;
    public LineRenderer lrr;
    public LineRenderer lrrr;
    public LineRenderer lrrrr;
    private int numthingy17 = 0;
    private int numthingy18 = 0;
    private int numthingy19 = 0;
    public int highlightnumthing12 = 0;

    public bool flaringtin = false;
    public bool flaringiron = false;
    public bool flaringsteel = false;
    public bool flaringpewter = false;


    private bool goright = false;
    private bool goleft = false;






    void Awake()
    {
    lr = gameObject.AddComponent<LineRenderer>();
    lr.positionCount = 2;
    lr.startWidth = lr.endWidth = 0.05f;
    lr.material = new Material(Shader.Find("Sprites/Default"));
    lr.startColor = lr.endColor =  new Color32(0, 0, 255, 100);;

    GameObject line2 = new GameObject("LineRenderer2");
    line2.transform.SetParent(gameObject.transform);
    lrr = line2.AddComponent<LineRenderer>();
    lrr.positionCount = 2;
    lrr.startWidth = lrr.endWidth = 0.1f;
    lrr.material = new Material(Shader.Find("Sprites/Default"));
    lrr.startColor = lrr.endColor =  new Color32(0, 255, 181, 200);;

    GameObject line3 = new GameObject("LineRenderer3");
    line3.transform.SetParent(gameObject.transform);
    lrrr = line3.AddComponent<LineRenderer>();
    lrrr.positionCount = 2;
    lrrr.startWidth = lrrr.endWidth = 0.05f;
    lrrr.material = new Material(Shader.Find("Sprites/Default"));
    lrrr.startColor = lrrr.endColor =  new Color32(13, 0, 120, 255);;

    GameObject line4 = new GameObject("LineRenderer4");
    line4.transform.SetParent(gameObject.transform);
    lrrrr = line4.AddComponent<LineRenderer>();
    lrrrr.positionCount = 2;
    lrrrr.startWidth = lrrrr.endWidth = 0.05f;
    lrrrr.material = new Material(Shader.Find("Sprites/Default"));
    lrrrr.startColor = lrrrr.endColor =  new Color32(16, 157, 192, 255);;
    }

    void Start()  
    {  
        isGrounded = false;  
        rb = GetComponent<Rigidbody2D>();  
        anim = GetComponent<Animator>();  
        key = 0;  
        StartCoroutine(DoEverySecond());  
        xSpeed = 3.5f;  
        maxVelX = 3.5f;
        jumpStrength = 6.1f;  
        notrealA = 1.0f;  
        pushForce = 30;
        pullForce = 30;
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
                if (flaringtin)  
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
                if (flaringpewter)  
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
                if (flaringsteel)  
                {  
                    steelbarpercent -= pushmetals.Count*2;  
                }  
                else  
                {  
                    steelbarpercent -= pushmetals.Count;  
                }  
            }  

            if (buringiron && ironbarpercent > 0)  
            {  
                if (flaringiron)  
                {  
                    ironbarpercent -= pullmetals.Count*2;  
                }  
                else  
                {  
                    ironbarpercent -= pullmetals.Count;  
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
            if (numboxings>0)  
            {  
            numboxings-=1;

            buringsteel = true;
            GameObject newcoin = Instantiate(boxingfab, transform.position + new Vector3(0, -2.5f, 0), Quaternion.identity);
            pushmetals.Add(newcoin.GetComponent<Collider2D>());
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
            GameObject newcoin = Instantiate(boxingfab, transform.position + new Vector3(xOffsetAmount, 0, 0), Quaternion.identity);
            pushmetals.Add(newcoin.GetComponent<Collider2D>());
            }
        } 
        if (Input.GetKeyDown(KeyCode.W)&&isGrounded)
        {
            rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
        List<Collider2D> temp = pushmetals;
        pushmetals = pullmetals;
        pullmetals = temp;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
        pullmetals.Clear();
        pushmetals.Clear();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            goright=true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            goleft = true;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            goright=false;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            goleft = false;
        }

    }  



    void FixedUpdate()  
    {






        float xsub = xSpeed*1.5f;
        maxVelX = xsub;
        if (goright && ((Math.Abs(rb.linearVelocityX)<maxVelX) || (rb.linearVelocityX < 0)))
        {
            float thingy5=maxVelX-Math.Abs(rb.linearVelocityX);
            if (thingy5>xsub){thingy5=xsub;}
            rb.AddForce(new Vector2 (thingy5/1* Time.deltaTime *100,0),ForceMode2D.Force);
            if (rb.linearVelocityX < 0)
            {
                rb.AddForce(new Vector2 (1f* Time.deltaTime *100,0),ForceMode2D.Impulse);
            }
        }
        if (goleft && ((Math.Abs(rb.linearVelocityX)<maxVelX) || (rb.linearVelocityX > 0)))
        {
            float thingy6=maxVelX-Math.Abs(rb.linearVelocityX);
            if (thingy6>xsub){thingy6=xsub;}
            rb.AddForce(new Vector2 (thingy6/-1* Time.deltaTime *100,0),ForceMode2D.Force);
            if (rb.linearVelocityX > 0)
            {
                rb.AddForce(new Vector2 (-1f* Time.deltaTime *100,0),ForceMode2D.Impulse);
            }
        
        } 


        if (helth>100){helth=100;}

        if (buringtin && flaringtin)  
        {  
            mistpsA = 0.002f;  
            notrealA = 0.3f;  
        }  
        else if (buringtin)
        {
        mistpsA = 0.015f;  
        notrealA = 1.0f; 
        }
        else
        {
        mistpsA = 0.075f;  
        notrealA = 1.0f; 
        }
        if (buringpewter&&flaringpewter)  
        {  
        pewterdivby =3;
        xSpeed = 10; 
        jumpStrength = 13;  
        } 
        else if (buringpewter)
        {
        pewterdivby=2;
        xSpeed = 7;  
        jumpStrength = 10;  
        }
        else
        {
        pewterdivby =1;
        xSpeed = 3.5f;  
        jumpStrength = 6.1f; 
        }
        if (buringsteel&&flaringsteel)  
        {  
        pushForce = 50;
        } 
        else if (buringsteel)
        {
        pushForce = 30;
        }
        if (buringiron&&flaringiron)  
        {  
        pullForce = 50;
        } 
        else if (buringiron)
        {
        pullForce = 30;    
        }
        if (flaringiron||flaringsteel){
        metalDetectRange = 30;
        slowMotionScale = 0.05f; 
        if (spacetoogle){Time.timeScale = slowMotionScale;}
        }
        else
        {
        metalDetectRange = 20;
        slowMotionScale = 0.1f;
        if (spacetoogle){Time.timeScale = slowMotionScale;}
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
                else if(buringpewter && !flaringpewter && helth<-25)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }









        pullmetals.RemoveAll(c => c == null);
        pushmetals.RemoveAll(c => c == null);









        Vector3 mouseScreen = Input.mousePosition;
        Vector3 mouseWorld3 = Camera.main.ScreenToWorldPoint(mouseScreen);
        Vector2 mousePos = new Vector2(mouseWorld3.x, mouseWorld3.y);
        float lagestAngle = 0;
        metalsinarea = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, metalDetectRange, meatelayer));
        if (spacetoogle && metalsinarea.Count > 0)
        {
        lrr.enabled=true;
        lr.enabled = true;
        lr.positionCount = 2*metalsinarea.Count;
        numthingy17 = 0;
        foreach (Collider2D col in metalsinarea)
            {
            lr.SetPosition(numthingy17, transform.position);
            numthingy17++;    
            lr.SetPosition(numthingy17, col.bounds.center);
            numthingy17++;
            }

        for (int ix = 0; ix < metalsinarea.Count; ix++)
            {
            Vector2 playerpos = transform.position;
            Vector2 obpos = metalsinarea[ix].bounds.center;
            float angle = Vector2.Angle((playerpos - mousePos).normalized, (obpos - mousePos).normalized);
            if (lagestAngle < angle)
                {
                lagestAngle = angle;
                highlightnumthing12=ix;
                }
            }
        lrr.SetPosition(0, transform.position);
        lrr.SetPosition(1, metalsinarea[highlightnumthing12].bounds.center);
        }
        else{lr.enabled = false;lrr.enabled=false;}
        

        if (buringsteel){
        foreach (Collider2D col in pushmetals)
        {
            if (col==null){continue;}
            Rigidbody2D pushrb = col.attachedRigidbody;

            if (pushrb != null)
            {
                Vector2 dierec_to_player = pushrb.position - (Vector2)transform.position;

                pushrb.AddForce(dierec_to_player.normalized * pushForce* Time.deltaTime *50 / dierec_to_player.magnitude, ForceMode2D.Force);
            }
            Vector2 dierec_to_pushob = (Vector2)transform.position - (Vector2)col.transform.position;
            
            rb.AddForce(dierec_to_pushob.normalized * pushForce* Time.deltaTime *50 / dierec_to_pushob.magnitude, ForceMode2D.Force);

        }
        }
        if(buringiron){
        foreach (Collider2D col in pullmetals)
        {
            if (col==null){continue;}
            Rigidbody2D pullrb = col.attachedRigidbody;

            if (pullrb != null)
            {
                Vector2 dierec_to_player = pullrb.position - (Vector2)transform.position;

                pullrb.AddForce(-dierec_to_player.normalized * pullForce * Time.deltaTime *50 / dierec_to_player.magnitude, ForceMode2D.Force);
            }
            Vector2 dierec_to_pullob = (Vector2)transform.position - (Vector2)col.transform.position;

            rb.AddForce(-dierec_to_pullob.normalized * pullForce * Time.deltaTime *50/ dierec_to_pullob.magnitude, ForceMode2D.Force);

        }
        }






        pushmetals.RemoveAll(c => c == null);
        lrrr.enabled = true;
        lrrr.positionCount = 2*pushmetals.Count;
        numthingy18 = 0;
        foreach (Collider2D col in pushmetals)
            {
            lrrr.SetPosition(numthingy18, transform.position);
            numthingy18++;    
            lrrr.SetPosition(numthingy18, col.bounds.center);
            numthingy18++;
            }

        lrrrr.enabled = true;
        lrrrr.positionCount = 2*pullmetals.Count;
        numthingy19 = 0;
        foreach (Collider2D col in pullmetals)
            {
            lrrrr.SetPosition(numthingy19, transform.position);
            numthingy19++;    
            lrrrr.SetPosition(numthingy19, col.bounds.center);
            numthingy19++;
            }



















    }

























    void OnCollisionEnter2D(Collision2D collision)
    {
    if (!collision.gameObject.CompareTag("boxing")) {
    Vector2 falldamage = collision.relativeVelocity;
    float damageonimpact = falldamage.magnitude;
    if (damageonimpact > 5*pewterdivby+10){
        helth -= ((int)Math.Round(damageonimpact/pewterdivby))-5*pewterdivby+10;
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
            steelbarpercent += 200;  
            if (steelbarpercent > 400) steelbarpercent = 400;  
        }  
        if (collision.gameObject.CompareTag("Iron"))  
        {  
            Destroy(collision.gameObject);  
            ironbarpercent += 200;  
            if (ironbarpercent > 400) ironbarpercent = 400;  
        }  
        if (collision.gameObject.CompareTag("Aitum"))  
        {  
            Destroy(collision.gameObject);  
            pewterbarpercent += 25;  
            if (pewterbarpercent > 100) pewterbarpercent = 100;  
            tinbarpercent += 125;  
            if (tinbarpercent > 500) tinbarpercent = 500;  
            steelbarpercent += 100;  
            if (steelbarpercent > 400) steelbarpercent = 400;  
            ironbarpercent += 100;  
            if (ironbarpercent > 400) ironbarpercent = 400;  
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