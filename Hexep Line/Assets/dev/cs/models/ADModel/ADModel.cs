using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class ADModel : Singleton<ADModel>
{
	public const int REWARD_AD_TIMES = 3;
	public const int REWARD_AD_GEM = 1;

	public bool enableTestMode = true;

	public Action showADEvent;

	public void LoadADData()
	{
		if (PlayerModel.CLEAR_ALL) {

			PlayerPrefsUtil.RemoveData(PlayerPrefsUtil.AD_PLAY_TIMES);
			PlayerPrefsUtil.RemoveData(PlayerPrefsUtil.AD_TODAY_TIMES);
			PlayerPrefsUtil.RemoveData(PlayerPrefsUtil.AD_RESET_DAY);
		}
		CheckResetDay ();
	}

	public void CheckResetDay()
	{
		DateTime todayTime = DateTime.UtcNow;

		int today = todayTime.DayOfYear;
		int resetDay = PlayerPrefsUtil.GetInt (PlayerPrefsUtil.AD_RESET_DAY);

		PlayerPrefsUtil.SetInt(PlayerPrefsUtil.AD_RESET_DAY,today);

		if(today != resetDay)
		{
			PlayerPrefsUtil.SetInt(PlayerPrefsUtil.AD_TODAY_TIMES,0);
		}
	}

	public void Init()
	{
		string gameId = null;

		#if UNITY_IOS // If build platform is set to iOS...
		gameId = "1144696";
		#elif UNITY_ANDROID // Else if build platform is set to Android...
		gameId = "1144700";
		#endif

		if (string.IsNullOrEmpty(gameId)) { // Make sure the Game ID is set.
			Debug.LogError("Failed to initialize Unity Ads. Game ID is null or empty.");
		} else if (!Advertisement.isSupported) {
			Debug.LogWarning("Unable to initialize Unity Ads. Platform not supported.");
		} else if (Advertisement.isInitialized) {
			Debug.Log("Unity Ads is already initialized.");
		} else {
			Debug.Log(string.Format("Initialize Unity Ads using Game ID {0} with Test Mode {1}.",
				gameId, enableTestMode ? "enabled" : "disabled"));
			Advertisement.Initialize(gameId, enableTestMode);
		}
	}

	public bool ADIsReady(bool isRewardedView = false)
	{
		if(isRewardedView)
		{
			return Advertisement.IsReady ("rewardedVideoZone");
		}else
		{
			return Advertisement.IsReady ();
		}
	}

	public void ShowAD(bool isRewardedView = false)
	{
		if(isRewardedView)
		{
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideoZone", options);
		}else
		{
			Advertisement.Show ();
			Debug.Log("Advertisement.Show (simple)");
		}
		OnShowAd ();
	}

	private void OnShowAd()
	{
		int todayTimes = PlayerPrefsUtil.GetInt (PlayerPrefsUtil.AD_TODAY_TIMES);

		PlayerPrefsUtil.SetInt(PlayerPrefsUtil.AD_TODAY_TIMES,todayTimes + 1);
		PlayerPrefsUtil.SetInt(PlayerPrefsUtil.AD_PLAY_TIMES,PlayerPrefsUtil.GetInt(PlayerPrefsUtil.AD_PLAY_TIMES) + 1);
		CheckResetDay ();

		if(todayTimes < REWARD_AD_TIMES)
		{
			WealthInfo gemInfo = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Gem);
			PromptModel.Instance.Pop("+" + REWARD_AD_GEM, true, gemInfo.type);

			gemInfo.count += REWARD_AD_GEM;
			PlayerModel.Instance.SaveWealths(gemInfo.type);
		}

		if(showADEvent != null)
		{
			showADEvent ();
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log("The ad was successfully shown.");
			PromptModel.Instance.Pop("The ad was successfully shown.");
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			PromptModel.Instance.Pop("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			PromptModel.Instance.Pop("The ad failed to be shown.");
			break;
		}
	}
}