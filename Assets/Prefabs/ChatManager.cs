using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    public ChatUI ui;
    public ChatClient chatClient;

    public static ChatManager instance;
    Dictionary<string, string> contentChatChanel = new Dictionary<string, string>();

    private void Awake()
    {
    }

    private void Start()
    {
        instance = this;

        //Đặt nickname cho người chơi
        PhotonNetwork.LocalPlayer.NickName = "Player" + Random.Range(1, 1000);
        chatClient = new ChatClient(this);


        //Chọn vùng là asia
        chatClient.ChatRegion = "ASIA";
        //Kết nối tới server
        chatClient.Connect(
        PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
        PhotonNetwork.AppVersion,
        new AuthenticationValues($"User_{PhotonNetwork.LocalPlayer.NickName}")
        );
    }
    
   private void Update()
    {
        chatClient.Service();
    }

    public void SendChatMessage(string channelName, string msg)
    {
        chatClient.PublishMessage(channelName, msg);
    } 

    public void SendPrivateMessage(string nickname, string msg)
    {
        chatClient.SendPrivateMessage(nickname, msg);
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }
    
    public void OnDisconnected()
    {
    }

    public string GetChannelContent(string channel)
    {
        if (contentChatChanel.TryGetValue(channel, out string content))
            return content;
        return "";
    }

    public void OnConnected()
    {
        //Đăng ký các kênh chat
        string[] k = new string[] { "Kênh thế giới", "Kênh bang hội" };
        chatClient.Subscribe(k, 10);
        foreach(string s in k)
        {
            contentChatChanel.Add(s, "");
        }
    }
    
    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string mess = "";
        for (int i = 0; i < senders.Length; i++)
        {
            mess += $"{senders[i]}: {messages[i]}\n";
        }

        contentChatChanel[channelName] += mess;

        // Nếu UI đang ở channel này thì mới update
        if (ui.CurrentChannel == channelName)
        {
            ui.allChatContent.text = contentChatChanel[channelName];
        }
    }



    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        string mess = "";
        mess += $"{sender}: {message}\n";
        ui.allChatContent.text += mess;
    }
    
    public void OnSubscribed(string[] channels, bool[] results)
    {
    }
    
    public void OnUnsubscribed(string[] channels)
    {
    }
    
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
    }
    
    public void OnUserSubscribed(string channel, string user)
    {
    }
    
    public void OnUserUnsubscribed(string channel, string user)
    {
    }
}