using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayFabPlayerStats
{
    public static bool Loaded = false;

    public static int CachedHealth = 100;
    public static int CachedMaxHealth = 100;
    public static int CachedScore = 0;

    public static event Action OnStatsLoaded;

    // ------------------------------------------
    // LOAD
    // ------------------------------------------
    public static void LoadStats()
    {
        if (!PlayFabLogin.LoggedIn)
        {
            Debug.LogWarning("[PlayFab] LoadStats skipped. Not logged in yet.");
            return;
        }

        Debug.Log("[PlayFab] Loading stats...");

        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),
        result =>
        {
            foreach (var s in result.Statistics)
            {
                if (s.StatisticName == "Health") CachedHealth = s.Value;
                if (s.StatisticName == "MaxHealth") CachedMaxHealth = s.Value;
                if (s.StatisticName == "Score") CachedScore = s.Value;
            }

            Loaded = true;
            Debug.Log("[PlayFab] Stats loaded!");

            OnStatsLoaded?.Invoke();
        },
        error =>
        {
            Debug.LogError("[PlayFab] Load stats FAILED: " + error.ErrorMessage);
            Loaded = true;
            OnStatsLoaded?.Invoke();
        });
    }

    // ------------------------------------------
    // SAVE
    // ------------------------------------------
    public static void SaveStats(int health, int maxHealth, int score)
    {
        if (!PlayFabLogin.LoggedIn)
        {
            Debug.LogWarning("[PlayFab] Skip SAVE — Not logged in yet");
            return;
        }

        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "Health", Value = health },
                new StatisticUpdate { StatisticName = "MaxHealth", Value = maxHealth },
                new StatisticUpdate { StatisticName = "Score", Value = score }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request,
        result =>
        {
            Debug.Log("[PlayFab] Stats SAVED!");
        },
        error =>
        {
            Debug.LogError("[PlayFab] Save stats FAILED: " + error.ErrorMessage);
        });
    }
}
