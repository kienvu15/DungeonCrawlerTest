using NUnit.Framework.Internal.Execution;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameManager gameManager;
    public Transform panel;
    public Kien inoutAction;
    public List<TextMeshProUGUI> menus = new List<TextMeshProUGUI>();

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
        inoutAction = new Kien();
        inoutAction.Enable();
        inoutAction.UI.LeaderBoard.started += ToggleMenu;
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        
        panel.gameObject.SetActive(!panel.gameObject.activeSelf);
    }

    private void OnDisable()
    {
        inoutAction.Disable();
    }

    private void Start()
    {
        menus = new List<TextMeshProUGUI>();
        foreach (Transform child in panel)
        {
            menus.Add(child.GetComponent<TextMeshProUGUI>());
        }

    }

    public void SetText(List<LeaderBoardRowInfo> list)
    {
        if(list == null) return;
        for (int i = 0; i < list.Count; i++)
        {
            if (i < list.Count)
            {
                menus[i].text = $"{list[i].id}: {list[i].score}";
            }
            else
            {
                menus[i].text = "";
            }
        }
    }

}
