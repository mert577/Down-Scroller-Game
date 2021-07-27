using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    [SerializeField] public static EventManager current;
    // Start is called before the first frame update
    void Awake()
    {
      
        if (current != null && current != this)
        {
            Destroy(this.gameObject);
            return;
        }
        current = this;

    }


    public void Start()
    {
   
    }


    public event Action onWaveClear;
    public void WaveClear()
    {
        if (onWaveClear != null)
        {
            onWaveClear();
        }
    }

    public event Action onPlayerDeath;
    public void PlayerDeath()
    {
        if (onPlayerDeath != null)
        {
            onPlayerDeath();
        }
    }

    public event Action onEnemyDeath;
    public void EnemyDeath()
    {
        if (onEnemyDeath != null)
        {
            onEnemyDeath();
        }
    }

    public event Action onEnemySpawn;
    public void enemySpawn()
    {
        if (onEnemySpawn != null)
        {
            onEnemySpawn();
        }
    }


    public event Action onNewLevelReady;
    public void newLevelReady()
    {
        if (onNewLevelReady != null)
        {
            onNewLevelReady();
        }
    }

    public event Action onPlayerShoot;
    public void PlayerShoot()
    {
     if(onPlayerShoot!= null)
        {
            onPlayerShoot();
        }

    }

    public event Action onFirstBossAttack;
    public void FirstBossAttack()
    {
        if(onFirstBossAttack!= null)
        {
            onFirstBossAttack();
        }
    }

    public event Action onFirstBossAttackEnd;
    public void FirstBossAttackEnd()
    {
        if (onFirstBossAttackEnd != null)
        {
            onFirstBossAttackEnd();
        }
    }

    public event Action onPaletteChange;
    public void PaletteChange()
    {
        if (onPaletteChange != null)
        {
            onPaletteChange();
        }
    }

    public event Action onGameStart;
    public void GameStart()
    {
        if(onGameStart!= null)
        {
            onGameStart();
        }
    }


    public event Action onFinishStage;
    public void FinishStage()
    {
        if (onFinishStage != null)
        {
            onFinishStage();
        }
    }

    public event Action onGameOpen;
    public void GameOpen()
    {
        if(onGameOpen!= null)
        {
            onGameOpen();
        }
    }

    public event Action onFirstBossSpawn;
    public void FirstBossSpawn()
    {
        if (onFirstBossSpawn != null)
        {
            onFirstBossSpawn();
        }
    }
    public event Action onSecondBossSpawn;
    public void SecondBossSpawn()
    {
        if (onSecondBossSpawn != null)
        {
            onSecondBossSpawn();
        }
    }
    public event Action onFirstStageStart;
    public void FirstStageStart()
    {
        if (onFirstStageStart != null)
        {
            onFirstStageStart();
        }
    }

    public event Action onSecondStageStart;
    public void SecondStageStart()
    {
        if (onSecondStageStart != null)
        {
            onSecondStageStart();
        }
    }

    public event Action onPause;
    public void Pause()
    {
        if (onPause != null)
        {
            onPause();
        }
    }

    public event Action onResume;
    public void Resume()
    {
        if (onResume != null)
        {
            onResume();
        }
    }
}
