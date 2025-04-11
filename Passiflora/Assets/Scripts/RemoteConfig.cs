using UnityEngine;
using Unity.RemoteConfig;
using System.Threading;
using System;
using Unity.Services.RemoteConfig;

public class RemoteConfig : MonoBehaviour
{
    public struct userAttributes
    {
        public bool expansionFlag;
    }

    public struct appAttributes
    {
        public int level;
        public int score;
        public string appVersion;
    }

    private float startSpeed;
    private float maxSpeed;

    private int adsCounter;

    public bool newData = false;
    public bool finished = false;

    public static RemoteConfig instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
    }

    public void Start()
    {
        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
    }

    void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        
        switch (configResponse.requestOrigin)
        {   
            case ConfigOrigin.Default:
                Debug.Log("No settings loaded this session; using default values.");
                break;
            case ConfigOrigin.Cached:
                Debug.Log("No settings loaded this session; using cached values from a previous session.");
                startSpeed = RemoteConfigService.Instance.appConfig.GetFloat("StartSpeed");
                maxSpeed = RemoteConfigService.Instance.appConfig.GetFloat("MaxSpeed");
                adsCounter = RemoteConfigService.Instance.appConfig.GetInt("AdsCounter");
                break;
            case ConfigOrigin.Remote:
                Debug.Log("New settings loaded this session; update values accordingly.");
                startSpeed = RemoteConfigService.Instance.appConfig.GetFloat("StartSpeed");
                maxSpeed = RemoteConfigService.Instance.appConfig.GetFloat("MaxSpeed");
                adsCounter = RemoteConfigService.Instance.appConfig.GetInt("AdsCounter");
                newData = true;
                break;
        }

        finished = true;
    }

    public float GetStartSpeed()
    {
        return startSpeed;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public int GetAdsCounter()
    {
        return adsCounter;
    }
}
