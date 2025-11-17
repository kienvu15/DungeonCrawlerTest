using UnityEngine;
using Fusion;

public class LookAtCamera : NetworkBehaviour
{
    private void LateUpdate()
    {
        Vector3 dir = transform.position - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
