using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToNpc : MonoBehaviour
{
    public GameObject UI;
    private bool playerInTrigger = false;
    public GameObject player;
    public Transform cameraPos;
    public float moveSpeed;
    private float originalFOV;
    private Vector3 previousCameraPos; // Changed to dynamically track previous position
    public float yOffset = 1f;
    bool canMove = true;
    public float fovDivider = 1.7f;

    private void Start()
    {
        originalFOV = cameraPos.gameObject.GetComponent<Camera>().fieldOfView;
        previousCameraPos = cameraPos.position; // Initialize with starting position
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            this.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
            UI.SetActive(false);
            player.GetComponent<MoveCamera>().enabled = false;
            if (canMove)
            {
                StopAllCoroutines();
                StartCoroutine(MoveToTarget(this.transform));
                canMove = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInTrigger = true;
            UI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInTrigger = false;
            UI.SetActive(false);
            this.SendMessage("EndInteraction", SendMessageOptions.DontRequireReceiver);
            player.GetComponent<MoveCamera>().enabled = true;

            if (!canMove)
            {
                StopAllCoroutines();
                StartCoroutine(ReturnToPreviousPosition()); // Changed to return to the stored position
                canMove = true;
            }
        }
    }

    private IEnumerator MoveToTarget(Transform target)
    {
        previousCameraPos = cameraPos.position; // Store the camera's position before moving
        Camera cam = cameraPos.gameObject.GetComponent<Camera>();
        float targetFOV = originalFOV / fovDivider;

        while (Vector2.Distance(cameraPos.position, transform.position) > 0.01f)
        {
            player.transform.GetComponent<CameraShake>().isMoving = true;

            float speed = moveSpeed;
            Vector3 targetPos = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
            Vector3 newPosition = Vector2.Lerp(cameraPos.position, targetPos, speed * Time.deltaTime);
            cameraPos.position = new Vector3(newPosition.x, newPosition.y, -10f);

            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, speed * Time.deltaTime);

            yield return null;
        }

        transform.GetComponent<CameraShake>().NewPosition(new Vector3(transform.position.x, transform.position.y, -10));
        cameraPos.position = new Vector3(transform.position.x, transform.position.y, -10f);
        cam.fieldOfView = targetFOV;
    }

    private IEnumerator ReturnToPreviousPosition()
    {
        Camera cam = cameraPos.gameObject.GetComponent<Camera>();
        float timeOut = 2f;
        float elapsedTime = 0f;

        while (Vector2.Distance(cameraPos.position, previousCameraPos) > 0.01f && elapsedTime < timeOut)
        {
            player.transform.GetComponent<CameraShake>().isMoving = true;

            float speed = moveSpeed;
            Vector3 newPosition = Vector3.Lerp(cameraPos.position, previousCameraPos, speed * Time.deltaTime);
            cameraPos.position = new Vector3(newPosition.x, newPosition.y, -10f);

            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, originalFOV, speed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position/values are set correctly
        player.transform.GetComponent<CameraShake>().NewPosition(previousCameraPos);
        cameraPos.position = previousCameraPos;
        cam.fieldOfView = originalFOV;

        player.transform.GetComponent<CameraShake>().isMoving = false;
    }
}
