using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public bool followX = true;
    public bool followY = true;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = transform.position;

        if (followX)
            desiredPosition.x = target.position.x + offset.x;
        if (followY)
            desiredPosition.y = target.position.y + offset.y;

        desiredPosition.z = offset.z;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
}