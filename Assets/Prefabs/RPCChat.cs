using Fusion;
using UnityEngine;

public class RPCChat : NetworkBehaviour
{
    public static RPCChat chat;
    public ChatUI chatUI;
    private void Awake()
    {
        chat = this;
    }
    [
    Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcReceiveMessenger(string playerID, string messenger)
    {
        Debug.Log("called");
        string formatteMessage = $"{playerID}:{messenger}";
        chatUI.allChatContent.text += formatteMessage + "\n";
    }
    public void SendChatContent(string mes)
    {
        string playerName = Runner.LocalPlayer.ToString();
        Debug.Log(playerName + ": " + mes);
        RpcReceiveMessenger(playerName, mes);
    }
}