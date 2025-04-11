using System;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public string GAME_ID = "4473937";

    private const string BANNER_PLACEMENT = "Banner_Android";
    private const string VIDEO_PLACEMENT = "Interstitial_Android";
    private const string REWARDED_VIDEO_PLACEMENT = "Rewarded_Android";

    [SerializeField] private BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER;

    private bool testMode = false;
    private bool showBanner = false;

    public TextMeshProUGUI debug;

    Action onRewardedAdSuccess;
    public static AdsManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Initialize();
    }

    public void Initialize()
    {
        if (Advertisement.isSupported)
        {
            print(Application.platform + " supported by Advertisement");
        }
        Advertisement.Initialize(GAME_ID, testMode, this);
    }

    public void SetReward(Action reward)
    {
        onRewardedAdSuccess = reward;
    }

    public void ToggleBanner(bool toggle)
    {
        if (showBanner == toggle)
            return;

        showBanner = toggle;

        if (showBanner)
        {
            Advertisement.Banner.SetPosition(bannerPosition);
            Advertisement.Banner.Show(BANNER_PLACEMENT);
        }
        else
        {
            Advertisement.Banner.Hide(false);
        }
    }

    public void LoadRewardedAd()
    {
        Advertisement.Load(REWARDED_VIDEO_PLACEMENT, this);
    }

    public void ShowRewardedAd()
    {
        LoadRewardedAd();
        Advertisement.Show(REWARDED_VIDEO_PLACEMENT, this);
    }

    public void LoadNonRewardedAd()
    {
        Advertisement.Load(VIDEO_PLACEMENT, this);
    }

    public void ShowNonRewardedAd()
    {
        LoadNonRewardedAd();
        Advertisement.Show(VIDEO_PLACEMENT, this);
    }

    #region Interface Implementations
    public void OnInitializationComplete()
    {
        Debug.Log("Init Success");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Init Failed: [{error}]: {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Load Success: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Load Failed: [{error}:{placementId}] {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"OnUnityAdsShowFailure: [{error}]: {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"OnUnityAdsShowStart: {placementId}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"OnUnityAdsShowClick: {placementId}");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"OnUnityAdsShowComplete: [{showCompletionState}]: {placementId}");

        if (placementId == REWARDED_VIDEO_PLACEMENT && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            onRewardedAdSuccess?.Invoke();
        }
    }
    #endregion
}
