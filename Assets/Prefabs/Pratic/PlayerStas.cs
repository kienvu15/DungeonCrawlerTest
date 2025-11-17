using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStas : NetworkBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    [Networked, OnChangedRender(nameof(OnHealthChanged))]
    public int Health { get; set; }

    [Networked]
    public int MaxHealth { get; set; }

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            MaxHealth = maxHealth;
            Health = maxHealth;
        }

        UpdateUI();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcTakeDamage(int damage, PlayerRef attacker)
    {
        if (!Object.HasStateAuthority) return;

        Health = Mathf.Max(Health - damage, 0);
    }

    void OnHealthChanged()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = MaxHealth;
            healthSlider.value = Health;
        }

        if (healthText != null)
        {
            healthText.text = $"{Health}/{MaxHealth}";
        }
    }

}
