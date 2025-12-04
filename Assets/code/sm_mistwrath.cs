using UnityEngine;

public class sm_mistwrath : MonoBehaviour
{
    public GameObject playerobject;
    public GameObject meobject;
    public Rigidbody2D rb;
    public float xSpeed = 10f;

    // cached collider refs
    private Collider2D playerCollider;
    private Collider2D meCollider;

    // cached positions (world-space)
    private float playerx;
    private float playery;
    private float mex;
    private float mey;

    void Start()
    {
    playerCollider = playerobject.GetComponent<Collider2D>();
    meCollider = meobject.GetComponent<Collider2D>();
    Vector2 playerPos = playerCollider.bounds.center;
    Vector2 mePos = meCollider.bounds.center;
    float playerx = playerPos.x;
    float playery = playerPos.y;
    float mex = mePos.x;
    float mey = mePos.y;
    }
    void Update()
    {
    if ((float)(mex-playerx)>0)
    rb.AddForce(Vector2.left * xSpeed);
    if ((float)(mex-playerx)<0)
    rb.AddForce(Vector2.right * xSpeed);
    }
}
