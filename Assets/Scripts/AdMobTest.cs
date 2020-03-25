using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class AdMobTest : MonoBehaviour
{
    [SerializeField] private GameObject _originObject;

    [SerializeField] private Button _btnInitAdMob;
    [SerializeField] private Button _btnLoadAd;
    [SerializeField] private Button _btnShowAd;
    [SerializeField] private Button _btnShowRewardAd;

    [SerializeField] private ScrollRect _scrollRect;

    private RewardedAd _rewardedAd;
    private InterstitialAd _interstitialAd;

    private readonly string _rewardAdUnitId = "ca-app-pub-3940256099942544/5224354917"; // NOTE(JJO): 테스트 광고
    private readonly string _interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";

    private void Log(string message)
    {
        StartCoroutine(LogCoroutine(message));
    }

    private IEnumerator LogCoroutine(string message)
    {
        var obj = GameObject.Instantiate(_originObject, _scrollRect.content);
        obj.SetActive(true);
        obj.transform.localPosition = Vector3.zero;
        obj.GetComponent<Text>().text = message;

        Debug.Log(message);
        
        yield return null;
        
        _scrollRect.verticalNormalizedPosition = 0f;
    }

    private void RequestLoadRewardAd()
    {
        _rewardedAd = new RewardedAd(_rewardAdUnitId);

        _rewardedAd.OnAdLoaded += OnRewardAdLoaded;
        _rewardedAd.OnAdFailedToLoad += OnAdFailedToLoad;
        _rewardedAd.OnAdOpening += OnAdOpening;
        _rewardedAd.OnAdFailedToShow += OnAdFailedToShow;
        _rewardedAd.OnUserEarnedReward += OnUserEarnedReward;
        _rewardedAd.OnAdClosed += OnRewardAdClosed;
        
        _rewardedAd.LoadAd(new AdRequest.Builder().Build());
    }

    private void RequestLoadAd()
    {
        _interstitialAd = new InterstitialAd(_interstitialAdUnitId);

        _interstitialAd.OnAdLoaded += OnAdLoaded;
        _interstitialAd.OnAdFailedToLoad += OnAdFailedToLoad;
        _interstitialAd.OnAdOpening += OnAdOpening;
        _interstitialAd.OnAdClosed += OnAdClosed;
        _interstitialAd.OnAdLeavingApplication += OnAdLeavingApplication;
        
        _interstitialAd.LoadAd(new AdRequest.Builder().Build());
    }

    private void OnAdLeavingApplication(object sender, EventArgs e)
    {
        Log($"OnAdLeavingApplication: {e}");
    }

    private void OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        Log($"OnAdFailedToLoad: {e.Message}");
    }
    
    private void OnAdClosed(object sender, EventArgs e)
    {
        Log($"OnAdClosed: {e}");
        
        RequestLoadAd();
    }

    private void OnRewardAdClosed(object sender, EventArgs e)
    {
        Log($"OnRewardAdClosed: {e}");

        RequestLoadRewardAd();
    }

    private void OnUserEarnedReward(object sender, Reward e)
    {
        var type = e.Type;
        var amount = e.Amount;
        
        Log($"OnUserEarnedReward: type={type} / amount={amount}");
    }

    private void OnAdFailedToShow(object sender, AdErrorEventArgs e)
    {
        Log($"OnAdFailedToShow: {e.Message}");
    }

    private void OnAdOpening(object sender, EventArgs e)
    {
        Log($"OnAdOpening: {e}");
    }

    private void OnAdFailedToLoad(object sender, AdErrorEventArgs e)
    {
        Log($"OnAdFailedToLoad: {e.Message}");
    }

    private void OnRewardAdLoaded(object sender, EventArgs e)
    {
        Log($"OnAdLoaded: {e}");
    }

    private void OnAdLoaded(object sender, EventArgs e)
    {
        Log($"OnAdLoaded: {e}");
    }

    public void OnClickInitAdMob()
    {
        MobileAds.Initialize(initStatus =>
        {
            if (initStatus != null)
            {
                Log(initStatus.ToString());
            }
            else
            {
                if (Application.isEditor == false)
                {
                    Log("Something has wrong!");
                }
            }
        });
    }

    public void OnClickLoadRewardAd()
    {
        RequestLoadRewardAd();
        
        Log($"Request to load a RewardAd");
    }

    public void OnClickLoadAd()
    {
        RequestLoadAd();
        
        Log($"Request to load a InterstitialAd");
    }

    public void OnClickShowAd()
    {
        if (_interstitialAd.IsLoaded() == false)
        {
            Log($"{nameof(_interstitialAd)} is not loaded!");
            return;
        }
        
        _interstitialAd.Show();
    }

    public void OnClickShowRewardAd()
    {
        if (_rewardedAd.IsLoaded() == false)
        {
            Log($"{nameof(_rewardedAd)} is not loaded!");
            return;
        }
        
        _rewardedAd.Show();
    }

    public void OnClickTestLog()
    {
        Log($"Test - {UnityEngine.Random.Range(int.MinValue, int.MaxValue)}");
    }
}
