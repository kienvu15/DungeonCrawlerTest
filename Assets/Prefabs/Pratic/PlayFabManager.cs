using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;

public class PlayFabManager : MonoBehaviour
{
    private void Start()
    {
        //Guid guid = Guid.NewGuid();
        string uid = PlayerPrefs.GetString("uuid");
        Login(uid);
        StartCoroutine(StartLogin(uid));
    }

    IEnumerator StartLogin(string uid)
    {
        yield return Login(uid);
        yield return Addnewuser("TestUser", "TestPassword123");

        PlayData playData = new PlayData
        {
            name = "PlayerOne",
            score = 100,
            def = 50
        };

        string jsonData = JsonUtility.ToJson(playData);
        yield return UpdateUserData("PlayerData", jsonData);
    }

    IEnumerator Login(string customId)
    {
        bool isDone = false;
        var request = new LoginWithCustomIDRequest
        {
            CustomId = customId,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, result =>
        {
            isDone = true;
        }, error =>
        {
            isDone = true;
        });
        yield return new WaitUntil(() => isDone);
    }

    IEnumerator Addnewuser(string username, string password)
    {
        bool isDone = false;
        var request = new AddUsernamePasswordRequest
        {
            Username = username,
            Password = password
        };



        PlayFabClientAPI.AddUsernamePassword(request, result =>
        {
            isDone = true;
        }, error =>
        {
            isDone = true;
        });

        yield return new WaitUntil(() => isDone);
    }

    IEnumerator UpdateDisplayname(string displayName)
    {
        bool isDone = false;
        var request = new UpdateUserTitleDisplayNameRequest
        { 
            DisplayName = displayName
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, result =>
        {
            isDone = true;
        }, error =>
        {
            isDone = true;
        });
        yield return new WaitUntil(() => isDone);
    }

    IEnumerator UpdateUserData(string key, string value)
    {
        bool isDone = false;

        var request = new UpdateUserDataRequest
        {
            Data = new System.Collections.Generic.Dictionary<string, string>
            {
                { key, value }
            }
        };

        yield return new WaitUntil(() => isDone);
    }

}

[System.Serializable]
public class PlayData
{
    public string name;
    public int score;
    public int def;
}