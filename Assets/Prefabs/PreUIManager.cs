using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PreUIManager : MonoBehaviour
{
    public static PreUIManager Instance;

    public TextMeshProUGUI scoreText;
    public GameObject panel;
    public GameObject leaderPanel;
    public Kien inputActions;

    public LeaderboardDisplay leaderboardDisplay;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        inputActions = new Kien();
        inputActions.Enable();
        inputActions.UI.Chat.started += ToggleChat;
        inputActions.Player.Tab.started += ToggleLead;
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void ToggleChat(InputAction.CallbackContext context)
    {
        panel.gameObject.SetActive(!panel.gameObject.activeSelf);
    }

    void ToggleLead(InputAction.CallbackContext context)
    {
        leaderPanel.gameObject.SetActive(!leaderPanel.gameObject.activeSelf);
        leaderboardDisplay.GetLeaderboard();
    }
    void Start()
    {
        SetScore(0);
    }
    public void SetScore(int score)
    {
        scoreText.text = "Score: " + score;
    }
}
