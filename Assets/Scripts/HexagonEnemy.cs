using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonEnemy : MonoBehaviour
{
    Vector2 direction;
    public float Speed;
    Rigidbody2D rb;
    bool isShooting;
    public GameObject projectile;
    public float projectileSpeed;
    [SerializeField] LayerMask lm;
    // Start is called before the first frame update
    void Start()
    {
        isShooting = false;
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
        isShooting = true;
        GetComponent<Animator>().SetTrigger("Shoot");
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 6; i++)
        {
            GameObject p = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0,0,i*60)));
            p.GetComponent<Rigidbody2D>().velocity = p.transform.up * projectileSpeed;
            
        }
        isShooting = false;
        yield return new WaitForSeconds(.5f);
    }

    IEnumerator changeDirections()
    {
        float randomSeconds = Random.Range(2f, 2.7f);
        
        
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 newPos = (Vector2) transform.position  + randomDirection * Speed * randomSeconds;
        bool isInside = Physics2D.OverlapCircle(newPos, 1f, lm);
        while (newPos.x<-12 || newPos.y<-8|| newPos.x>12 || newPos.y > 8||isInside)
        {
            randomDirection = Random.insideUnitCircle.normalized;
            newPos = (Vector2) transform.position  + randomDirection * Speed * randomSeconds;
            isInside = Physics2D.OverlapCircle(newPos, 0.1f, lm);

        }
        LeanTween.move(gameObject, newPos, randomSeconds).setEaseInOutQuad();
        
        yield return new WaitForSeconds(randomSeconds);
    }

    IEnumerator Cycle()
    {
        for(int i = 0; i < 3; i++)
        {
           yield return StartCoroutine(changeDirections());
        }

        yield return StartCoroutine(Shoot());


        StartCoroutine(Cycle());
        
       
    }
}
