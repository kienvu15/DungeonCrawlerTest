using Fusion;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : NetworkBehaviour
{
    public List<LeaderBoardRowInfo> leaderBoard()
    {
        List<LeaderBoardRowInfo> scorelist = new List<LeaderBoardRowInfo>();

        if(Runner == null) return scorelist;

        foreach (var player in Runner.GetAllNetworkObjects())
        {
            if (player.TryGetComponent<MoveTest>(out MoveTest moveTest))
            {
                if (moveTest.Object.IsValid)
                {
                    LeaderBoardRowInfo rowInfo = new LeaderBoardRowInfo
                    {
                        id = moveTest.Object.InputAuthority.PlayerId,
                        score = moveTest.score
                    };
                    scorelist.Add(rowInfo);
                }
            }
        }

        scorelist = scorelist.OrderByDescending(s => s.score).ToList();
        return scorelist;
    }
}

[System.Serializable]
public class LeaderBoardRowInfo
{
    public int id;
    public float score;
}
