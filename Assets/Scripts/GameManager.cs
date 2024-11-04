using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public static event Action<int> OnScoreChanged;
    public TileBoard board;
    [SerializeField] CanvasGroup gameOver;
    private int score = 0;
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

        gameOver.interactable = false;
        NewGame();
    }
    public void NewGame()
    {
        gameOver.alpha = 0f;
        gameOver.interactable = true;

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
        StartCoroutine(Fade(gameOver, 1f, 1f));
    }
    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay)
    {
        yield return new WaitForSeconds(delay);
        float from = canvasGroup.alpha;
        float elapsed = 0f;
        float duration = 0.5f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = to;
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
