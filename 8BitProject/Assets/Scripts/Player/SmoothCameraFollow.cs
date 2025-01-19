using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("The target the camera will follow (e.g., the player).")]
    public Transform target;

    [Header("Camera Settings")]
    [Tooltip("How smoothly the camera follows the target.")]
    [Range(0.01f, 10f)] public float followSpeed = 5f;

    [Tooltip("Offset from the target position.")]
    public Vector3 offset;

    [Header("Bounds (Optional)")]
    [Tooltip("Enable bounds to restrict camera movement.")]
    public bool useBounds = false;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("SmoothCameraFollow: No target assigned!");
            return;
        }

        // Calculate the target position with offset
        Vector3 targetPosition = target.position + offset;

        // Smoothly interpolate between current position and target position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Apply bounds if enabled
        if (useBounds)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);
        }

        // Update the camera position
        transform.position = smoothedPosition;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the camera bounds in the editor (if bounds are enabled)
        if (useBounds)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(minBounds.x, minBounds.y, 0), new Vector3(minBounds.x, maxBounds.y, 0));
            Gizmos.DrawLine(new Vector3(minBounds.x, maxBounds.y, 0), new Vector3(maxBounds.x, maxBounds.y, 0));
            Gizmos.DrawLine(new Vector3(maxBounds.x, maxBounds.y, 0), new Vector3(maxBounds.x, minBounds.y, 0));
            Gizmos.DrawLine(new Vector3(maxBounds.x, minBounds.y, 0), new Vector3(minBounds.x, minBounds.y, 0));
        }
    }
}
