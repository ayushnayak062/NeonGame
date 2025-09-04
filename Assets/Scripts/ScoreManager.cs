using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private int score;

    [Header("Scoring Rules")]
    public int matchPoints = 10;
    public int mismatchPenalty = -5;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private void Start()
    {
        LoadScore();
    }

    public void AddMatchPoints()
    {
        score += matchPoints;
        SaveScore();
        Debug.Log("Score: " + score);
        // later: notify UIManager
    }

    public void AddMismatchPenalty()
    {
        score += mismatchPenalty;
        SaveScore();
        Debug.Log("Score: " + score);
        // later: notify UIManager
    }

    public void ResetScore()
    {
        score = 0;
        SaveScore();
    }

    public int GetScore()
    {
        return score;
    }

    private void SaveScore()
    {
        PlayerPrefs.SetInt("playerScore", score);
        PlayerPrefs.Save();
    }

    private void LoadScore()
    {
        score = PlayerPrefs.GetInt("playerScore", 0);
    }
}
