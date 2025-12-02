using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI leaderboardText;
    private const string LeaderboardName = "Score";

    private void Start()
    {
        // Đăng ký: Khi PlayFabLogin báo đăng nhập xong, hàm GetLeaderboard sẽ được gọi
        PlayFabLogin.OnLoginSuccess += GetLeaderboard;
    }

    private void OnDestroy()
    {
        // Quan trọng: Hủy đăng ký để tránh lỗi khi Scene bị thoát
        PlayFabLogin.OnLoginSuccess -= GetLeaderboard;
    }

    public void GetLeaderboard()
    {
        if (!PlayFabLogin.LoggedIn)
        {
            Debug.LogWarning("Chưa đăng nhập PlayFab.");
            return;
        }

        var request = new GetLeaderboardRequest
        {
            StatisticName = LeaderboardName,
            StartPosition = 0, // Bắt đầu từ vị trí 0
            MaxResultsCount = 10 // Lấy 10 người đứng đầu
        };

        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnError);
    }

    private void OnGetLeaderboardSuccess(GetLeaderboardResult result)
    {
        string leaderboardString = "--- Leaderboard Top 10 ---\n";

        // Duyệt qua các Entry (mục) trong Leaderboard
        foreach (var item in result.Leaderboard)
        {
            leaderboardString += $"#{item.Position + 1}. {item.DisplayName}: {item.StatValue}\n";
        }

        leaderboardText.text = leaderboardString;
        Debug.Log("Tải Leaderboard thành công!");
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError("Tải Leaderboard thất bại: " + error.GenerateErrorReport());
    }

    // GỌI HÀM NÀY KHI BẠN MUỐN HIỂN THỊ LEADERBOARD (ví dụ: nhấn nút)
    // public void ExampleCall()
    // {
    //     GetLeaderboard();
    // }
}