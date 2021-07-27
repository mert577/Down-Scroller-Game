using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(beamSequence());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator beamSequence()
    {
        LeanTween.scale(gameObject, new Vector2(60f, .3f), 1.8f).setEaseInOutCirc();
        yield return new WaitForSeconds(1.8f);

        LeanTween.scale(gameObject, new Vector2(60f, 2.9f), .3f).setEaseInOutCirc();
        yield return new WaitForSeconds(.3f);
        LeanTween.scale(gameObject, new Vector2(60f, 3.2f), .3f).setEaseInCubic().setLoopPingPong();


    }
}
