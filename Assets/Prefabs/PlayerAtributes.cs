using Fusion;
using UnityEngine;
using UnityEngine.UI;


public class PlayerAtributes : NetworkBehaviour
{

    [Header("Health")]
    public RectTransform fill;
    public Image fillImage;

    [Header("Health")]
    [Networked, OnChangedRender(nameof(OnHealthChanged))]
    public float health { get; set; }
    [Networked]
    public float maxHealth { get; set; }

    override public void Spawned()
    {
        if (!Object.HasStateAuthority) return;
        maxHealth = 100f;
        health = maxHealth;
    }

    private void OnHealthChanged()
    {
        float h = health / maxHealth;
        fill.sizeDelta = new Vector2(h, fill.sizeDelta.y);
        if (h < 3f)
        {
            fillImage.color = Color.red;
        }
        else if (h > .5f)
        {
            fillImage.color = Color.yellow;
        }
        else
        {
            fillImage.color = Color.green;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcApplyDamage(float damage, PlayerRef attacker)
    {
        health = Mathf.Max(0, health - damage);
    }

}
