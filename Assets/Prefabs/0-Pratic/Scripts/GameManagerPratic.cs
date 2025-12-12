using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GameManagerPratic : NetworkBehaviour
{
    public static GameManagerPratic instance;

    public GameObject entryPrefab;
    public Transform content;
    public Kien inputActions;

    public GameObject leaderUI;

    private void Awake()
    {
        instance = this;

        
            inputActions = new Kien();
            inputActions.Enable();
            inputActions.UI.LeaderBoard.started += ToggleLeader;
        
    }

    public List<PlayerStasPratic> allPlayer = new List<PlayerStasPratic>();

    public void RegisterPlayer(PlayerStasPratic player)
    {
        if (!allPlayer.Contains(player)) 
        { 
            allPlayer.Add(player);
        }
    }

    public List<PlayerStasPratic> GetLeaderBoard()
    {
        List<PlayerStasPratic> result = new List<PlayerStasPratic>(allPlayer);
        result.Sort((a, b) => b.Score.CompareTo(a.Score));
        return result;
    }

    private void ToggleLeader(InputAction.CallbackContext context)
    {
        leaderUI.gameObject.SetActive(!leaderUI.gameObject.activeSelf);
    }

}
