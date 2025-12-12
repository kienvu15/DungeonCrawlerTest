using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    public static bool LoggedIn = false;
    public static event System.Action OnLoginSuccess; // <--- THÊM DÒNG NÀY

    private void Start()
    {
        Debug.Log("[PlayFab] Logging in...");

        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier + UnityEngine.Random.Range(0, 99999999),
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request,
        result =>
        {
            Debug.Log("[PlayFab] Login success!");
            LoggedIn = true;

            PlayFabPlayerStats.LoadStats();
            OnLoginSuccess?.Invoke(); // <--- GỌI EVENT KHI THÀNH CÔNG

        },
        error =>
        {
            Debug.LogError("[PlayFab] Login FAILED: " + error.GenerateErrorReport());
        });
    }
}