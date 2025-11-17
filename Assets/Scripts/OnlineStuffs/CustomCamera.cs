using UnityEngine;

public class CustomCamera : MonoBehaviour
{

    [SerializeField] public Transform target;
    [SerializeField] private float mouseSensitivity = 10f;

    float inputH;
    float inputV;

    float verticalRotation = 0f;
    float horizontalRotation = 0f;

    private void LateUpdate()
    {
        if(target == null) return;

        transform.position = target.position;

        inputH = Input.GetAxis("Mouse X") ;
        inputV = Input.GetAxis("Mouse Y") ;

        verticalRotation -= inputV * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        horizontalRotation += inputH * mouseSensitivity;

        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
    }

}
