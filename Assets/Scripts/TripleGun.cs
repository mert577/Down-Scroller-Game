using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleGun : RangedWeapon
{
  

    public  override void Shoot()
    {
        if (!isPickUp)
        {
            Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            dir.Normalize();
            for (int i = -1; i < 2; i++)
            {
                 
                GameObject p = Instantiate(projectile, nozzle.position, transform.rotation*Quaternion.Euler(0, 0, 30 * i) );

                p.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0, 0, (30 * i)) * dir * projectileSpeed;
            }

        }
    }
}
