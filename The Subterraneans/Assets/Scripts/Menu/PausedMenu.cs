using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class PausedMenu : MonoBehaviour
{
    public bool paused = false;
    public GameObject pauseMenu;
    public GameObject fpsCounter;
    public TMP_Text toggleFPSOption;
    public TMP_Text toggleDifficultyText;
    public GameObject otherPlayer;
    bool easyMode = false;

    void Update()
    {
        // Toggle pause menu when pressing Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            pauseMenu.SetActive(paused);

            if (paused)
            {
                this.GetComponent<PlayerMovement>().ToggleMovementDisabled();
                otherPlayer.GetComponent<PlayerMovement>().ToggleMovementDisabled();
                Time.timeScale = 0f; // Freeze the game
            }
            else
            {
                this.GetComponent<PlayerMovement>().ToggleMovementDisabled();
                otherPlayer.GetComponent<PlayerMovement>().ToggleMovementDisabled();
                Time.timeScale = 1f; // Resume the game
            }
        }
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f; // Ensure time is resumed before quitting
        SceneManager.LoadScene(0); // Load the main menu (Scene 0)
    }

    public void ResumeGame()
    {
        paused = false;
        pauseMenu.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f; // Resume the game time
    }

    public void ToggleFPSCounter()
    {
        fpsCounter.SetActive(!fpsCounter.activeInHierarchy);
        if(fpsCounter.activeInHierarchy )
        {
            toggleFPSOption.text = "Hide FPS";
        }
        else
        {
            toggleFPSOption.text = "Show FPS";
        }
    }

    public void ToggleDifficulty()
    {
        easyMode = !easyMode;
        if (!easyMode)
        {
            toggleDifficultyText.text = "Easy Mode";
            this.GetComponent<PlayerMovement>().fallMultiplier = 2;
            otherPlayer.GetComponent<PlayerMovement>().fallMultiplier = 2;
        }
        else
        {
            toggleDifficultyText.text = "Normal Mode";
            this.GetComponent<PlayerMovement>().fallMultiplier = 10;
            otherPlayer.GetComponent<PlayerMovement>().fallMultiplier = 10;
        }
    }
}
