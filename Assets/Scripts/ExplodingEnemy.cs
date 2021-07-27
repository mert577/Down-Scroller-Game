using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : MonoBehaviour
{
    Vector2 direction;
    public float Speed;
    Rigidbody2D rb;

    public GameObject projectile;
    public float projectileSpeed;
    [SerializeField] LayerMask lm;

    public float hitRadius;
    public AnimationCurve ac;
    Transform target;
    // Start is called before the first frame update
    private void Awake()
    {
        target = GameObject.Find("Player").transform;
    }
    void Start()
    {

        StartCoroutine(Cycle());
        rb = GetComponent<Rigidbody2D>();
  

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -12f, 12f), Mathf.Clamp(transform.position.y, -8f, 8f));

    }

    IEnumerator Shoot()
    {
        Vector2 newPos = target.position + (Vector3)Random.insideUnitCircle;
        bool isInside = Physics2D.OverlapCircle(newPos, 3f, lm);

        while (newPos.x < -12 || newPos.y < -8 || newPos.x > 12 || newPos.y > 8 || isInside)
        {
         
            newPos = target.position + (Vector3)Random.insideUnitCircle;
            isInside = Physics2D.OverlapCircle(newPos, 3f, lm);

        }

        LeanTween.move(gameObject,newPos, .8f).setEaseInOutBack();
        yield return new WaitForSeconds(.8f);


    }

    IEnumerator changeDirections()
    {
        float randomSeconds = Random.Range(2f, 2.7f);

       
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 newPos = target.position + (Vector3)Random.insideUnitCircle * 2f;
        bool isInside = Physics2D.OverlapCircle(newPos, 3f, lm);
        while (newPos.x < -12 || newPos.y < -8 || newPos.x > 12 || newPos.y > 8 || isInside)
        {
            randomDirection = Random.insideUnitCircle.normalized;
            newPos = target.position + (Vector3)Random.insideUnitCircle * 2f;
            isInside = Physics2D.OverlapCircle(newPos, 3f, lm);

        }
          LeanTween.move(gameObject, newPos, randomSeconds);
        float percent = 0;
        while (percent <randomSeconds)
        {
            percent += Time.deltaTime;
            Vector3 dir = newPos - (Vector2) transform.position;
            dir.Normalize();
            transform.GetChild(0).localPosition = projectileSpeed*  new Vector2(-dir.y,dir.x)   * ac.Evaluate(percent / randomSeconds);
            yield return null;

        }
        yield return new WaitForSeconds(.03f);
        if (Vector2.Distance(transform.position, target.position) <= 5f)
            yield return StartCoroutine(Shoot());


    }

    IEnumerator Cycle()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return StartCoroutine(changeDirections());
        }
    

        StartCoroutine(Cycle());


    }

    void TurnToTarget(Vector3 pos)
    {


        Vector2 dir = pos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().getHurt(1);
        }
    }

}
