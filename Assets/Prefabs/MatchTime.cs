using Fusion;
using TMPro;
using UnityEngine;

public class MatchTimer : NetworkBehaviour
{
    public TextMeshProUGUI textUI;
    [Networked, OnChangedRender(nameof(OnChangeTimer))]
    public float timer { get; set; }
    int seconds;
    int minutes;
    public override void Spawned()
    {
        if (HasStateAuthority) timer = 1000f;
    }
    void OnChangeTimer()
    {
        UpdateTimerUI();
    }
    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority && timer > 0)
{
            timer -= Runner.DeltaTime;
        }
    }
    void UpdateTimerUI()
    {
        minutes = Mathf.FloorToInt(timer / 60);
        seconds = Mathf.FloorToInt(timer % 60);
        textUI.text = minutes.ToString("D2") + " : " + seconds.ToString("D2");
    }
}
