using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    int CurrentHealth;

    public bool invincible;
    public static bool isDead;
    public GameObject deathParticles;
    public GameObject hurtparticles;

    public float invincibilityTime;


    void Start()
    {
        isDead = false;
        //CANI MAX CANA EŞİTLE
        CurrentHealth = maxHealth;
        UIManager.current.updateHealthBar(CurrentHealth, maxHealth);
    }

   
    void Update()
    {
        //CAN SIFIR YA DA AZSA ÖLME METODUNU ÇAĞIR
        if (CurrentHealth <= 0&&!isDead)
        {
            CurrentHealth = 0;
            Death();
        }
    }

    public void getHurt(int damage)
    {
        if (!invincible && !isDead)
        {
            StartCoroutine(Invincibility());
            CurrentHealth -= damage;
            GameObject p = Instantiate(hurtparticles, transform.position, Quaternion.identity);
            Destroy(p, .7f);
            UIManager.current.TriggerFlash();

        }
        UIManager.current.updateHealthBar(CurrentHealth ,maxHealth);
     

    }

    void Death()
    {
        EventManager.current.PlayerDeath();
        GameObject p = Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(p, 2f);
        isDead = true;

        foreach (Transform t in transform.GetChild(1))
        {
            transform.GetChild(1).GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        GetComponent<SpriteRenderer>().enabled = false;
        UIManager.current.DeathMenuEnter();
       // Destroy(this.gameObject);
    }

    IEnumerator Invincibility()
    {
        invincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        invincible = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            getHurt(1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            getHurt(1);
        }
    }

}
