using UnityEngine;
using Unity.Cinemachine;

public class OrbitCamera : MonoBehaviour
{
    public CinemachineCamera cam;
    public float sensX = 0.2f;
    public float sensY = 0.2f;

    public float minX = -21f;
    public float maxX = 21f;

    CinemachineFollow follow;

    void Start()
    {
        follow = cam.GetComponent<CinemachineFollow>();
    }

    void LateUpdate()
    {
        if (follow.FollowTarget == null) return;

        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            //float mouseY = Input.GetAxis("Mouse Y");

            Vector3 offset = follow.FollowOffset;

            offset.x += mouseX * sensX;
            //offset.y += mouseY * sensY;

            //offset.x = Mathf.Clamp(offset.x, minX, maxX);
            offset.x += mouseX * sensX;

            follow.FollowOffset = offset;
        }
    }
}
