using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
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


        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().getHurt(damage);
           
            Destroy(this.gameObject);
        }

        else
        {
            GameObject b = Instantiate(blast, transform.position, transform.rotation);
            Destroy(b, 0.5f);
            Destroy(this.gameObject);
        }

    }
}
