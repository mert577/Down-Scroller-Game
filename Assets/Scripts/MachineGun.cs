using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : RangedWeapon
{
    // Start is called before the first frame update
    public override void Shoot()
    {
        if (!isPickUp)
        {
            Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            dir.Normalize();
            GameObject p = Instantiate(projectile, nozzle.position, transform.rotation*Quaternion.Euler(0,0,Random.Range(-18f,18f)));
            p.GetComponent<Rigidbody2D>().velocity = p.transform.right * projectileSpeed;
        }
    }

}
