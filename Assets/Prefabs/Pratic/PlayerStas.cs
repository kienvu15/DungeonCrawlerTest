using Fusion;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayerStas : NetworkBehaviour
{
    [Header("Health UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Other")]
    [SerializeField] private NetworkMecanimAnimator networkMecanimAnimator;
    [SerializeField] private ParticleSystem hitEffect;

    [Networked] public int Health { get; set; }
    [Networked] public int MaxHealth { get; set; }
    [Networked] public int Score { get; set; }

    public override void Spawned()
    {
        PlayFabPlayerStats.OnStatsLoaded += ApplyPlayFabStats;

        if (PlayFabPlayerStats.Loaded)
            ApplyPlayFabStats();

        if (Object.HasInputAuthority)
            UpdateUI();
    }



    private void ApplyPlayFabStats()
    {
        if (Object.HasStateAuthority)
        {
            Health = PlayFabPlayerStats.CachedHealth;
            MaxHealth = PlayFabPlayerStats.CachedMaxHealth;
            Score = PlayFabPlayerStats.CachedScore;
        }

        RpcUpdateUI();
    }

    // --- SCORE ---
    public void AddScore(int amount)
    {
        if (!Object.HasStateAuthority) return;

        Score += amount;

        PlayFabPlayerStats.SaveStats(Health, MaxHealth, Score);

        RpcUpdateUI();
    }





    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcTakeDamage(int damage, PlayerRef attacker)
    {
        TakeDamage(damage);

        RpcUpdateUI();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RpcPlayHurtAnimation()
    {
        networkMecanimAnimator.Animator.SetTrigger("hurt");
        hitEffect.Play();
    }

    public void TakeDamage(int amount)
    {
        Health = Mathf.Max(Health - amount, 0);
        RpcPlayHurtAnimation();

        if (Object.HasStateAuthority)
            PlayFabPlayerStats.SaveStats(Health, MaxHealth, Score);
    }


    void RpcUpdateUI()
    {
        if (Object.HasInputAuthority)
        {
            UpdateUI();  // Chỉ player local update UI của nó
        }
    }



    void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = MaxHealth;
            healthSlider.value = Health;
        }

        if (healthText != null)
            healthText.text = $"{Health}/{MaxHealth}";

        if (PreUIManager.Instance != null)
            PreUIManager.Instance.SetScore(Score);
    }


    private void OnTriggerEnter(Collider col)
    {
        if (!HasInputAuthority) return;
        if (!col.CompareTag("Coin")) return;

        RPC_RequestAddScore(20, Object.InputAuthority);

        Destroy(col.gameObject);
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_RequestAddScore(int amount, PlayerRef target)
    {
        // Tìm đúng player muốn cộng điểm
        if (Object.InputAuthority == target)
        {
            AddScore(amount);
        }
    }

}
