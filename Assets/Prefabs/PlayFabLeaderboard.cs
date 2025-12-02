using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayFabLeaderboard
{
    // Gửi score lên leaderboard
    public static void SubmitScore(int score, Action onSuccess = null)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "Score", Value = score }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request,
        result =>
        {
            Debug.Log("[Leaderboard] Score submitted!");
            onSuccess?.Invoke();
        },
        error =>
        {
            Debug.LogError("[Leaderboard] Submit FAILED: " + error.GenerateErrorReport());
        });
    }

    // Lấy top 100
    public static void GetTop100(Action<List<PlayerLeaderboardEntry>> callback)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Score",
            StartPosition = 0,
            MaxResultsCount = 100
        };

        PlayFabClientAPI.GetLeaderboard(request,
        result =>
        {
            Debug.Log("[Leaderboard] Top 100 loaded!");
            callback?.Invoke(result.Leaderboard);
        },
        error =>
        {
            Debug.LogError("[Leaderboard] Load FAILED: " + error.GenerateErrorReport());
        });
    }

    // Lấy leaderboard quanh người chơi
    public static void GetAroundPlayer(Action<List<PlayerLeaderboardEntry>> callback)
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "Score",
            MaxResultsCount = 20
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(request,
        result =>
        {
            Debug.Log("[Leaderboard] Around player loaded!");
            callback?.Invoke(result.Leaderboard);
        },
        error =>
        {
            Debug.LogError("[Leaderboard] Around FAILED: " + error.GenerateErrorReport());
        });
    }
}
