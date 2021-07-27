using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadImage : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = Camera.main.WorldToScreenPoint(target.position + offset);
        transform.position = newPos;
    }
}
