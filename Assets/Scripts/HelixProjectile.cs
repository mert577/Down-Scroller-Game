using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixProjectile : EnemyProjectile
{
    public float frequency;
    public float magnitude;
    float speed;
    float angle;
    Rigidbody2D rb;
    public float counter;
    Quaternion rot;
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
       
    }
    void Start()
    {
        rot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.up * speed  ;

    }

    private void FixedUpdate()
    {
        counter += Time.deltaTime;
        float rotation = -Mathf.Sin(counter*frequency);

        transform. rotation = rot*Quaternion.Euler(0,0,rotation*magnitude);
    }

   public void setSpeed(float _speed)
    {
        speed = _speed;
        rb.velocity = transform.right * speed;
    }
   
}
