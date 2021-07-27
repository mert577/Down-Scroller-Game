using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SoundManager : MonoBehaviour
{
    public static SoundManager _soundManagerInstance;

    public  AudioSource _musicAudioSource;
    public AudioSource _pauseAudioSource;
    public AudioClip[] audioClips;
    AudioClip currentClip;
    
    
    private void Awake()
    {
        if(_soundManagerInstance!=null && _soundManagerInstance!= this)
        {
            Destroy(this.gameObject);
            return;
        }

        _soundManagerInstance = this;
        _musicAudioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(this);

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SubscribeToEvents;
    }

    private void Start()
    {
           }


    void SubscribeToEvents(Scene _scene,LoadSceneMode ls)
    {
        EventManager.current.onGameOpen += MainMenuMusic;
        EventManager.current.onFirstBossSpawn += FirstBossMusic;
        EventManager.current.onFirstStageStart += FirstStageMusic;
        EventManager.current.onSecondStageStart += SecondStageMusic;
        EventManager.current.onPause += PauseMusic;
        EventManager.current.onResume += ResumeMusic;
        EventManager.current.onSecondBossSpawn += SecondBossMusic;

    }
    private void Update()
    {
        
    }


    public void MainMenuMusic()
    {
        currentClip = audioClips[2];
        OnMusicChange();
    }


    public void FirstStageMusic()
    {
        currentClip = audioClips[1];
        OnMusicChange();

    }
    public void SecondStageMusic()
    {
        currentClip = audioClips[3];
        OnMusicChange();

    }
    public void FirstBossMusic()
    {
        currentClip = audioClips[0];
        OnMusicChange();
    }

    public void SecondBossMusic()
    {
        currentClip = audioClips[4];
        OnMusicChange();
    }

    void OnMusicChange()
    {
        if (_musicAudioSource.clip == currentClip) return;
        _musicAudioSource.clip = currentClip;
        _musicAudioSource.Play();
    }

    void PauseMusic()
    {
        _pauseAudioSource.volume = 1f;
        _musicAudioSource.Pause();
    }
    void ResumeMusic()
    {
        _pauseAudioSource.volume = 0f;
        _musicAudioSource.UnPause();

    }

}
