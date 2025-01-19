using UnityEngine;

public class RopeGenerator : MonoBehaviour
{
    [Header("Rope Settings")]
    public GameObject ropeSegmentPrefab; // Prefab for each rope segment
    public int segmentCount = 10;        // Number of segments in the rope
    public Transform startPoint;        // Starting point of the rope
    public Transform endPoint;          // Optional endpoint

    private void Start()
    {
        GenerateRope();
    }

    void GenerateRope()
    {
        if (ropeSegmentPrefab == null || startPoint == null)
        {
            Debug.LogError("Rope segment prefab or start point is missing!");
            return;
        }

        Vector3 segmentPosition = startPoint.position;
        GameObject previousSegment = null;

        for (int i = 0; i < segmentCount; i++)
        {
            // Create a new rope segment
            GameObject segment = Instantiate(ropeSegmentPrefab, segmentPosition, Quaternion.identity, transform);

            // Connect to the previous segment
            if (previousSegment != null)
            {
                HingeJoint2D hinge = segment.GetComponent<HingeJoint2D>();
                hinge.connectedBody = previousSegment.GetComponent<Rigidbody2D>();
            }
            else
            {
                // Connect the first segment to the start point
                HingeJoint2D hinge = segment.GetComponent<HingeJoint2D>();
                hinge.connectedAnchor = startPoint.position;
                hinge.autoConfigureConnectedAnchor = false;
            }

            // Update for the next segment
            previousSegment = segment;
            segmentPosition.y -= segment.GetComponent<SpriteRenderer>().bounds.size.y;
        }

        if (endPoint != null)
        {
            // Attach the last segment to the end point
            HingeJoint2D hinge = previousSegment.GetComponent<HingeJoint2D>();
            hinge.connectedAnchor = endPoint.position;
            hinge.autoConfigureConnectedAnchor = false;
        }
    }
}
