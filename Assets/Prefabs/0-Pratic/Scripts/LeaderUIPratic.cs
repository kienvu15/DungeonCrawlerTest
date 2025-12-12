using UnityEngine;

public class LeaderUIPratic : MonoBehaviour
{

    private void OnEnable()
    {
        var leaderboard = GameManagerPratic.instance.GetLeaderBoard();

        foreach (Transform t in GameManagerPratic.instance.content) Destroy(t.gameObject);

        foreach (var p in leaderboard)
        {
            var obj = Instantiate(GameManagerPratic.instance.entryPrefab, GameManagerPratic.instance.content);
            obj.GetComponent<TMPro.TextMeshProUGUI>().text =
                $"Player {p.Object.InputAuthority.PlayerId} : {p.Score}";
        }
    }

}
