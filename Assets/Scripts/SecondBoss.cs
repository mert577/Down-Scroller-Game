using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBoss : MonoBehaviour
{
    public GameObject projectile;
    public GameObject particle;
    public float Speed;

    States currentState;



    public float idleMoveRadius;

    public float projectileSpeed;

    float coolDown;

    bool isMoving;

    public GameObject[] quadrants;
    public Transform[] holders;
    public GameObject holder;
    LTDescr xMove;
    LTDescr yMove;
    Rigidbody2D rb;
    public AnimationCurve ac;
    LineRenderer ln;

    GameObject player;
    public float animationSpeed;
    public float laserWidth;
   int attack;
    public BoxCollider2D laserHitbox;
    enum States
    {
        Idling = 1,
        ConstantShooting = 2,
        Attacking = 3

    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        ln = GetComponent<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

        attack = 1;
        StartCoroutine(Idle());
     
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void FixedUpdate()
    {

      //  transform.position = new Vector2(Mathf.Clamp(transform.position.x, -11f, 11f), Mathf.Clamp(transform.position.y, -7f, 7f));

    }

    IEnumerator Idle()
    {
        AttachQuadrants();
        LeanTween.moveLocal(gameObject, new Vector2(-10f, 6f), 1f);
        LeanTween.rotateAroundLocal(holder, Vector3.forward, 360f, 2f).setEaseInOutCirc().setLoopClamp();

        yield return new WaitForSeconds(1f);
        xMove = LeanTween.moveLocalX(gameObject, 10f, 4f).setEaseInOutSine().setLoopPingPong();
        yMove = LeanTween.moveLocalY(gameObject, 5f, .8f).setEaseInOutSine().setLoopPingPong();

        yield return new WaitForSeconds(3f);

       

        if (attack%3== 1)
        {
            StartCoroutine(AttackOne());
         
        }
        else if (attack % 3 == 2)
        {
            StartCoroutine(AttackTwo());
          
        }
        else if (attack % 3 == 0)
        {
            StartCoroutine(AttackThree());
           
        }
        



    }

    IEnumerator AttackOne()
    {

        yield return new WaitForSeconds(0.1f);
        LeanTween.cancel(gameObject);
        LeanTween.moveY(gameObject, 3f, 1f).setEaseInCirc();
        yield return new WaitForSeconds(1f);
   
      
         
         
            LeanTween.moveX(gameObject,-11,.5f).setEaseInBack();
            yield return new WaitForSeconds(.5f);
           
       
            float percent=0;
            
            while (percent < 1f)
            {
            RaycastHit2D hitGround = Physics2D.Raycast(transform.position, Vector2.down, 30f, LayerMask.GetMask("Zemin"));
            ln.SetPosition(0, transform.position);
            ln.SetPosition(1, hitGround.point);
            percent += Time.deltaTime * animationSpeed;

                if (percent >= .24f && percent <= .9f)
                {
                    laserHitbox.enabled = true;
                }
                else
                {
                    laserHitbox.enabled = false;
                }

                if (laserHitbox.enabled&& !isMoving)
                {
                LeanTween.moveX(gameObject, 11, (1f / animationSpeed)*0.77f).setEaseInOutSine();
                isMoving = true;
                 }
            
               ln.SetWidth(laserWidth * ac.Evaluate(percent), laserWidth * ac.Evaluate(percent));
               
                yield return null;
            }
        isMoving = false;
        yield return new WaitForSeconds(.3f);
   

         percent = 0;

        while (percent < 1f)
        {
            RaycastHit2D hitGround = Physics2D.Raycast(transform.position, Vector2.down, 30f, LayerMask.GetMask("Zemin"));
            ln.SetPosition(0, transform.position);
            ln.SetPosition(1, hitGround.point);
            percent += Time.deltaTime * animationSpeed;

            if (percent >= .24f && percent <= .9f)
            {
                laserHitbox.enabled = true;
            }
            else
            {
                laserHitbox.enabled = false;
            }

            if (laserHitbox.enabled && !isMoving)
            {
                LeanTween.moveX(gameObject, -11f, (1f / animationSpeed) * 0.77f).setEaseInOutSine();
                isMoving = true;
            }

            ln.SetWidth(laserWidth * ac.Evaluate(percent), laserWidth * ac.Evaluate(percent));

            yield return null;
        }
        yield return new WaitForSeconds(.07f);

        isMoving = false;
        attack++;
        StartCoroutine(Idle());
    }


    


    IEnumerator AttackTwo()
    {
        LeanTween.cancel(gameObject);
        LeanTween.move(gameObject, Vector2.zero, 1f).setEaseInOutSine();

        yield return new WaitForSeconds(1f);

        //ROTATE AND SHOOT 5 BULLETS 3 TIMES FROM EACH SIDE


        for (int j = 0; j < 50; j++)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject p = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0, 0, (i *36)+ (j*15f) )));
                p.GetComponent<Rigidbody2D>().velocity = p.transform.up * projectileSpeed;
                UIManager.current.cameraShake();
            }
            yield return new WaitForSeconds(.15f);
        }
   
        yield return new WaitForSeconds(.8f);
        attack++;
        StartCoroutine(Idle());
    }


    IEnumerator AttackThree()
    {
        yield return new WaitForSeconds(.2f);
        LeanTween.cancel(holder);
        holder.transform.rotation = Quaternion.identity;
        LeanTween.cancel(gameObject);
        LeanTween.move(gameObject, Vector2.zero, 1f).setEaseInOutSine();
       
        yield return new WaitForSeconds(1f);
        Vector2[] poses = new Vector2[4];
        for (int i = 0; i < 4; i++)
        {
            poses[i] = holders[i].transform.position;

        }
        for (int i =0;i<4;i++)
        {
            LeanTween.move(holders[i].gameObject, new Vector2(-10f+(i*20f/3f),6f), .3f).setEaseInOutCirc();
            yield return new WaitForSeconds(.3f);
        }

        for (int i = 0; i < 4; i++)
        {
            
            Vector2 dir = (player.transform.position - holders[i].position);
          dir.Normalize();
            var hit = Physics2D.Raycast(holders[i].position, dir, 30f, LayerMask.GetMask("Zemin"));
            LeanTween.move(holders[i].gameObject, hit.point, .6f).setEaseInBack().setLoopPingPong().setLoopOnce();

            UIManager.current.cameraShake();
            yield return new WaitForSeconds(.6f);
            GameObject p = Instantiate(particle, hit.point, Quaternion.identity);
            Destroy(p, .4f);
        }

     

        LeanTween.move(holders[0].gameObject, poses[0],.5f).setEaseInBack();
        LeanTween.move(holders[1].gameObject, poses[1], .5f).setEaseInBack();
        LeanTween.move(holders[2].gameObject, poses[2], .5f).setEaseInBack();
        LeanTween.move(holders[3].gameObject, poses[3], .5f).setEaseInBack();
        for (int i = 0; i < 4; i++)
        {
            quadrants[i].GetComponent<BoxCollider2D>().enabled = false;
        }

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < 4; i++)
        {
            quadrants[i].GetComponent<BoxCollider2D>().enabled = true;
        }

        for (int i = 0; i < 4; i++)
        {
            poses[i] = holders[i].transform.position;

        }
        for (int i =0;i<4;i++)
        {
            LeanTween.move(holders[i].gameObject, new Vector2(-10f+(i*20f/3f),6f), .3f).setEaseInOutCirc();
            yield return new WaitForSeconds(.3f);
        }

        for (int i = 0; i < 4; i++)
        {
            Vector2 dir = player.transform.position - holders[i].position;
            dir.Normalize();
            var hit = Physics2D.Raycast(holders[i].position, dir, 30f, LayerMask.GetMask("Zemin"));
            LeanTween.move(holders[i].gameObject,hit.point , .6f).setEaseInBack().setLoopPingPong().setLoopOnce();
            yield return new WaitForSeconds(.6f);
            GameObject p = Instantiate(particle, hit.point, Quaternion.identity);
            Destroy(p, .4f);
            UIManager.current.cameraShake();
        }

        LeanTween.move(holders[0].gameObject, poses[0],.5f).setEaseInBack();
        LeanTween.move(holders[1].gameObject, poses[1], .5f).setEaseInBack();
        LeanTween.move(holders[2].gameObject, poses[2], .5f).setEaseInBack();
        LeanTween.move(holders[3].gameObject, poses[3], .5f).setEaseInBack();
        for (int i = 0; i < 4; i++)
        {
            quadrants[i].GetComponent<BoxCollider2D>().enabled = false;
        }

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < 4; i++)
        {
            quadrants[i].GetComponent<BoxCollider2D>().enabled = true;
        }
        attack++;
        yield return  StartCoroutine(Idle());

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

    void AttachQuadrants()
    {
        for(int i = 0; i < 4; i++)
        {

            quadrants[i].GetComponent<Rigidbody2D>().isKinematic = true;
            
            quadrants[i].transform.SetParent(holders[i]);
        }
    }

    void DettachQuadrant(int index)
    {
        quadrants[index].transform.SetParent(null);
       
        

    }
}
