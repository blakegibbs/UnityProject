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
            if(targetPos != null)
            {
                StartCoroutine(MoveToTarget());
            }
        }
    }

    private IEnumerator MoveToTarget()
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
