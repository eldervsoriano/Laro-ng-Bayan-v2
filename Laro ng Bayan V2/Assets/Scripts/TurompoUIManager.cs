using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TurompoUIManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject gameplayUI;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    [Header("Game Over UI")]
    public Text winnerText;
    public Text finalScoreText;

    [Header("Tutorial")]
    public GameObject tutorialPanel;
    public Button[] tutorialPages;
    private int currentTutorialPage = 0;

    private bool isPaused = false;

    void Start()
    {
        // Start with main menu active
        ShowMainMenu();

        // Hide all tutorial pages except the first one
        if (tutorialPages != null && tutorialPages.Length > 0)
        {
            for (int i = 1; i < tutorialPages.Length; i++)
            {
                tutorialPages[i].gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        // Check for pause key (Escape)
        if (Input.GetKeyDown(KeyCode.Escape) && !mainMenuPanel.activeSelf && !gameOverPanel.activeSelf)
        {
            TogglePause();
        }
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        gameplayUI.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        tutorialPanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        gameplayUI.SetActive(true);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        tutorialPanel.SetActive(false);

        // Tell the game manager to start the game
        if (TurompoGameManager.Instance != null)
        {
            TurompoGameManager.Instance.StartGame();
        }

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartGame()
    {
        // Reset game and start fresh
        if (TurompoGameManager.Instance != null)
        {
            TurompoGameManager.Instance.StartGame(); // Use StartGame instead of ResetGame
        }

        // UI setup
        gameplayUI.SetActive(true);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void QuitToMainMenu()
    {
        ShowMainMenu();

        // Reset the game state
        if (TurompoGameManager.Instance != null)
        {
            TurompoGameManager.Instance.StartGame(); // Use StartGame instead of ResetGame
            // We're showing the menu so we'll make the gameplay UI invisible anyway
        }
    }

    public void ShowTutorial()
    {
        mainMenuPanel.SetActive(false);
        tutorialPanel.SetActive(true);
        currentTutorialPage = 0;

        // Show only the first page
        if (tutorialPages != null && tutorialPages.Length > 0)
        {
            for (int i = 0; i < tutorialPages.Length; i++)
            {
                tutorialPages[i].gameObject.SetActive(i == 0);
            }
        }
    }

    public void NextTutorialPage()
    {
        if (tutorialPages != null && currentTutorialPage < tutorialPages.Length - 1)
        {
            tutorialPages[currentTutorialPage].gameObject.SetActive(false);
            currentTutorialPage++;
            tutorialPages[currentTutorialPage].gameObject.SetActive(true);
        }
        else
        {
            // Last page, go back to main menu
            ShowMainMenu();
        }
    }

    public void PreviousTutorialPage()
    {
        if (tutorialPages != null && currentTutorialPage > 0)
        {
            tutorialPages[currentTutorialPage].gameObject.SetActive(false);
            currentTutorialPage--;
            tutorialPages[currentTutorialPage].gameObject.SetActive(true);
        }
    }

    public void ShowGameOver(string winnerMessage, int player1Score, int player2Score)
    {
        gameplayUI.SetActive(false);
        gameOverPanel.SetActive(true);

        if (winnerText != null)
            winnerText.text = winnerMessage;

        if (finalScoreText != null)
            finalScoreText.text = $"Player 1: {player1Score} - Player 2: {player2Score}";
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}