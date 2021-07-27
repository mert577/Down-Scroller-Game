using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public float speed;
    public Transform target;
    Rigidbody2D rb;
    public GameObject spriteHolder;
    public float floatingRange;
    public float floatSpeed;

    Vector3 targetPos;
    float coolDownTimer;
    public int hasar;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        coolDownTimer = Time.time;
        LeanTween.moveLocalY(spriteHolder, -floatingRange,floatSpeed).setEaseInOutCubic().setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > coolDownTimer + .25f)
        {

            targetPos = target.position+ (Vector3) Random.insideUnitCircle*6f;
            coolDownTimer = Time.time;

        }
       

    }

    private void FixedUpdate()
    {
        Vector2 dir =  targetPos -transform.position;
        dir.Normalize();
        rb.velocity = dir * speed;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().getHurt(hasar);
        }
    }

}
