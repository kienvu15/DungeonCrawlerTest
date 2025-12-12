using TMPro;
using UnityEngine;

public class UIManagerPratic : MonoBehaviour
{
    public static UIManagerPratic instance;

    public TextMeshProUGUI score;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateScore(int value)
    {
        score.text = "Score: " + value.ToString();
    }
}
