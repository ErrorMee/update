using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public enum SocialLoginType
{
    failure = 0,    //失败
    success = 1,    //成功
    wait = 2,       //等待
}

public class SocialModel : Singleton<SocialModel>
{
    public SocialLoginType loginTupe = SocialLoginType.wait;

    public void StartUp()
    {
		Debug.Log("*** Authenticate ***");
        //认证
        Social.localUser.Authenticate(HandleAuthenticate);
    }

    private void HandleAuthenticate(bool success)
    {
        Debug.Log("*** HandleAuthenticated: success = " + success);
        if (success)
        {
            loginTupe = SocialLoginType.success;

            //需要在iTunesConnect里设置排行、成就之类的东西
            //Social.localUser.LoadFriends(HandleFriendsLoaded);
            //Social.LoadAchievements(HandleAchievementsLoaded);
            //Social.LoadAchievementDescriptions(HandleAchievementDescriptionsLoaded);
        }
        else
        {
            loginTupe = SocialLoginType.failure;
        }
    }
    private void HandleFriendsLoaded(bool success)
    {
        Debug.Log("*** HandleFriendsLoaded: success = " + success);
        foreach (IUserProfile friend in Social.localUser.friends)
        {
            Debug.Log("* friend = " + friend.ToString());
        }
    }

    private void HandleAchievementsLoaded(IAchievement[] achievements)
    {
        Debug.Log("* HandleAchievementsLoaded");
        foreach (IAchievement achievement in achievements)
        {
            Debug.Log("* achievement = " + achievement.ToString());
        }
    }

    private void HandleAchievementDescriptionsLoaded(IAchievementDescription[] achievementDescriptions)
    {
        Debug.Log("*** HandleAchievementDescriptionsLoaded");
        foreach (IAchievementDescription achievementDescription in achievementDescriptions)
        {
            Debug.Log("* achievementDescription = " + achievementDescription.ToString());
        }
    }

    // achievements
    public void ReportProgress(string achievementId, double progress)
    {
		if (Social.localUser.authenticated && GameModel.Instance.DebugDev == false)
        {
			double offProgress = Convert.ToDouble (Convert.ToDouble (progress * 100).ToString("0.0"));
			Debug.Log ("achievement " + achievementId + " " + offProgress);
			Social.ReportProgress(achievementId, offProgress, HandleProgressReported);
        }
    }

    private void HandleProgressReported(bool success)
    {
        Debug.Log("*** HandleProgressReported: success = " + success);
    }

    public void ShowAchievements()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
    }

    // leaderboard
    public void ReportScore(string leaderboardId, long score)
    {
		if (Social.localUser.authenticated && GameModel.Instance.DebugDev == false)
        {
            Social.ReportScore(score, leaderboardId, HandleScoreReported);
        }
    }

    private void HandleScoreReported(bool success)
    {
        Debug.Log("*** HandleScoreReported: success = " + success);
    }

    public void ShowLeaderboard()
    {
		if (Social.localUser.authenticated)
		{
			Social.ShowLeaderboardUI();
		}
    }

	public void ShowReview()
	{
		const string APP_ID = "1138811103";
		string url = 
			string.Format(
			"itms-apps://ax.itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id={0}",
			APP_ID);

		//todo
		//url = "itms-apps://ax.itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=1138811103";
		//url = "https://itunes.apple.com/us/app/bug-bugs/id1138811103?l=zh&ls=1&mt=8";
		//url = "itms-apps://phobos.apple.com/WebObjects/MZStore.woa/wa/viewSoftware?id=1138811103";
		url = "itms-apps://itunes.apple.com/app/viewContentsUserReviews?id=1138811103";
		Debug.Log("ShowReview = " + url);
		Application.OpenURL(url);
	}
}