using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrimissileEnemy : MonoBehaviour
{
    public float speed;
    public Transform target;

    Vector3 targetPos;
    Rigidbody2D rb;

    public float maxChaseRadius;
    public float minChaseRadius;
    public float easing;

   [SerializeField] States state;
    Vector2 attackDir;
    float coolDownTimer;
    float coolDownTimerShooting;
    public GameObject projectile;
    public float projectileSpeed;

    Vector3 offset;

 
    enum States
    {
        Idling =1,
        Following =2,
        Backpedaling=3

    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
        state = States.Following;
        coolDownTimer = Time.time;
        coolDownTimerShooting = Time.time+Random.Range(-1f,1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetPos = target.position + offset;

        targetPos = new Vector2(Mathf.Clamp(targetPos.x, -11f, 11f), Mathf.Clamp(targetPos.y, -7f, 7f));
       transform.position= new Vector2(Mathf.Clamp(transform.position.x, -12f, 12f), Mathf.Clamp(transform.position.y, -8f, 8f));

        attackDir = target.position - transform.position;
        attackDir.Normalize();
        if (Time.time > coolDownTimer + .8f)
        {
            Vector3 add = Random.onUnitSphere * 7f;
            add = new Vector2(add.x, Mathf.Abs(add.y));
            offset = add;
         
            coolDownTimer = Time.time;

        }

    
        if (Time.time> coolDownTimerShooting+3f) {
            GameObject p=   Instantiate(projectile, transform.position, transform.rotation);
            p.GetComponent<Rigidbody2D>().velocity = attackDir * projectileSpeed;
            coolDownTimerShooting = Time.time+Random.Range(-0.6f, .6f);
        }

     
        TurnToTarget();

       

        float distance = Vector2.Distance(targetPos, transform.position);
        float playerDistance = Vector2.Distance(target.position, transform.position);
        if (distance < minChaseRadius || playerDistance < 2f)
        {
            state = States.Backpedaling;
        }

        else if (distance >= maxChaseRadius)
        {
            state = States.Following;
        }

        else
        {
            state = States.Idling;
        }
      
        
        


        if(state == States.Following)
        {
            GetCloseToTarget();
        }
        else if(state == States.Backpedaling)
        {
            GetAwayFromTarget();
        }
        else if(state== States.Idling)
        {
            rb.velocity = Vector2.zero;
        }


        
           
    }


    void TurnToTarget()
    {
       

        Vector2 dir = target.position- transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle-90);
  
    }

    void GetCloseToTarget()
    {
        Vector2 dir = target.position+offset - transform.position;

 
        
        rb.velocity = dir * speed;

    }

    void GetAwayFromTarget()
    {
        Vector2 dir = target.position  - transform.position;
        
        rb.velocity = dir * -speed*3f;

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(targetPos, 1f);
    }

}
