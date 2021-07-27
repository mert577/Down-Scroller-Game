using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public static PlatformController current;

    public GameObject topPlatform;
    public GameObject bottomPlatform;
    public GameObject rightPlatform;
    public GameObject leftPlatform;

    public GameObject particle;

    private void Awake()
    {
        current = this;
       
    }



    // Start is called before the first frame update
    void Start()
    {
        EventManager.current.onFirstBossAttack += DisableTopAndBottom;
        EventManager.current.onFirstBossAttackEnd += EnableTopAndBottom;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DisableTopAndBottom()
    {
        topPlatform.SetActive(false);
        GameObject p = Instantiate(particle, topPlatform.transform.position, transform.rotation);
        Destroy(p, 0.5f);
        bottomPlatform.SetActive(false);
        GameObject p2 = Instantiate(particle, bottomPlatform.transform.position, transform.rotation);
        Destroy(p2, 0.5f);
    }



    public void EnableTopAndBottom()
    {
        topPlatform.SetActive(true);
        GameObject p = Instantiate(particle, topPlatform.transform.position, transform.rotation);
        Destroy(p, 0.5f);
        bottomPlatform.SetActive(true);
        GameObject p2 = Instantiate(particle, bottomPlatform.transform.position, transform.rotation);
        Destroy(p2, 0.5f);
    }


}
