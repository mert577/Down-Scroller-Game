using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCheck : MonoBehaviour
{
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
}
