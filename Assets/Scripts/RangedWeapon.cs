using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    public GameObject projectile;
    public float attackSpeed;
    public float projectileSpeed;

    public bool isPickUp;
    
    float coolDownTimer;
    [SerializeField] protected Transform nozzle;


    // Start is called before the first frame update
    void Awake()
    {

        isPickUp = true;
   
      
    }
    private void Start()
    {
        EventManager.current.onPlayerShoot += Shoot;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isPickUp)
        {
            Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public virtual void Shoot()
    {
        if (!isPickUp)
        {
            Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            dir.Normalize();
            GameObject p = Instantiate(projectile, nozzle.position, transform.rotation);
            p.GetComponent<Rigidbody2D>().velocity = dir * projectileSpeed;
        }
    }


    public void GetPickedUp(Transform gunHolder)
    {
        isPickUp = false;
        GetComponent<Collider2D>().enabled = false;
        transform.position = gunHolder.position;
        transform.parent = gunHolder;
        transform.localScale = new Vector2(0.47f, .24f);
    }
    

    private void OnDestroy()
    {
        EventManager.current.onPlayerShoot -= Shoot;
    }
}
