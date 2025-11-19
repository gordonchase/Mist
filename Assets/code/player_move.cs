using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    private Transform steelTarget = null; // push target  
    private Transform ironTarget = null;  // pull target  
    public float pullForce = 500f;  
    public float pushForce = 500f;  
    public float slowMotionScale = 0.2f;  

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
        jumpStrength = 300;  
        notrealA = 1.0f;  
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

        if (Input.GetKeyDown(KeyCode.X))  
        {  
            if (buringpewter)  
            {  
                xSpeed = 3.5f;  
                jumpStrength = 300;  
                buringpewter = false;  
            }  
            else  
            {  
                xSpeed = 7;  
                jumpStrength = 450;  
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
                ActivateLineChooser();  
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
    }  

    void FixedUpdate()  
    {  
        if (flaring)  
        {  
            if (buringtin)  
            {  
                mistpsA = 0.002f;  
                notrealA = 0.3f;  
            }  
            if (buringpewter)  
            {  
                xSpeed = 10;  
                jumpStrength = 650;  
            }  
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

        // Ground check  
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

        if (!isLineChooserActive)  
        {  
            float xHat = Input.GetAxisRaw("Horizontal");  
            float vx = xHat * xSpeed;  
            rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);  

            float yHat = new Vector2(0, Input.GetAxis("Vertical")).normalized.y;  
            if (isGrounded && yHat == 1)  
            {  
                float vy = yHat * jumpStrength;  
                rb.AddForce(transform.up * vy);  
            }  
        }  

        anim.SetFloat("ySpeed", rb.linearVelocity.y);  
        anim.SetFloat("xSpeed", rb.linearVelocity.x);  

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
            Vector2 dir = (ironTarget.position - transform.position).normalized;  
            float distance = Vector2.Distance(ironTarget.position, transform.position);  
            float scaledPull = pullForce * Mathf.Clamp01(1f - (distance / metalDetectRange));  

            if (rbMetal != null)  
                rbMetal.AddForce(-dir * scaledPull * Time.fixedDeltaTime);  
            rb.AddForce(dir * scaledPull * Time.fixedDeltaTime);  
        }  

        // Push steel  
        if (buringsteel && steelTarget != null)  
        {  
            Rigidbody2D rbMetal = steelTarget.GetComponent<Rigidbody2D>();  
            Vector2 dir = (steelTarget.position - transform.position).normalized;  
            float distance = Vector2.Distance(steelTarget.position, transform.position);  
            float scaledPush = pushForce * Mathf.Clamp01(1f - (distance / metalDetectRange));  

            if (rbMetal != null)  
                rbMetal.AddForce(dir * scaledPush * Time.fixedDeltaTime);  
            rb.AddForce(-dir * scaledPush * Time.fixedDeltaTime);  
        }  
    }  

    // ---------------------------
    // Collision Logic
    // ---------------------------
    void OnCollisionEnter2D(Collision2D collision)  
    {  
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
    }  

    void ActivateLineChooser()  
    {  
        isLineChooserActive = true;  
        Time.timeScale = slowMotionScale;  
        FindMetalTargets();  
        CreateLines();  
        HighlightSelectedLine();  
    }  

    void FindMetalTargets()  
    {  
        metalTargets.Clear();  
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, metalDetectRange, metalLayer);  
        foreach (Collider2D col in hits)  
        {  
            if (col.isTrigger) continue;  
            metalTargets.Add(col.transform);  
        }  
    }  

    void CreateLines()  
    {  
        DestroyAllLines();  
        foreach (Transform metal in metalTargets)  
        {  
            GameObject lineObj = Instantiate(linePrefab);  
            LineRenderer lr = lineObj.GetComponent<LineRenderer>();  
            lr.positionCount = 2;  
            lr.startColor = Color.cyan;  
            lr.endColor = Color.cyan;  
            lr.startWidth = 0.05f;  
            lr.endWidth = 0.05f;  
            lines.Add(lr);  
        }  
    }  

    void UpdateLines()  
    {  
        for (int i = 0; i < lines.Count; i++)  
        {  
            if (lines[i] == null) continue;  
            lines[i].SetPosition(0, transform.position);  
            lines[i].SetPosition(1, metalTargets[i].position);  
        }  
    }  

    void HighlightSelectedLine()  
    {  
        for (int i = 0; i < lines.Count; i++)  
        {  
            if (lines[i] == null) continue;  
            if (i == selectedIndex)  
            {  
                lines[i].startColor = Color.blue;  
                lines[i].endColor = Color.blue;  
                lines[i].startWidth = 0.1f;  
                lines[i].endWidth = 0.1f;  
            }  
            else  
            {  
                lines[i].startColor = Color.cyan;  
                lines[i].endColor = Color.cyan;  
                lines[i].startWidth = 0.05f;  
                lines[i].endWidth = 0.05f;  
            }  
        }  
    }  

    void DestroyAllLines()  
    {  
        foreach (LineRenderer lr in lines)  
        {  
            if (lr != null)  
                Destroy(lr.gameObject);  
        }  
        lines.Clear();  
    }  

    void HandleLineChooserInput()  
    {  
        if (!isLineChooserActive) return;  

        if (Input.GetKeyDown(KeyCode.A))  
        {  
            selectedIndex--;  
            if (selectedIndex < 0) selectedIndex = metalTargets.Count - 1;  
            HighlightSelectedLine();  
        }  

        if (Input.GetKeyDown(KeyCode.D))  
        {  
            selectedIndex++;  
            if (selectedIndex >= metalTargets.Count) selectedIndex = 0;  
            HighlightSelectedLine();  
        }  

        if (Input.GetKeyDown(KeyCode.W))  
        {  
            isLineChooserActive = false;  
            Time.timeScale = 1f;  
            DestroyAllLines();  

            // Assign separate targets  
            if (buringsteel)  
                steelTarget = metalTargets[selectedIndex];  
            if (buringiron)  
                ironTarget = metalTargets[selectedIndex];  
        }  

        if (Input.GetKeyDown(KeyCode.S))  
        {  
            isLineChooserActive = false;  
            Time.timeScale = 1f;  
            DestroyAllLines();  
        }  
    }  

    void LateUpdate()  
    {  
        UpdateLines();  
        HandleLineChooserInput();  
    }  
}
