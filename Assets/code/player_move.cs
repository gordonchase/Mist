using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    // ===============================
    // CONFIGURABLE VARIABLES
    // ===============================
    // These are variables you can freely change in your code:
    //
    // linePrefab          -> Prefab for the LineRenderer used when selecting metals.
    // metalDetectRange    -> The radius around the player to detect metal objects.
    // metalLayer          -> LayerMask for metals that can be selected.
    // pullForce           -> Force applied when pulling a selected metal toward you.
    // pushForce           -> Force applied when pushing a selected metal away.
    // slowMotionScale     -> The timescale when line selector is active (slows game).
    //
    // You can also modify these burn/flare variables as you like:
    // flaring, buringtin, buringpewter, buringsteel, buringiron
    // xSpeed, jumpStrength, mistpsA, notrealA
    //
    // Example usage:
    // pullForce = 1000f; metalDetectRange = 10f; slowMotionScale = 0.2f;



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

    // ===== Line Selector Variables =====  
    public GameObject linePrefab; // prefab with LineRenderer  
    public float metalDetectRange = 8f;  
    public LayerMask metalLayer;  
    private List<Transform> metalTargets = new List<Transform>();  
    private List<LineRenderer> lines = new List<LineRenderer>();  
    private int selectedIndex = 0;  
    private bool isLineChooserActive = false;  
    // chooserMode bitmask: 1 = steel chooser active, 2 = iron chooser active
    private int chooserMode = 0;
    private Transform steelTarget = null; // push target 
    private Transform placeholder = null; 
    private Transform ironTarget = null;  // pull target  
    public float pullForce = 500f;  
    public float pushForce = 500f;  
    public float slowMotionScale = 0.2f;  

    // Persistent line renderers for active push/pull targets (kept while burning is active)
    private LineRenderer steelPersistentLine = null;
    private LineRenderer ironPersistentLine = null;
    // Optional distinct prefabs for persistent steel/iron lines (set in Inspector)
    public GameObject steelLinePrefab;
    public GameObject ironLinePrefab;
    // Optional prefab for converted physics tiles (set in Inspector). If null, created at runtime.
    public GameObject tilePhysicsPrefab;

    // Line colors (configure in Inspector or here)
    public Color32 steelLineColor = new Color32(13, 0, 120, 255);
    public Color32 ironLineColor  = new Color32(16, 157, 192, 255);

    // Shared neutral material used for LineRenderers so vertex colors show accurately
    private Material neutralLineMaterial;
    // If true, force line alpha to 1.0 (useful to override accidental transparency)
    public bool forceOpaqueLines = true;
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



    void linechooser()  
    {  
        Time.timeScale = slowMotionScale;  
    }  

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
        // Create a single neutral material instance for line renderers (white tint)
        neutralLineMaterial = new Material(Shader.Find("Sprites/Default"));
        neutralLineMaterial.color = Color.white;
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
                steelTarget = null; 
                // If chooser is open, remove steel from chooser mode; only fully close chooser if no types remain
                if (isLineChooserActive)
                {
                    chooserMode &= ~1;
                    if (chooserMode == 0)
                    {
                        isLineChooserActive = false;
                        Time.timeScale = 1f;
                        DestroyChooserLines();
                        superanoyingjumpthing = false;
                    }
                    else
                    {
                        // still have another chooser (e.g., iron) active
                        HighlightSelectedLine();
                    }
                }
            }  
            else  
            {  
                buringsteel = true;  
                superanoyingjumpthing = true;
                ActivateLineChooser();
            }  
        }  

        if (Input.GetKeyDown(KeyCode.V))  
        {  
            if (buringiron)  
            {  
                buringiron = false;  
                ironTarget = null;
                if (isLineChooserActive)
                {
                    chooserMode &= ~2;
                    if (chooserMode == 0)
                    {
                        isLineChooserActive = false;
                        Time.timeScale = 1f;
                        DestroyChooserLines();
                        superanoyingjumpthing = false;
                    }
                    else
                    {
                        HighlightSelectedLine();
                    }
                }
            }  
            else  
            {  
                buringiron = true;  
                superanoyingjumpthing = true;
                ActivateLineChooser();  
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
            steelTarget=Instantiate(boxingfab, transform.position + new Vector3(xOffsetAmount, 0, 0), Quaternion.identity).transform;
            CreateOrUpdatePersistentLine(ref steelPersistentLine, steelTarget, steelLineColor, steelLinePrefab);
            }
        } 
        if (Input.GetKeyUp(KeyCode.W)){superanoyingjumpthing = false;}
        if (Input.GetKeyDown(KeyCode.W)&&isGrounded&&!superanoyingjumpthing)
        {
        rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.R))  
        {  
            if(!(steelTarget == null)){buringiron = true;}
            if(!(ironTarget == null)){buringsteel = true;}
            placeholder=steelTarget;
            steelTarget=ironTarget;
            ironTarget=placeholder; 
            if(!(steelTarget == null)){CreateOrUpdatePersistentLine(ref steelPersistentLine, steelTarget, steelLineColor, steelLinePrefab);}
            if(!(ironTarget == null)){CreateOrUpdatePersistentLine(ref ironPersistentLine, ironTarget, ironLineColor, ironLinePrefab);} 
        }
    }  


    void FixedUpdate()  
    {  




        if(ironTarget == null && !isLineChooserActive){buringiron = false;}
        if(steelTarget == null && !isLineChooserActive){buringsteel = false;}

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
            steelTarget = null;  
        }  
        if (ironbarpercent < 1)  
        {  
            buringiron = false;  
            ironTarget = null;  
        }  

        if (!buringsteel && steelPersistentLine != null)
        {
            Destroy(steelPersistentLine.gameObject);
            steelPersistentLine = null;
            steelTarget = null;
        }
        if (!buringiron && ironPersistentLine != null)
        {
            Destroy(ironPersistentLine.gameObject);
            ironPersistentLine = null;
            ironTarget = null;
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

        if (!isLineChooserActive)  
        {  
            float xHat = Input.GetAxisRaw("Horizontal");  
            float vx = xHat * xSpeed;  
            rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);  

            // float yHat = new Vector2(0, Input.GetAxis("Vertical")).normalized.y;  
            // if (isGrounded && yHat == 1 && !superanoyingjumpthing)  
            // {  
            //     float vy = yHat * jumpStrength;  
            //     rb.AddForce(transform.up * vy);
            // }  
        }  

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

        // ---------------------------  
        // Push & Pull Logic  
        // ---------------------------  
        // Pull iron  
        if (buringiron && ironTarget != null)  
        {  
            Rigidbody2D rbMetal = ironTarget.GetComponent<Rigidbody2D>();  
            // if (rbMetal == null) Debug.Log("Pull: target has no Rigidbody2D: " + (ironTarget!=null?ironTarget.name:"null"));
            Vector2 dir = (ironTarget.position - transform.position).normalized;  
            float distance = Vector2.Distance(ironTarget.position, transform.position);  
            // Force attenuation based on actual distance (metalDetectRange still used only for detection)
            // Inverse-linear falloff: force decreases as distance increases, avoids divide-by-zero.
            float scaledPull = pullForce / (distance + 1f);

            if (rbMetal != null)  
                rbMetal.AddForce(-dir * scaledPull * Time.fixedDeltaTime);  
            rb.AddForce(dir * scaledPull * Time.fixedDeltaTime);  
        }  

        // Push steel  
        if (buringsteel && steelTarget != null)  
        {  
            Rigidbody2D rbMetal = steelTarget.GetComponent<Rigidbody2D>();  
            // if (rbMetal == null) Debug.Log("Push: target has no Rigidbody2D: " + (steelTarget!=null?steelTarget.name:"null"));
            Vector2 dir = (steelTarget.position - transform.position).normalized;  
            float distance = Vector2.Distance(steelTarget.position, transform.position);  
            // Force attenuation based on actual distance (metalDetectRange still used only for detection)
            // Inverse-linear falloff: force decreases as distance increases, avoids divide-by-zero.
            float scaledPush = pushForce / (distance + 1f);

            if (rbMetal != null)  
                rbMetal.AddForce(dir * scaledPush * Time.fixedDeltaTime);  
            rb.AddForce(-dir * scaledPush * Time.fixedDeltaTime);  
        }  



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
            // Debug.Log("Damage applied via OnCollisionStay2D");
        }
    }
}

// Helper component attached to temporary tile target GameObjects so we know origin Tilemap and cell
public class TileTargetInfo : MonoBehaviour
{
    public Tilemap tilemap;
    public Vector3Int cell;
}

    
