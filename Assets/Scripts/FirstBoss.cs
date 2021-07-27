using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBoss : MonoBehaviour
{

    public GameObject projectile;
    public float Speed;

    States currentState;

    

    public float idleMoveRadius;

    public float projectileSpeed;

     float coolDown;

    bool isMoving;

    Rigidbody2D rb;

    enum States
    {
        Idling = 1,
        ConstantShooting =2,
        Attacking = 3

    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Idle());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = GameObject.FindWithTag("Player").transform.position;
        Vector3 attackDir = target - transform.position;
        attackDir.Normalize();
       if(currentState == States.ConstantShooting)
        {
            if (Time.time > coolDown + .089f)
            {
                float angle = Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg;
              
                GameObject p = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle-90));
                p.GetComponent<Rigidbody2D>().velocity = p.transform.up * projectileSpeed*1.3f;
                coolDown = Time.time;
                UIManager.current.cameraShake();
            }
        }
    }

    private void FixedUpdate()
    {

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -11f, 11f), Mathf.Clamp(transform.position.y, -7f, 7f));

    }

    IEnumerator Idle()
    {
        yield return new WaitForSeconds(.15f);
        // IDLE MOVEMENT
        for (int i = 0; i < 4; i++)
        {
            Vector3 randomDirection = Random.insideUnitCircle.normalized;
            randomDirection.Normalize();
            Vector3 newPos = transform.position + (randomDirection * idleMoveRadius);
            while (newPos.x<-11 || newPos.y<-7|| newPos.x>11 || newPos.y > 7) {

                randomDirection = Random.insideUnitCircle.normalized;
                newPos = (transform.position + (randomDirection * idleMoveRadius*.75f));


            }
 
            LeanTween.moveLocal(gameObject,newPos, .7f).setEaseInCubic();
            yield return new WaitForSeconds(.75f);

        }

        int attack =Random.Range(1,4);

        if (attack==1)
        {
            StartCoroutine(AttackOne());
        }
        else if(attack ==2)
        {
            StartCoroutine(AttackTwo());
        }
        else if (attack == 3)
        {
            StartCoroutine(AttackThree());
        }




    }

    IEnumerator AttackOne()
    {
     
        yield return new WaitForSeconds(0.1f);
        EventManager.current.FirstBossAttack();

        LeanTween.move(gameObject, Vector2.zero, 1f).setEaseInBack();

        yield return new WaitForSeconds(1f);

        //ROTATE AND SHOOT 5 BULLETS 3 TIMES FROM EACH SIDE
        

        for (int j = -5; j < 6; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject p = Instantiate(projectile, transform.position,  Quaternion.Euler(new Vector3(0, 0,(i * 120)+j*15)));
                p.GetComponent<Rigidbody2D>().velocity = p.transform.up * projectileSpeed;
                UIManager.current.cameraShake();
            }
            yield return new WaitForSeconds(0.15f);
        }
        LeanTween.rotate(gameObject, new Vector3(0, 0, 120f), .8f).setEaseInBounce(); ;
        yield return new WaitForSeconds(.8f);


        for (int j = -5; j < 6; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject p = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0, 0,60+(i * 120)+ j * 15)));
                p.GetComponent<Rigidbody2D>().velocity = p.transform.up * projectileSpeed;
                UIManager.current.cameraShake();
            }
            yield return new WaitForSeconds(0.15f);
        }
        LeanTween.rotate(gameObject, new Vector3(0, 0, 240f), .8f).setEaseInBounce(); ;
        yield return new WaitForSeconds(.8f);

        for (int j = -5; j < 6; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject p = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0, 0, (i * 120)+ j * 15)));
                p.GetComponent<Rigidbody2D>().velocity = p.transform.up * projectileSpeed;
                UIManager.current.cameraShake();
            }
            yield return new WaitForSeconds(0.15f);
        }
        LeanTween.rotate(gameObject, new Vector3(0, 0, 360f), .8f).setEaseInBounce(); ;
        yield return new WaitForSeconds(.8f);


        for (int j = -5; j < 6; j++)
        {
            for (int i = 0; i < 3; i++)
            {

                GameObject p = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0, 0,60+(i * 120)+ j * 15)));
                p.GetComponent<Rigidbody2D>().velocity = p.transform.up * projectileSpeed;
                UIManager.current.cameraShake();
            }
            yield return new WaitForSeconds(0.15f);
        }



        EventManager.current.FirstBossAttackEnd();
        StartCoroutine(Idle());

    }


    IEnumerator AttackTwo()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 randomDirection = Random.insideUnitCircle.normalized;
            randomDirection.Normalize();
            Vector3 newPos = transform.position + (randomDirection * idleMoveRadius);
            while (newPos.x < -11 || newPos.y < -7 || newPos.x > 11 || newPos.y > 7)
            {

                randomDirection = Random.insideUnitCircle.normalized;
                newPos = (transform.position + (randomDirection * idleMoveRadius));


            }

            LeanTween.moveLocal(gameObject, newPos, .7f).setEaseInBack();
         
            
            yield return new WaitForSeconds(.7f);
            currentState = States.ConstantShooting;
            coolDown = 0;
            yield return new WaitForSeconds(.3f);
            currentState = States.Idling;
        }
            StartCoroutine(Idle());
    }


    IEnumerator AttackThree()
    {
        float attack = Random.value;

        if (attack > .5f)
        {
            LeanTween.move(gameObject, new Vector3(-10, 7, 0), 1f).setEaseInOutCirc();
            yield return new WaitForSeconds(1f);
            LeanTween.move(gameObject, new Vector3(-10, -7, 0), 2f);

           
               for (int j = 0; j < 4; j++)
               {

                    for (int i = -1; i < 2; i++)

                    {
                          GameObject p = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, (30 * i) + 90));

                           p.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0, 0, (30 * i)) * Vector2.right * projectileSpeed;
                           UIManager.current.cameraShake();
                    }
                yield return new WaitForSeconds(.5f);
                
               }
            
            
        }
        else
        {
            LeanTween.move(gameObject, new Vector3(10, 7, 0), 1f).setEaseInOutCirc();
            yield return new WaitForSeconds(1f);
            LeanTween.move(gameObject, new Vector3(10, -7, 0), 2f);


              for (int j = 0; j < 4; j++)
            {


                for (int i = -1; i < 2; i++)
                {
                    GameObject p = Instantiate(projectile, transform.position,  Quaternion.Euler(0, 0, (30 * i)-90));

                    p.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0, 0, (30 * i)) * -Vector2.right * projectileSpeed;
                    UIManager.current.cameraShake();
                }
                yield return new WaitForSeconds(.5f);
            }

        }



        StartCoroutine(Idle());

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().getHurt(1);
        }
    }

    private void OnDestroy()
    {
        EventManager.current.FirstBossAttackEnd();

    }
}
