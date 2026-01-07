//using UnityEngine;
//using Fusion;

//public class SpawnItemFromSky : NetworkBehaviour
//{
//    Vector3 bottm = new Vector3(15f, 7, 15f);
//    Vector3 up = new Vector3(-15f, 7, -15f);
//    bool begin;
//    float timer;
//    bool time2Spawn = false;

//    public GameObject chestPrefab;

//    public override void Spawned()
//    {
//        //if (HasStateAuthority) begin = true;
//    }

//    void Update()
//    {
//        if(GameManager.instacne.allplayer.Count > 0)
//        {
//            timer += Time.deltaTime;
//            if(timer > 5f)
//            {
//                time2Spawn = true;
//                timer = 0f;
//            }
//        }
//    }


//    public override void FixedUpdateNetwork()
//    {
//        if (!Object.HasStateAuthority)
//            return;
//        if (time2Spawn)
//        {
//            Vector3 ramdomPos = new Vector3(
//                Random.Range(bottm.x, up.x),
//                Random.Range(bottm.y, up.y),
//                Random.Range(bottm.z, up.z)
//                );

//            Runner.Spawn(chestPrefab, ramdomPos, Quaternion.identity);
//            time2Spawn = false;
//        }
//    }
//}
