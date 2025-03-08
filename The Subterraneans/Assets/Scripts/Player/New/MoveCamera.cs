using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPos;
    public Transform targetPos;
    public float moveSpeed = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ZoneTransition"))
        {
            targetPos = collision.transform;
            if (targetPos != null)
            {
                StartCoroutine(MoveToTarget(targetPos));
            }
        }
        else if (collision.CompareTag("CameraFollowZoneX"))
        {
            if(cameraPos.GetComponent<CameraFollow>() == null)
            {
                targetPos = null;
                CameraFollow followScript = cameraPos.gameObject.AddComponent<CameraFollow>();
                followScript.followX = true;
                followScript.target = transform;
                followScript.followY = true;
            }
        }
        else if (collision.CompareTag("CameraFollowZoneY"))
        {
            if(cameraPos.GetComponent<CameraFollow>()  == null)
            {
                targetPos = null;
                CameraFollow followScript = cameraPos.gameObject.AddComponent<CameraFollow>();
                followScript.followX = true;
                followScript.target = transform;
                followScript.followY = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("CameraFollowZoneX"))
        {
            CameraFollow followScript = cameraPos.gameObject.GetComponent<CameraFollow>();
            Destroy(followScript);

        }
        else if (collision.CompareTag("CameraFollowZoneY"))
        {
            CameraFollow followScript = cameraPos.gameObject.GetComponent<CameraFollow>();
            Destroy(followScript);
        }
    }

    private IEnumerator MoveToTarget(Transform target)
    {
        while (Vector2.Distance(cameraPos.position, targetPos.position) > 0.01f)
        {
            transform.GetComponent<CameraShake>().isMoving = true;

            float speed = moveSpeed;
            Vector3 newPosition = Vector2.Lerp(cameraPos.position, targetPos.position, speed * Time.deltaTime);
            cameraPos.position = new Vector3(newPosition.x, newPosition.y, -10f);

            yield return null;
        }

        transform.GetComponent<CameraShake>().NewPosition(new Vector3(targetPos.position.x, targetPos.position.y, -10));
        cameraPos.position = new Vector3(targetPos.position.x, targetPos.position.y, -10f);
    }
}
