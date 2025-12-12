using UnityEngine;
using Fusion;
using TMPro;
public class Avatar : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(OnChangedNickname))]
    public string Nickname { get; set; }

    [Networked, OnChangedRender(nameof(OnChangedReadyStatus))]
    public bool readyStatus { get; set; }

    //UI
    public TextMeshProUGUI text_nickname;
    public GameObject readyUI;
    public override void Spawned()
    {
        transform.parent = LobbyManager.Instance.parentAvatar;
        transform.localPosition = Vector3.one;

        if (Object.HasInputAuthority)
        {
            RpcSetNickName(LobbyManager.Instance.nickName);
        }
        else
        {
            OnChangedNickname();
        }
    }

    void OnChangedNickname()
    {
        text_nickname.text = Nickname;
    }
    void OnChangedReadyStatus() 
    { 
        readyUI.SetActive(readyStatus);
    }

    //RPC
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RpcSetNickName(string name) => Nickname = name;

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RpcChangStatus()
    {
        readyStatus = !readyStatus;
        if(readyStatus)
        {
            LobbyManager.Instance.countPlayerReady++;
        }
        else
        {
            LobbyManager.Instance.countPlayerReady--;
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RpcSaveNetworkObject() => LobbyManager.Instance.self_networkObject = Object;

    

}
