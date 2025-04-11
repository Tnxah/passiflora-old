using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool firstBoot = true;

    public static GameManager instance;

    public RemoteConfig remoteConfig;

    public static GameScene gameScene = GameScene.MainMenu;
    public static GameState gameState = GameState.NotInGame;

    public AdsManager adsManager;
    public FingerControl fingerControl;
    public PlaygroundManager playgroundManager;
    private void Start()
    {
        if (!instance)
        {
            instance = this;
        }

        if (firstBoot)
        {
            print("FirstBoot Trigger");
            FirstBoot();

            firstBoot = false;
        }

        OnStateChange();
    }

    private void FirstBoot()
    {
        remoteConfig.enabled = true;

        Settings.LoadPlayerPrefs();
        StartCoroutine(LoadRemoteConfig());
    }

    private IEnumerator LoadRemoteConfig()
    {
        yield return new WaitUntil(() => RemoteConfig.instance.finished);
        Settings.LoadRemoteConfig();
    }

    public void ChangeState(GameScene newScene, GameState newState)
    {
        gameScene = newScene;
        gameState = newState;

        if (gameState == GameState.Play)
            Time.timeScale = 1;

        print($"OnChangeState {gameScene} " +
            $"{gameState}");
    }

    private void OnStateChange()
    {
        print(gameScene);
        if (gameScene.Equals(GameScene.Game))
        {
            adsManager.enabled = true;
            fingerControl.enabled = true;
            playgroundManager.enabled = true;
        }
    }
}
