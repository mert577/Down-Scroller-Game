using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerScript : MonoBehaviour

{
    public GameObject transitionObject;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            EventManager.current.FirstStageStart();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {

        }
        else if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            EventManager.current.GameOpen();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator transtition()
    {
        LeanTween.moveY(transitionObject, -500, 1f).setEaseInCubic();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);

    }
    public void transitionToGameScene()
    {
        StartCoroutine(transtition());
    }
}
