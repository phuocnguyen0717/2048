using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public static event Action<int> OnScoreChanged;
    public TileBoard board;
    [SerializeField] CanvasGroup gameOverCanvasGroup;
    private int score = 0;
    [SerializeField] private Animation animationHelper;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        SetScore(0);

        gameOverCanvasGroup.interactable = false;
        NewGame();
    }
    public void NewGame()
    {
        gameOverCanvasGroup.alpha = 0f;
        gameOverCanvasGroup.interactable = true;

        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;
    }
    public void GameOver()
    {
        SetScore(0);
        board.bestScoreText.text = LoadBestScore().ToString();
        board.enabled = false;
        StartCoroutine(Fade(1f, 1f, 0.5f));
    }
    private IEnumerator Fade(float to, float delay, float duration)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(animationHelper.LerpAlpha(
        gameOverCanvasGroup, gameOverCanvasGroup.alpha, to, duration));
    }
    public void IncreaseScore(int points)
    {
        SetScore(score + points);
    }
    private void SetScore(int score)
    {
        this.score = score;
        OnScoreChanged?.Invoke(score);
        SaveBestScore();
    }
    private void SaveBestScore()
    {
        int bestScore = LoadBestScore();
        if (score > bestScore)
        {
            PlayerPrefs.SetInt("bestScore", score);
        }
    }
    public static int LoadBestScore()
    {
        return PlayerPrefs.GetInt("bestScore", 0);
    }
}
