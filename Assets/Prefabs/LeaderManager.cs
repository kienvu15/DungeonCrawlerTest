//using PlayFab;
//using PlayFab.ClientModels;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class LeaderboardManager : MonoBehaviour
//{
//    // Tên Leaderboard đã tạo trong PlayFab Manager (là 'Leader')
//    private const string LeaderboardName = "Leader";

//    [Header("UI References")]
//    [SerializeField] private GameObject entryPrefab; // Prefab cho 1 hàng (Rank, Name, Score)
//    [SerializeField] private Transform entriesParent; // Parent (Content) của Scroll View

//    /// <summary>
//    /// Gọi API PlayFab để lấy Leaderboard và hiển thị
//    /// </summary>
//    public void GetAndDisplayLeaderboard()
//    {
//        // Xóa các hàng Leaderboard cũ trước khi tải cái mới
//        foreach (Transform child in entriesParent)
//        {
//            Destroy(child.gameObject);
//        }

//        if (!PlayFabLogin.LoggedIn) // Giả sử bạn có lớp PlayFabLogin để kiểm tra trạng thái
//        {
//            Debug.LogError("Chưa đăng nhập PlayFab.");
//            return;
//        }

//        var request = new GetLeaderboardRequest
//        {
//            StatisticName = LeaderboardName, // Phải khớp với tên Leaderboard
//            MaxResultsCount = 10,           // Lấy 10 người đứng đầu
//            StartPosition = 0               // Bắt đầu từ vị trí 0 (người đứng đầu)
//        };

//        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnGetLeaderboardFailure);
//    }

//    private void OnGetLeaderboardSuccess(GetLeaderboardResult result)
//    {
//        Debug.Log("Lấy Leaderboard thành công!");

//        foreach (var entry in result.Leaderboard)
//        {
//            // Tạo và điền dữ liệu cho từng hàng
//            DisplayLeaderboardEntry(entry);
//        }
//    }

//    private void OnGetLeaderboardFailure(PlayFabError error)
//    {
//        Debug.LogError("Lỗi khi lấy Leaderboard: " + error.GenerateErrorReport());
//    }

//    private void DisplayLeaderboardEntry(PlayerLeaderboardEntry entry)
//    {
//        // 1. Tạo hàng mới
//        GameObject entryObject = Instantiate(entryPrefab, entriesParent);

//        // 2. Lấy các component Text (cần tùy chỉnh theo cấu trúc Prefab của bạn)
//        // Bạn cần đảm bảo entryPrefab có các Text/TMP này
//        TextMeshProUGUI rankText = entryObject.transform.Find("RankText").GetComponent<TextMeshProUGUI>();
//        TextMeshProUGUI nameText = entryObject.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
//        TextMeshProUGUI scoreText = entryObject.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();

//        //// 3. Điền dữ liệu
//        rankText.text = (entry.Position + 1).ToString(); // Position là 0-indexed, cần cộng 1
//        nameText.text = entry.DisplayName ?? "No Name";  // Sử dụng DisplayName (Tên hiển thị)
//        scoreText.text = entry.StatValue.ToString();     // StatValue là điểm số

//        // *Tùy chọn: Highlight người chơi hiện tại*
//        if (entry.PlayFabId == PlayFabLogin.PlayFabId) // Giả sử PlayFabId được lưu trong PlayFabLogin
//        {
//            entryObject.GetComponent<Image>().color = Color.yellow; // Ví dụ
//        }
//    }
//}