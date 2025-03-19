using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    [SerializeField] private int startingLives = 3; // starting number of lives
    private int currentLives; // track current lives

    [SerializeField] private Ball ball;
    [SerializeField] private Transform bricksContainer;
    [SerializeField] private ScoreCounterUI scoreCounter;

    [SerializeField] private List<GameObject> heartObjects; // list of heart GameObjects (instead of just Images)
    [SerializeField] private GameObject gameOverPanel; // game over UI panel

    private int currentBrickCount;
    private int totalBrickCount;

    private int score = 0;

    private void OnEnable()
    {
        InputHandler.Instance.OnFire.AddListener(FireBall);
        ball.ResetBall();
        totalBrickCount = bricksContainer.childCount;
        currentBrickCount = bricksContainer.childCount;
        currentLives = startingLives; // initialize current lives
        UpdateHeartsUI(); // update heart icons when game starts
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnFire.RemoveListener(FireBall);
    }

    private void FireBall()
    {
        ball.FireBall();
    }

    public void OnBrickDestroyed(Vector3 position)
    {
        // fire audio here
        // implement particle effect here
        // add camera shake here
        currentBrickCount--;
        AddScore(1);
        Debug.Log($"Destroyed Brick at {position}, {currentBrickCount}/{totalBrickCount} remaining");
        if (currentBrickCount == 0) SceneHandler.Instance.LoadNextScene();
    }

    public void KillBall()
    {
        currentLives--;
        // update lives on HUD here
        UpdateHeartsUI();
        // game over UI if currentLives <= 0, show Game Over screen
        if (currentLives <= 0)
        {
            StartCoroutine(GameOverSequence());
        }
        else
        {
            ball.ResetBall();
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreCounter.UpdateScore(score);
    }

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < heartObjects.Count; i++)
        {
            heartObjects[i].SetActive(i < currentLives);
        }
    }

    private IEnumerator GameOverSequence()
    {
        Time.timeScale = 0f; // Freeze game
        gameOverPanel.SetActive(true); // show game over UI
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 1f; // reset time
        SceneHandler.Instance.LoadMenuScene();
    }
}
