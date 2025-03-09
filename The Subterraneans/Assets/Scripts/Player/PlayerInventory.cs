using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int currentMoney;
    public TMP_Text coinCounter;
    public float normalFadeDelay = 2f; // Time before fading starts
    public float fadeDuration = 1f; // Duration of fade
    public bool canUpdateMoneyCounterAlpha = true;

    private Coroutine fadeCoroutine;
    private bool isHoldingTab = false;

    private void Update()
    {
        if (canUpdateMoneyCounterAlpha)
        {
            if (Input.GetKey(KeyCode.Tab))
            {
                // Keep text fully visible while holding Tab
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isHoldingTab = true;
                SetTextAlpha(1f);

                // Stop any fade coroutine if it's running
                if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                    fadeCoroutine = null;
                }
            }
            else if (isHoldingTab)
            {
                // Start fading when Tab is released
                isHoldingTab = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                fadeCoroutine = StartCoroutine(FadeText(0));
            }
        }
    }

    public void AddMoney(int value)
    {
        currentMoney += value;
        coinCounter.text = "$" + currentMoney.ToString();

        // Keep text fully visible when money is added
        SetTextAlpha(1f);

        // Restart fade coroutine if Tab is not held
        if (!Input.GetKey(KeyCode.Tab))
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeText(normalFadeDelay));
        }
    }

    public void RemoveMoney(int value)
    {
        currentMoney -= value;
        coinCounter.text = "$" + currentMoney.ToString();

        // Keep text fully visible when money is added
        SetTextAlpha(1f);

        // Restart fade coroutine if Tab is not held
        if (!Input.GetKey(KeyCode.Tab))
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeText(normalFadeDelay));
        }
    }

    private IEnumerator FadeText(float fadeDelay)
    {
        // Wait before fading
        yield return new WaitForSeconds(fadeDelay);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            if (Input.GetKey(KeyCode.Tab))
            {
                // If Tab is pressed mid-fade, reset and stop fading
                SetTextAlpha(1f);
                yield break;
            }

            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            SetTextAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it fully fades out
        SetTextAlpha(0f);
    }

    public void SetTextAlpha(float alpha)
    {
        Color color = coinCounter.color;
        color.a = alpha;
        coinCounter.color = color;
    }
}
