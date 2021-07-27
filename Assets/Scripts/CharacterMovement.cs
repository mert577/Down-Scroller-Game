using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    Rigidbody2D rb;
    public float speed;
    public float jumpForce;
    public float longJumpForce;

    public LayerMask groundLayer;
    public float groundCheckRadius;
    public bool isGrounded;
    private int jumpCount;
    public int jumpNumber;
    public float fallingMultiplier;
    private float gravityScale;
    public Transform groundCheck;
    public Transform gunHolder;

    public float maxFallVel;
    private bool isJumping;
    public float jumpTime;
    private float jumpTimeCounter;

    public float ascendForce;

    float coolDownTimer;
    bool isAscending;
   [SerializeField]  GameObject currentWeapon;
    public List<GameObject> weapons = new List<GameObject>();
    public GameObject jumpParticle;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = jumpNumber;
        gravityScale = rb.gravityScale;
       
        coolDownTimer = Time.time;
       }

    // Update is called once per frame
    void Update()
    {
        //YERE DEĞİNCE JUMPLARI RESETLE
        if (isGrounded)
        {
            jumpCount = jumpNumber;
        }
        
        //ZIPLAMA
        if(Input.GetButtonDown("Jump")&& jumpCount > 0&&!PlayerHealth.isDead&& WaveController.currentState!= WaveController.GameState.Null)
        {
            isJumping = true;
            rb.velocity = Vector2.up * jumpForce;
            GameObject p = Instantiate(jumpParticle, transform.position+Vector3.up*0.4f, Quaternion.identity);
            Destroy(p, 0.5f);
            jumpCount--;
            jumpTimeCounter = jumpTime;
        }


        if (Input.GetButton("Jump")&& isJumping)
        {
            if (jumpTimeCounter > 0f)
            {
                rb.velocity += Vector2.up * longJumpForce*Time.deltaTime;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

        //ATEŞ ETME
        if (Input.GetMouseButton(0) && !PlayerHealth.isDead && WaveController.currentState != WaveController.GameState.Teleporting && currentWeapon != null && WaveController.currentState != WaveController.GameState.Null)
        {
            if (Time.time > coolDownTimer + (1f / currentWeapon.GetComponent<RangedWeapon>().attackSpeed))
            {

                EventManager.current.PlayerShoot();
             
                coolDownTimer = Time.time;

            }
            else if (currentWeapon != null)
            {
               
                UIManager.current.setReloadBarFillAmount((coolDownTimer + (1 / currentWeapon.GetComponent<RangedWeapon>().attackSpeed) - Time.time) / (1 / currentWeapon.GetComponent<RangedWeapon>().attackSpeed));

            }
        }
        if(currentWeapon != null)
        {

        
        if (Time.time < coolDownTimer + (1f / currentWeapon.GetComponent<RangedWeapon>().attackSpeed))
        {
            float amount = (coolDownTimer + (1 / currentWeapon.GetComponent<RangedWeapon>().attackSpeed) - Time.time) / (1 / currentWeapon.GetComponent<RangedWeapon>().attackSpeed);
                if (amount < 0.1f) amount = 0f;

             UIManager.current.setReloadBarFillAmount(amount);

        }
        }

    }

    private void FixedUpdate()
    {
        //YERE DEĞMEYİ KONTROL ET
        bool result = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius,groundLayer);
        if (!isGrounded && result)
        {
            GameObject p = Instantiate(jumpParticle, transform.position + Vector3.up * -0.4f, Quaternion.identity);
            Destroy(p, 0.5f);

        }
        isGrounded = result;
        //SAĞA SOLA HAREKET
        float currentSpeed = Input.GetAxisRaw("Horizontal") * speed;

        if (!PlayerHealth.isDead && WaveController.currentState != WaveController.GameState.Null&& !isAscending)
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);


        //DAHA HIZLI DÜŞ
        if (rb.velocity.y < 0)
            rb.gravityScale = gravityScale * fallingMultiplier;
        else
            rb.gravityScale = gravityScale;

        //MAX FALL VELOCİTYTİSNE CLAMPLA
        if (rb.velocity.y < 0 && rb.velocity.y < -maxFallVel)
        {
            rb.velocity = new Vector2(rb.velocity.x, -maxFallVel);
        }

        if (WaveController.currentState == WaveController.GameState.Teleporting ||WaveController.currentState == WaveController.GameState.NextTransition)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            SetInvisible();
        }

      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gun"))
        {
            GameObject newGun = collision.gameObject;
            if (currentWeapon != null)
                Destroy(currentWeapon.gameObject);
            newGun.GetComponent<RangedWeapon>().GetPickedUp(gunHolder);

            currentWeapon = newGun;
        }
        else if (collision.gameObject.CompareTag("Portal"))
        {

            EventManager.current.FinishStage();
        }
        else if(collision.gameObject.CompareTag("Beam"))
        {
          //  ascend();

        }

    }

    
    void ascend()
     {
        rb.isKinematic = true;
        isAscending = true;
        rb.velocity = Vector2.up * ascendForce;
     }

    public void SetInvisible()
    {
        foreach (Transform t in transform.GetChild(1))
        {
            transform.GetChild(1).GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
     
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void SetVisible()
    {
        transform.position =new Vector2(0,-7f);
        rb.isKinematic = false;
        foreach (Transform t in transform.GetChild(1))
        {
            transform.GetChild(1).GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        GetComponent<SpriteRenderer>().enabled = true;
    }

 
}
