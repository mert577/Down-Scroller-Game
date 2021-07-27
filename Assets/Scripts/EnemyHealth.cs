using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int maxHealth;
    int currentHealth;
    public GameObject hurtParticle;
    public GameObject deathParticle;

    bool isBoss;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.current.enemySpawn();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)

        {
           
            Die();
        }
    }

    void Die()
    {

        EventManager.current.EnemyDeath();
        GameObject p = Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(p, 3f);

        Destroy(this.gameObject);
    }



    public void GetHurt(int damage, Vector2 pos)
    {
        currentHealth -= damage;
        if (currentHealth > 0) {
       GameObject p = Instantiate(hurtParticle, pos, Quaternion.identity);
        Destroy(p, 0.5f);
        }
    }
}
