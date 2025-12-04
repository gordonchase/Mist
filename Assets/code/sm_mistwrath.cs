using UnityEngine;

public class sm_mistwrath : MonoBehaviour
{
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
    
    void Start()
    {
    Vector2 playerPos = playerobject.transform.position;
    Vector2 enemyPos = enemyobject.transform.position;
    rb = enemyobject.GetComponent<Rigidbody2D>();
    checkdistance = Vector2.Distance(playerPos, enemyPos);
    }
    void Update()
    {
    Vector2 playerPos = playerobject.transform.position;
    Vector2 enemyPos = enemyobject.transform.position;
    checkdistance = Vector2.Distance(playerPos, enemyPos);
    if (checkdistance<seedistance){
        if (playerPos.x-enemyPos.x>0 && canmove)
        {
        rb.AddForce(Vector2.right * speed, ForceMode2D.Force);
        }
        if (playerPos.x-enemyPos.x<0 && canmove)
        {
        rb.AddForce(Vector2.left * speed, ForceMode2D.Force);
        }
        if (playerPos.y-enemyPos.y>1 && isgrounded)
        {
        rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        isgrounded = false;
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
}
