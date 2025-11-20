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
    // chooserMode bitmask: 1 = steel chooser active, 2 = iron chooser active
    private int chooserMode = 0;
    private Transform steelTarget = null; // push target  
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
                xSpeed = 7;  
                jumpStrength = 450;  
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
            xSpeed = 3.5f;  
            jumpStrength = 300;  
        }  
        if (steelbarpercent < 1)  
        {  
            buringsteel = false;  
            steelTarget = null;  
            // If chooser was open for steel, remove that bit. Don't close chooser entirely if iron remains.
            if (isLineChooserActive)
            {
                chooserMode &= ~1;
                if (chooserMode == 0)
                {
                    isLineChooserActive = false;
                    Time.timeScale = 1f;
                    DestroyChooserLines();
                }
            }
        }  
        if (ironbarpercent < 1)  
        {  
            buringiron = false;  
            ironTarget = null;  
            // If chooser was open for iron, remove that bit. Don't close chooser entirely if steel remains.
            if (isLineChooserActive)
            {
                chooserMode &= ~2;
                if (chooserMode == 0)
                {
                    isLineChooserActive = false;
                    Time.timeScale = 1f;
                    DestroyChooserLines();
                }
            }
        }  

        // If burning flags are false, ensure persistent lines and targets are cleared
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
            if (rbMetal == null) Debug.Log("Pull: target has no Rigidbody2D: " + (ironTarget!=null?ironTarget.name:"null"));
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
            if (rbMetal == null) Debug.Log("Push: target has no Rigidbody2D: " + (steelTarget!=null?steelTarget.name:"null"));
            Vector2 dir = (steelTarget.position - transform.position).normalized;  
            float distance = Vector2.Distance(steelTarget.position, transform.position);  
            // Force attenuation based on actual distance (metalDetectRange still used only for detection)
            // Inverse-linear falloff: force decreases as distance increases, avoids divide-by-zero.
            float scaledPush = pushForce / (distance + 1f);

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
        // Set chooser mode bits depending on which burn types are active
        chooserMode = 0;
        if (buringsteel) chooserMode |= 1;
        if (buringiron) chooserMode |= 2;
        if (chooserMode == 0)
        {
            // Nothing to choose for
            return;
        }

        // Clear any previous chooser state, then build targets and lines
        isLineChooserActive = true;  
        Time.timeScale = slowMotionScale;  
        DestroyChooserLines();
        FindMetalTargets();  
        selectedIndex = 0;
        CreateLines();  
        HighlightSelectedLine();  
    }  

    void FindMetalTargets()  
    {  
        metalTargets.Clear();  
        DestroyTempTargets();

        // Add collider-based targets (existing objects on metal layer)
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, metalDetectRange, metalLayer);  
        foreach (Collider2D col in hits)  
        {  
            if (col.isTrigger) continue;  
            metalTargets.Add(col.transform);  
        }  

        // Also find Tilemap tiles that are on GameObjects with the 'metal' layer
        int metalLayerIndex = LayerMask.NameToLayer("metal");
        Tilemap[] allTilemaps = GameObject.FindObjectsOfType<Tilemap>();
        foreach (Tilemap tm in allTilemaps)
        {
            if (tm.gameObject.layer != metalLayerIndex) continue;

            // compute cell bounds to search
            Vector3Int centerCell = tm.WorldToCell(transform.position);
            int cellRadius = Mathf.CeilToInt(metalDetectRange / Mathf.Max(tm.cellSize.x, tm.cellSize.y));
            for (int dx = -cellRadius; dx <= cellRadius; dx++)
            {
                for (int dy = -cellRadius; dy <= cellRadius; dy++)
                {
                    Vector3Int cell = new Vector3Int(centerCell.x + dx, centerCell.y + dy, centerCell.z);
                    Vector3 worldCenter = tm.GetCellCenterWorld(cell);
                    if ((worldCenter - transform.position).magnitude > metalDetectRange) continue;
                    TileBase tb = tm.GetTile(cell);
                    if (tb == null) continue;

                    // create a temporary target GameObject at the cell center
                    GameObject tmp = new GameObject("TileTarget");
                    tmp.transform.position = worldCenter;
                    tmp.transform.parent = GetOrCreateTempParent();
                    // mark with TileTargetInfo so conversion can identify origin tilemap and cell
                    TileTargetInfo info = tmp.AddComponent<TileTargetInfo>();
                    info.tilemap = tm;
                    info.cell = cell;
                    metalTargets.Add(tmp.transform);
                }
            }
        }
        Debug.Log($"FindMetalTargets: found {metalTargets.Count} targets (including tile cells)");
    }  

    void CreateLines()  
    {  
        // Create one chooser line per detected target. Do NOT destroy temp targets here; they were just created.
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
            Debug.Log($"CreateLines: created {lines.Count} chooser lines");
        }

    void UpdateLines()  
    {  
        for (int i = 0; i < lines.Count; i++)  
        {  
            if (lines[i] == null) continue;  
            lines[i].SetPosition(0, transform.position);  
            lines[i].SetPosition(1, metalTargets[i].position);  
        }  
        // Update persistent lines for active targets (do not remove while burning)
        if (steelTarget != null && steelPersistentLine != null)  
        {  
            steelPersistentLine.SetPosition(0, transform.position);  
            steelPersistentLine.SetPosition(1, steelTarget.position);  
        }  
        if (ironTarget != null && ironPersistentLine != null)  
        {  
            ironPersistentLine.SetPosition(0, transform.position);  
            ironPersistentLine.SetPosition(1, ironTarget.position);  
        }  
    }  

    void HighlightSelectedLine()  
    {  
        for (int i = 0; i < lines.Count; i++)  
        {  
            if (lines[i] == null) continue;  
            // If this metal is already assigned as steelTarget or ironTarget, color accordingly
            if (metalTargets != null && i < metalTargets.Count && metalTargets[i] == steelTarget)  
            {  
                lines[i].startColor = Color.red;  
                lines[i].endColor = Color.red;  
                lines[i].startWidth = 0.08f;  
                lines[i].endWidth = 0.08f;  
            }  
            else if (metalTargets != null && i < metalTargets.Count && metalTargets[i] == ironTarget)  
            {  
                lines[i].startColor = Color.green;  
                lines[i].endColor = Color.green;  
                lines[i].startWidth = 0.08f;  
                lines[i].endWidth = 0.08f;  
            }  
            else if (i == selectedIndex)  
            {  
                // currently being navigated in chooser
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
        // Remove all chooser temporary lines
        foreach (LineRenderer lr in lines)  
        {  
            if (lr != null)  
                Destroy(lr.gameObject);  
        }  
        lines.Clear();  
        // Also remove persistent lines if they exist (full cleanup)
        if (steelPersistentLine != null)  
        {  
            Destroy(steelPersistentLine.gameObject);  
            steelPersistentLine = null;  
        }  
        if (ironPersistentLine != null)  
        {  
            Destroy(ironPersistentLine.gameObject);  
            ironPersistentLine = null;  
        }  
    }  

    // Destroys only the chooser temporary lines, keep persistent push/pull lines
    void DestroyChooserLines()
    {
        foreach (LineRenderer lr in lines)
        {
            if (lr != null)
                Destroy(lr.gameObject);
        }
        lines.Clear();
        DestroyTempTargets();
        // reset chooser mode when chooser lines are removed
        chooserMode = 0;
    }

    // Parent transform for temporary tile target objects
    private Transform tempTargetsParent = null;

    Transform GetOrCreateTempParent()
    {
        if (tempTargetsParent == null)
        {
            GameObject go = new GameObject("_TempTileTargets");
            tempTargetsParent = go.transform;
        }
        return tempTargetsParent;
    }

    void DestroyTempTargets()
    {
        if (tempTargetsParent == null) return;
        for (int i = tempTargetsParent.childCount - 1; i >= 0; i--)
        {
            var ch = tempTargetsParent.GetChild(i);
            if (ch != null)
                Destroy(ch.gameObject);
        }
        Destroy(tempTargetsParent.gameObject);
        tempTargetsParent = null;
    }

    // Create or replace a persistent line renderer for a target
    void CreateOrUpdatePersistentLine(ref LineRenderer persistent, Transform target, Color color, GameObject prefab)
    {
        if (target == null)
        {
            if (persistent != null)
            {
                Destroy(persistent.gameObject);
                persistent = null;
            }
            return;
        }

        if (persistent == null)
        {
            GameObject go = null;
            if (prefab != null)
                go = Instantiate(prefab);
            else
                go = Instantiate(linePrefab);
            persistent = go.GetComponent<LineRenderer>();
            persistent.positionCount = 2;
            persistent.startWidth = 0.07f;
            persistent.endWidth = 0.07f;
        }
        persistent.startColor = color;
        persistent.endColor = color;
        persistent.SetPosition(0, transform.position);
        persistent.SetPosition(1, target.position);
    }

    // Convert a tile at the closest cell to the player (on the provided transform's Tilemap) into
    // a physics-enabled GameObject and return its transform. If the transform isn't part of a Tilemap
    // or conversion fails, returns the original transform unchanged.
    Transform ConvertTileToPhysicsIfNeeded(Transform targetTransform)
    {
        if (targetTransform == null) return null;

        // If this target is a temporary Tile target, use its TileTargetInfo
        TileTargetInfo tinfo = targetTransform.GetComponent<TileTargetInfo>();
        Tilemap tm = null;
        Vector3Int cell = Vector3Int.zero;
        Vector3 closestWorld = transform.position;
        if (tinfo != null)
        {
            tm = tinfo.tilemap;
            cell = tinfo.cell;
            closestWorld = tm.GetCellCenterWorld(cell);
        }
        else
        {
            // Try to get Tilemap on the object or parent
            tm = targetTransform.GetComponent<Tilemap>();
            if (tm == null) tm = targetTransform.GetComponentInParent<Tilemap>();
            if (tm == null) return targetTransform; // not a Tilemap target

            // Find the Collider2D to get a closest point to the player
            Collider2D col = targetTransform.GetComponent<Collider2D>();
            if (col == null) col = targetTransform.GetComponentInParent<Collider2D>();

            closestWorld = (col != null) ? col.ClosestPoint(transform.position) : transform.position;
            cell = tm.WorldToCell(closestWorld);
        }

        // If the immediate cell has no tile, search nearby cells (radius 2) for the closest occupied cell.
        TileBase tileBase = tm.GetTile(cell);
        if (tileBase == null)
        {
            int searchRadius = 2; // search 5x5 area
            float bestDistSqr = float.MaxValue;
            Vector3Int bestCell = cell;
            TileBase found = null;
            for (int dx = -searchRadius; dx <= searchRadius; dx++)
            {
                for (int dy = -searchRadius; dy <= searchRadius; dy++)
                {
                    Vector3Int c = new Vector3Int(cell.x + dx, cell.y + dy, cell.z);
                    TileBase tb = tm.GetTile(c);
                    if (tb == null) continue;
                    Vector3 center = tm.GetCellCenterWorld(c);
                    float d2 = (center - closestWorld).sqrMagnitude;
                    if (d2 < bestDistSqr)
                    {
                        bestDistSqr = d2;
                        bestCell = c;
                        found = tb;
                    }
                }
            }
            if (found == null)
            {
                Debug.Log("ConvertTileToPhysicsIfNeeded: no TileBase at cell " + cell + " or nearby cells");
                return targetTransform; // no tile found nearby
            }
            // use nearest found
            cell = bestCell;
            tileBase = found;
            closestWorld = tm.GetCellCenterWorld(cell);
            Debug.Log("ConvertTileToPhysicsIfNeeded: found nearby tile at " + cell);
        }

        // Attempt to get a sprite: prefer Tilemap.GetSprite (handles RuleTile etc), fallback to TileBase cast
        Sprite tileSprite = tm.GetSprite(cell);
        if (tileSprite == null)
        {
            Tile tileObj = tileBase as Tile;
            if (tileObj != null) tileSprite = tileObj.sprite;
        }
        if (tileSprite == null)
        {
            Debug.Log("ConvertTileToPhysicsIfNeeded: couldn't determine sprite for tile at " + cell + ". Conversion aborted.");
            return targetTransform; // can't convert without sprite
        }

        // Remove the tile from the Tilemap
        tm.SetTile(cell, null);
        // Refresh tile and force TilemapCollider2D to rebuild so the removed cell no longer collides
        tm.RefreshTile(cell);
        var tcol = tm.GetComponent<TilemapCollider2D>();
        if (tcol != null)
        {
            tcol.enabled = false;
            tcol.enabled = true;
        }
        var comp = tm.GetComponent<CompositeCollider2D>();
        if (comp != null)
        {
            comp.enabled = false;
            comp.enabled = true;
        }

        // Instantiate physics object (use prefab if set)
        GameObject go;
        Vector3 spawnPos = tm.GetCellCenterWorld(cell);
        if (tilePhysicsPrefab != null)
        {
            go = Instantiate(tilePhysicsPrefab, spawnPos, Quaternion.identity);
            var sr = go.GetComponent<SpriteRenderer>();
            if (sr != null) sr.sprite = tileSprite;
        }
        else
        {
            go = new GameObject("PhysicsTile");
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = tileSprite;
            go.transform.position = spawnPos;
        }

        // Ensure physics components
        if (go.GetComponent<Rigidbody2D>() == null)
        {
            var rb = go.AddComponent<Rigidbody2D>();
            rb.mass = 1f;
            rb.gravityScale = 0f;
        }
        if (go.GetComponent<Collider2D>() == null)
        {
            var bc = go.AddComponent<BoxCollider2D>();
            var sr = go.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                bc.size = sr.sprite.bounds.size;
            }
        }

        // Put new object on metal layer if available
        int metalLayerIndex = LayerMask.NameToLayer("metal");
        if (metalLayerIndex != -1) go.layer = metalLayerIndex;

        Debug.Log($"ConvertTileToPhysicsIfNeeded: converted tile at {cell} to GameObject '{go.name}' at {spawnPos}");

        return go.transform;
    }

    void HandleLineChooserInput()
    {
        if (!isLineChooserActive) return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            selectedIndex--;
            if (selectedIndex < 0) selectedIndex = metalTargets.Count - 1;
            HighlightSelectedLine();
            Debug.Log($"Chooser: selectedIndex={selectedIndex}");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            selectedIndex++;
            if (selectedIndex >= metalTargets.Count) selectedIndex = 0;
            HighlightSelectedLine();
            Debug.Log($"Chooser: selectedIndex={selectedIndex}");
        }

        // Press Q to assign current selection as the Steel (push) target
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (buringsteel && metalTargets.Count > 0)
            {
                Transform chosen = metalTargets[selectedIndex];
                Debug.Log($"Chooser: assigning steel target index={selectedIndex} name={chosen.name}");
                Transform assigned = ConvertTileToPhysicsIfNeeded(chosen);
                steelTarget = assigned;
                CreateOrUpdatePersistentLine(ref steelPersistentLine, steelTarget, Color.red, steelLinePrefab);
                HighlightSelectedLine();
            }
        }

        // Press E to assign current selection as the Iron (pull) target
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (buringiron && metalTargets.Count > 0)
            {
                Transform chosen = metalTargets[selectedIndex];
                Debug.Log($"Chooser: assigning iron target index={selectedIndex} name={chosen.name}");
                Transform assigned = ConvertTileToPhysicsIfNeeded(chosen);
                ironTarget = assigned;
                CreateOrUpdatePersistentLine(ref ironPersistentLine, ironTarget, Color.green, ironLinePrefab);
                HighlightSelectedLine();
            }
        }

        // Confirm and exit chooser. If a burn type is active but no target assigned, assign current selection by default.
        if (Input.GetKeyDown(KeyCode.W))
        {
            isLineChooserActive = false;
            Time.timeScale = 1f;
            DestroyChooserLines();

            // Assign defaults if needed
            if (buringsteel && steelTarget == null && metalTargets.Count > 0)
            {
                Transform chosen = metalTargets[selectedIndex];
                Transform assigned = ConvertTileToPhysicsIfNeeded(chosen);
                steelTarget = assigned;
                CreateOrUpdatePersistentLine(ref steelPersistentLine, steelTarget, Color.red, steelLinePrefab);
            }
            if (buringiron && ironTarget == null && metalTargets.Count > 0)
            {
                Transform chosen = metalTargets[selectedIndex];
                Transform assigned = ConvertTileToPhysicsIfNeeded(chosen);
                ironTarget = assigned;
                CreateOrUpdatePersistentLine(ref ironPersistentLine, ironTarget, Color.green, ironLinePrefab);
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            // Cancel chooser — keep any already assigned persistent lines but remove chooser lines
            isLineChooserActive = false;
            Time.timeScale = 1f;
            DestroyChooserLines();
        }
    }

    void LateUpdate()
    {
        UpdateLines();
        HandleLineChooserInput();
    }
}

// Helper component attached to temporary tile target GameObjects so we know origin Tilemap and cell
public class TileTargetInfo : MonoBehaviour
{
    public Tilemap tilemap;
    public Vector3Int cell;
}
    
