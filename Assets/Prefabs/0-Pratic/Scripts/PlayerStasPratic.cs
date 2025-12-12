using UnityEngine;
using Fusion;
using UnityEngine.UI;
using TMPro;

public class PlayerStasPratic : NetworkBehaviour
{
    [Header("Stats")]
    [Networked, OnChangedRender(nameof(OnScoreChanged))] public int Score {  get; set; }
    [Networked, OnChangedRender(nameof(OnHealthChanged))] public int Health { get; set; } = 100;
    public int damage = 10;

    [Header("Refer")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public NetworkMecanimAnimator playerMecanimAnimator;

    public override void Spawned()
    {
        GameManagerPratic.instance.RegisterPlayer(this);
        UpdateUI();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcTakeDamage(int dmg)
    {
        Health = Mathf.Max(Health-dmg, 0);
    }

    private void OnHealthChanged()
    {
        UpdateUI();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RpcRequestAddScore(int amount)
    {
        Score += amount;
    }

    private void OnScoreChanged()
    {
        if (Object.HasInputAuthority)
        {
            UIManagerPratic.instance.UpdateScore(Score);
        }
    }

    private void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = 100;
            healthSlider.value = Health;
            healthText.text = Health.ToString();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Object.HasInputAuthority)
        {
            if (other.CompareTag("Coin"))
            {
                RpcRequestAddScore(10);
            }
        }
    }
}
