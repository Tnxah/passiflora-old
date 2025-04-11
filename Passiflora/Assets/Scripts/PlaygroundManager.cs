using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaygroundManager : MonoBehaviour
{
    public static PlaygroundManager instance;

    public ObstacleSpawner obstacleSpawner;

    public FingerControl fc;

    public float speed;
    public int score;

    [SerializeField]
    private static int countToAd;

    public bool resurrected;

    public delegate void OnDeathCallback();
    public OnDeathCallback onDeathCallback;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void Start()
    {
        print("TEST Start in PlayerManage");
        fc = GetComponent<FingerControl>();
        Initialize();
        AdsManager.instance.SetReward(Resurrect);
        StartCoroutine(StartGame());
        print("TEST Last line in Start in PlayerManage");
    }

    void Initialize()
    {
        speed = Settings.startSpeed;
        countToAd = countToAd <= 0 ? Settings.adsCounter : countToAd;
    }

    IEnumerator StartGame()
    {
        print("TEST StartGame in PlayerManage");
        Time.timeScale = 1;

#if UNITY_EDITOR
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
#else
        yield return new WaitUntil(()=> FingerControl.instance.BouthTouched());
#endif
        GameManager.instance.ChangeState(GameManager.gameScene, GameState.Play);
        print("TEST 1");
        PlaygroundUIManager.instance.OnPlay();
        print("TEST 2");
        obstacleSpawner.Launch();
        print("TEST 3");
        StartCoroutine(SpeedIncreaser());
        print("TEST 4");
        StartCoroutine(ScoreIncreaser());
        print("TEST 5");
    }

    IEnumerator ScoreIncreaser()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / (speed / 10));
            ObstacleSpawner.instance.FillPool(score);
            score++;
        }
        
    }

    IEnumerator SpeedIncreaser()
    {
        print("TEST a");
        while (true)
        {
            print($"TEST b {speed} {Settings.maxSpeed}");
            while (speed < Settings.maxSpeed)
            {
                print($"TEST c {speed} {Settings.maxSpeed}");
                yield return new WaitForSeconds(10);
                print("TEST d");
                speed++;
                print("TEST e");
            }
            print("TEST f");
        }

    }
    public void OnDeath()
    {
        PauseGame();

        PlaygroundUIManager.instance.OnDeath();

        onDeathCallback?.Invoke();

        GooglePlayServicesManager.instance.SaveScore(score);
        GooglePlayServicesManager.instance.DistanceAchive(score);

        countToAd--;
        if (countToAd == 0)
        {
            AdsManager.instance.ShowNonRewardedAd();
            countToAd = Settings.adsCounter;
        }
    }

    public void Resurrect()
    {
        ResumeGame();
        resurrected = true;

        ObstacleSpawner.instance.Clean();

        FingerControl.instance.StopLights();

        PlaygroundUIManager.instance.OnResurrect();        
    }

    public void Restart()
    {
        ResumeGame();
        PlaygroundUIManager.instance.OnRestart();
        resurrected = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void PauseGame()
    {
        print("PauseGame Trigger");
        Time.timeScale = 0;
        GameManager.instance.ChangeState(GameManager.gameScene, GameState.Pause);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    private void OnEnable()
    {
        AdsManager.instance.ToggleBanner(true);
    }

    private void OnDisable()
    {
        AdsManager.instance.ToggleBanner(false);
    }
}
