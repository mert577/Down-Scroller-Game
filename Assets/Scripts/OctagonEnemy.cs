using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctagonEnemy : MonoBehaviour
{
    Vector2 direction;
    public float Speed;
    Rigidbody2D rb;
    bool isShooting;
    [SerializeField] LayerMask lm;

    public LineRenderer[] lineRenderers;
    public float animationSpeed;
    public AnimationCurve acurve;
    public int Damage;

    public GameObject particle;
    public float laserWidth;
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
        for (int i = 0; i < 6; i++)
        {
            LeanTween.moveLocal(gameObject, (Vector2) transform.position+ (Random.insideUnitCircle*.1f), .1f).setEaseShake();
            yield return new WaitForSeconds(.1f);
        }
        yield return new WaitForSeconds(.2f);

        RaycastHit2D[] results = new RaycastHit2D[4];

        results[0] = Physics2D.Raycast(transform.position, Vector2.right, 30, LayerMask.GetMask("Zemin"));
        results[1] = Physics2D.Raycast(transform.position, -Vector2.right, 30, LayerMask.GetMask("Zemin"));
        results[2] = Physics2D.Raycast(transform.position, Vector2.up, 30, LayerMask.GetMask("Zemin"));
        results[3] = Physics2D.Raycast(transform.position, -Vector2.up, 30, LayerMask.GetMask("Zemin"));
        for (int i = 0; i < 4; i++)
        {
            lineRenderers[i].SetPosition(0, transform.position);
            lineRenderers[i].SetPosition(1, results[i].point);
            GameObject p = Instantiate(particle, results[i].point, Quaternion.identity);
            Destroy(p, 1.2f);

        }

        float percent = 0;
        while (percent < 1f)
        {
            percent += Time.deltaTime * animationSpeed;

            if (percent >= .33f && percent <= .8f)
            {
                RaycastHit2D[] playeRresults = new RaycastHit2D[4];

                playeRresults[0] = Physics2D.Raycast(transform.position, Vector2.right, 30, LayerMask.GetMask("Player"));
                playeRresults[1] = Physics2D.Raycast(transform.position, -Vector2.right, 30, LayerMask.GetMask("Player"));
                playeRresults[2] = Physics2D.Raycast(transform.position, Vector2.up, 30, LayerMask.GetMask("Player"));
                playeRresults[3] = Physics2D.Raycast(transform.position, -Vector2.up, 30, LayerMask.GetMask("Player"));

                foreach(RaycastHit2D hit in playeRresults)
                {
                    if (hit)
                    {
                        if (hit.collider.gameObject.GetComponent<PlayerHealth>() != null)
                        hit.collider.gameObject.GetComponent<PlayerHealth>().getHurt(Damage);
                        
                    }
                }



            }
            
            for (int i = 0; i < 4; i++)
            {
                lineRenderers[i].SetWidth(laserWidth*acurve.Evaluate(percent), laserWidth * acurve.Evaluate(percent));
            }  
            yield return null;
        }
     

        isShooting = false;
        yield return new WaitForSeconds(.5f);
      
    }

    IEnumerator changeDirections()
    {
        float randomSeconds = Random.Range(0.5f, 1f);


        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 newPos = (Vector2)transform.position + randomDirection * Speed * randomSeconds;
        bool isInside = Physics2D.OverlapCircle(newPos, 1f, lm);
        while (newPos.x < -12 || newPos.y < -8 || newPos.x > 12 || newPos.y > 8 || isInside)
        {
            randomDirection = Random.insideUnitCircle.normalized;
            newPos = (Vector2)transform.position + randomDirection * Speed * randomSeconds;
            isInside = Physics2D.OverlapCircle(newPos, 0.1f, lm);

        }
        LeanTween.move(gameObject, newPos, randomSeconds).setEaseInOutCirc();

        yield return new WaitForSeconds(randomSeconds);
    }

    IEnumerator Cycle()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return StartCoroutine(changeDirections());
        }

        yield return StartCoroutine(Shoot());


        StartCoroutine(Cycle());


    }
}
