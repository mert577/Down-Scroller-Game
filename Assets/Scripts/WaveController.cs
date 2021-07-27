using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WaveController : MonoBehaviour
{
    public enum GameState
    {
        Fighting = 0,
        Teleporting = 1,
        ReadyToSpawn=3,
        EnemySpawning = 2,
        Null= 4,
        BossStage=5,
        ReadyForNextScene=6,
        NextTransition = 7,

    }

    public static int EnemiesAlive;

    public GameObject[] enemyList;
    public GameObject[] gunList;
    public GameObject[] layouts;
    public GameObject[] bossLayouts;
    public GameObject[] bossList;

    public static GameState currentState= GameState.Null;

    public static int roomNumber=0;

    public List<Transform> spawnPositions;

    public GameObject spawningVFX;
    public GameObject bossSpawningVFXEnd;
    public GameObject bossSpawningVFX;

    public GameObject teleporting;
    bool teleportStarted;

    public static int Score;
    public Transform gunPos;
    public GameObject playerSuckedIn;

     [SerializeField] GameObject beam;
    GameObject currentLayout;
    // Start is called before the first frame update
    void Awake()
    {
        EventManager.current.onEnemyDeath += OnEnemyDeath;
        EventManager.current.onEnemySpawn += OnEnemySpawn;
        EventManager.current.onNewLevelReady += TrasnslateToNextLevel;
        EventManager.current.onFinishStage += Transition;
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }
    }

    private void Start()
    {
        currentState = GameState.Null;
        EnemiesAlive=0;
        Score = 0;
        roomNumber = 0;
        UIManager.current.CameraPanUp();
        int randIndex = Random.Range(0, layouts.Length);

        currentLayout = Instantiate(layouts[randIndex], Vector2.zero,Quaternion.identity);
        spawnPositions = layouts[randIndex].GetComponent<LevelLayout>().spawnPos;
        gunPos = layouts[randIndex].GetComponent<LevelLayout>().GunPos;

        StartCoroutine(startGame());

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            EventManager.current.FirstStageStart();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            EventManager.current.SecondStageStart();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            EventManager.current.GameOpen();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", Score);
        }

        UIManager.current.SetScoreText(Score.ToString());
        UIManager.current.SetEndGameScoreText("Your Score   :"+Score);
        UIManager.current.SetHighScoreText("Highest Score:" + PlayerPrefs.GetInt("HighScore"));
    
        if (currentState== GameState.ReadyToSpawn)
        {
            if ((roomNumber + 1 == 6))
            {              
                StartCoroutine(SpawnBoss(bossList[0]));
            }
            else  StartCoroutine(SpawnEnemies(6));
        }
       
       
        if(EnemiesAlive==0 && currentState== GameState.Fighting&&!teleportStarted)
        {
            teleportStarted = true;
            Score += 50;
      

            StartCoroutine(TeleportOut());
        }
        else if((EnemiesAlive == 0 && currentState == GameState.BossStage && !teleportStarted)) {
            StartCoroutine(beamUp());
           teleportStarted = true;
        }
       
    }

    void changeLayout(int index)
    {
        Destroy(currentLayout);
       
        currentLayout = Instantiate(layouts[index], Vector2.zero, Quaternion.identity);
        spawnPositions = layouts[index].GetComponent<LevelLayout>().spawnPos;
        gunPos= layouts[index].GetComponent<LevelLayout>().GunPos;

    }
    void changeLayoutBoss()
    {
        Destroy(currentLayout);

        currentLayout = Instantiate(bossLayouts[0]);

    }


    public void OnEnemySpawn()
    {
        EnemiesAlive += 1;
        
    }

    public void OnEnemyDeath()
    {
        EnemiesAlive -= 1;
     
        Score += 10;
    }
    IEnumerator beamUp()
    {
        yield return new WaitForSeconds(1.5f);
        beam.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameObject x = Instantiate(playerSuckedIn, GameObject.FindWithTag("Player").transform.position, Quaternion.identity);
        Destroy(x, 4f);
     
        currentState = GameState.NextTransition;
        yield return new WaitForSeconds(3f);
        Transition();

    }
    IEnumerator SpawnEnemies(int numberOfEnemies)
    {

        if ((roomNumber+1) % 2 == 0)
        {
          
                GameObject gun = gunList[Random.Range(0, gunList.Length)];
                Instantiate(gun , gunPos.position , Quaternion.identity);
          
        }
        currentState = GameState.EnemySpawning;
        if (numberOfEnemies > 10)
            numberOfEnemies = 10;
        for (int i= 0; i < numberOfEnemies; i++)
        {
            int randomEnemyIndex = Random.Range(0, enemyList.Length);
        

            StartCoroutine(SpawnEnemy(enemyList[randomEnemyIndex]));
            yield return new WaitForSeconds(.4f);
            
        }
        currentState = GameState.Fighting;
    }

    IEnumerator SpawnEnemy(GameObject enemy)
    {
        Vector2 spawnPos = spawnPositions[Random.Range(0, spawnPositions.Count)].position;

        GameObject vfx= Instantiate(spawningVFX, spawnPos, Quaternion.identity);
        yield return new WaitForSeconds(0.6f);
        Destroy(vfx);
        Instantiate(enemy, spawnPos, Quaternion.identity);
    }

    IEnumerator TeleportOut()
    {
        yield return new WaitForSeconds(1f);
        currentState = GameState.Teleporting;
        roomNumber++;
        GameObject x = Instantiate(teleporting, GameObject.FindWithTag("Player").transform.position, Quaternion.identity);
        Destroy(x, 2f);
        yield return new WaitForSeconds(1f);
        UIManager.current.StartFadeSequence();
        yield return new WaitForSeconds(.8f);
        if (roomNumber + 1 == 6)
        {
            changeLayoutBoss();
        }
        else
        {
            int randIndex = Random.Range(0, layouts.Length);
            changeLayout(randIndex);

        }


        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Gun"))
        {
            if(g.GetComponent<RangedWeapon>().isPickUp)
            {
                Destroy(g);
            }
        }
        teleportStarted = false;
    

    }


    IEnumerator SpawnBoss(GameObject boss)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
            EventManager.current.FirstBossSpawn();
        else if (SceneManager.GetActiveScene().buildIndex == 2)
            EventManager.current.SecondBossSpawn();

            currentState = GameState.EnemySpawning;
        Vector2 spawnPos = Vector2.zero;

        GameObject vfx = Instantiate(bossSpawningVFX, spawnPos, Quaternion.identity);
        yield return new WaitForSeconds(3f);
        Destroy(vfx);
        GameObject endvfx = Instantiate(bossSpawningVFXEnd, spawnPos, Quaternion.identity);
        Destroy(endvfx, 3f);
        Instantiate(boss, spawnPos, Quaternion.identity);
        currentState = GameState.BossStage;

      
    }

    IEnumerator startGame()
    {
        EventManager.current.GameStart();
        yield return new WaitForSeconds(.3f);

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            UIManager.current.StartAscend();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            UIManager.current.Higher();
        }

        yield return new WaitForSeconds(4.8f);
        currentState = GameState.ReadyToSpawn;
    }

    void TrasnslateToNextLevel()
    {
        currentState = GameState.ReadyToSpawn;
        GameObject.FindWithTag("Player").GetComponent<CharacterMovement>().SetVisible();
    }

    public void resetGame()
    {
        Destroy(EventManager.current.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    IEnumerator nextTransition()
    {
       
     
        yield return  new WaitForSeconds(.4f);
        UIManager.current.CameraPanUpHigh();

        yield return new WaitForSeconds(1.1f);
        nextScene();
    }

    public void Transition()
    {
        StartCoroutine(nextTransition());
    }

    public void nextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
