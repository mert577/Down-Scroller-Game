using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject blast;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<EnemyHealth>()!=null)
            collision.gameObject.GetComponent<EnemyHealth>().GetHurt(damage, collision.contacts[0].point);
            else
                collision.gameObject.transform.parent.parent.parent.GetComponent<EnemyHealth>().GetHurt(damage, collision.contacts[0].point);
            Destroy(this.gameObject);
        }

        else
        {
            GameObject b = Instantiate(blast, transform.position, Quaternion.identity);
            Destroy(b, 0.5f);
            Destroy(this.gameObject);
        }

    }
}
