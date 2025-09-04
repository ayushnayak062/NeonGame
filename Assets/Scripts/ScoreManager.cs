using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private int score;
    private int comboStreak;

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
        ResetScore();
    }

    public void AddMatchPoints()
    {
        comboStreak++; // increase streak
        int pointsToAdd = matchPoints * comboStreak; // multiplier effect
        score += pointsToAdd;

        // make sure score never goes below 0
        score = Mathf.Max(score, 0);

        Debug.Log($"Match! +{pointsToAdd} points (Combo x{comboStreak}) | Score: {score}");
        // later: notify UIManager
    }

    public void AddMismatchPenalty()
    {
        comboStreak = 0; // reset streak
        score += mismatchPenalty;

        // clamp to 0
        score = Mathf.Max(score, 0);

        Debug.Log($"Mismatch! {mismatchPenalty} points | Score: {score}");
        // later: notify UIManager
    }

    public void ResetScore()
    {
        score = 0;
        comboStreak = 0;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetCombo()
    {
        return comboStreak;
    }
}
