using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager current;
    public GameObject screenFader;
    public Text healthBar;
    public GameObject deathMenu;
    public GameObject cameraObject;
    public Image reloadBar;
    public Animator flash;
    public Text highScoreText;
    public Text ScoreText;
    public Text EndgameScoreText;
    public GameObject pauseObject;
    public GameObject pauseBackground;
    public GameObject pausePanel;

    public GameObject animatedTextPrefab;

    bool pauseAnimation;

    public static bool isPaused;
    // Start is called before the first frame update
    void Awake()
    {

        current = this;


    }

    private void Start()
    {
        EventManager.current.onPlayerShoot += cameraShake;
    }
    

    void Update()
    {
        if (!pauseAnimation)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (!isPaused)
                    PauseMenuOn();
                else
                    PauseMenuOff();
            }
        }
    }



    public void DeathMenuEnter()
    {
        StartCoroutine(startDeathMenuEnter());
    }

    public void fadeOut()
    {
        LeanTween.alpha(screenFader.GetComponent<RectTransform>(), 0f, .5f).setEaseInCubic();
    }

    public void fadeIn()
    {
        LeanTween.alpha(screenFader.GetComponent<RectTransform>(), 1f, .5f).setEaseInCubic();
    }

    public void StartFadeSequence()
    {
        StartCoroutine(fadeSequence());
    }

    public void updateHealthBar(int health, int maxHealth)
    {
        healthBar.text = "Health: " + health + "/" + maxHealth;
    }

    public void TriggerFlash()
    {
        flash.SetTrigger("Flash");
    }

    public void SetHighScoreText(string s)
    {
        highScoreText.text = s;
    }

    public void SetScoreText(string s)
    {
        ScoreText.text = "Score:" + s;
    }

    public void SetEndGameScoreText(string s)
    {
        EndgameScoreText.text = s;
    }

    public static void startFading() { }

    public void CameraPanUp()
    {
        LeanTween.moveY(cameraObject, 0, 1f).setEaseOutCubic();
    }

    public void CameraPanUpHigh()
    {
        LeanTween.moveLocalY(cameraObject, 41, 1f).setEaseOutCubic();
    }

    public void setReloadBarFillAmount(float amount)
    {
        if (amount < 0.05f) amount = 0;
        reloadBar.fillAmount = amount;
    }


    public void cameraShake()
    {
        if (Random.Range(0f, 1f) > .5f)
            cameraObject.GetComponent<Animator>().SetTrigger("Shake");
        else
            cameraObject.GetComponent<Animator>().SetTrigger("Shake2");
    }

    public void StartAscend()
    {
        
        StartCoroutine(StartSequence());
    }

    public void Higher()
    {
        StartCoroutine(SecondStage());
    }

    IEnumerator fadeSequence()
    {
        fadeIn();
        yield return new WaitForSeconds(1.5f);
        EventManager.current.newLevelReady();
        fadeOut();
    }

    IEnumerator startDeathMenuEnter()
    {
        yield return new WaitForSeconds(1.5f);
        LeanTween.moveY(deathMenu, (Screen.height / 2), .8f).setEaseOutCubic();

    }


    IEnumerator StartSequence()
    {
        yield return StartCoroutine(ShowText("YOU..."));
        yield return StartCoroutine(ShowText("...MUST..."));
        yield return StartCoroutine(ShowText("...ASCEND!"));
 
    }


    IEnumerator SecondStage()
    {
        yield return StartCoroutine(ShowText("HIGHER..."));
        yield return StartCoroutine(ShowText("...AND..."));
        yield return StartCoroutine(ShowText("...HIGHER!"));



    }

    IEnumerator ShowText(string textToShow)
    {
        GameObject text = Instantiate(animatedTextPrefab,new Vector2(-300,540),Quaternion.identity,GameObject.Find("Canvas").transform);
        text.GetComponent<RectTransform>().SetAsFirstSibling();
        text.GetComponent<Text>().text = textToShow;
        LeanTween.moveX(text, 700, .5f).setEaseInOutExpo();
        yield return new WaitForSeconds(1f);
        LeanTween.moveX(text, 1800, .5f).setEaseInOutExpo();
        yield return new WaitForSeconds(.8f);
        Destroy(text);

    }

    public void PauseMenuOn()
    {

        if (isPaused) return;
        StartCoroutine(_pauseMenuOn());
        EventManager.current.Pause();
    }


    IEnumerator _pauseMenuOn()
    {
        pauseAnimation = true;
        pauseObject.SetActive(true);
        LeanTween.scale(pauseObject, Vector3.one, 0.4f).setEaseInCubic().setIgnoreTimeScale(true);
        
        yield return new WaitForSecondsRealtime(.3f);
        isPaused = true;
        Time.timeScale = 0;
        pauseAnimation = false;
    }
    IEnumerator _pauseMenuOff()
    {
        pauseAnimation = true;
        LeanTween.scale(pauseObject, Vector3.zero, 0.3f).setEaseInCubic().setIgnoreTimeScale(true);

        yield return new WaitForSecondsRealtime(.3f);
        pauseObject.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
        pauseAnimation = false;

    }
    public void PauseMenuOff()
    {
        if (!isPaused) return;
        StartCoroutine(_pauseMenuOff());
        EventManager.current.Resume();
    }
}
