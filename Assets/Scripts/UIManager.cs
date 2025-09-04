using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD")]
    [SerializeField] private TMP_Text scoreText; // or TMP_Text
    [SerializeField] private TMP_Text comboText; // or TMP_Text
    [SerializeField] private TMP_Text pairsText; // or TMP_Text

    [Header("Overlays")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text finalScoreText; // or TMP_Text

    [Header("Controls")]
    [SerializeField] private UnityEngine.UI.Toggle muteToggle;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        // Basic null guards
        if (scoreText == null) Debug.LogError("UIManager: scoreText is not assigned");
        if (comboText == null) Debug.LogError("UIManager: comboText is not assigned");
        if (pairsText == null) Debug.LogError("UIManager: pairsText is not assigned");
        if (winPanel == null) Debug.LogError("UIManager: winPanel is not assigned");
    }

    private void Start()
    {
        HideWinImmediate();
        RefreshAll();
    }

    // Called by ScoreManager after score changes
    public void UpdateScore(int score, int combo)
    {
        if (scoreText) scoreText.text = $"Score: {score}";
        if (comboText)
        {
            if (combo > 0) comboText.text = $"Combo: x{combo}";
            else comboText.text = "";
        }
    }

    // Called by CardManager when matchedPairs or totalPairs changes
    public void UpdatePairs(int matchedPairs, int totalPairs)
    {
        if (pairsText) pairsText.text = $"{matchedPairs}/{totalPairs} Pairs";
    }

    // Called by CardManager on win
    public void ShowWin(int finalScore)
    {
        if (finalScoreText) finalScoreText.text = $"Final Score: {finalScore}";
        if (winPanel) winPanel.SetActive(true);
    }

    public void HideWinImmediate()
    {
        if (winPanel) winPanel.SetActive(false);
    }

    private void SetCanvasGroup(CanvasGroup cg, bool visible)
    {
        if (!cg) return;
        cg.alpha = visible ? 1f : 0f;
        cg.interactable = visible;
        cg.blocksRaycasts = visible;
    }

    private void RefreshAll()
    {
        // Initial UI sync on scene load
        int score = ScoreManager.instance != null ? ScoreManager.instance.GetScore() : 0;
        int combo = ScoreManager.instance != null ? ScoreManager.instance.GetCombo() : 0;
        UpdateScore(score, combo);

        // If CardManager exposes accessors, use them; otherwise keep pairsText blank at boot
        UpdatePairs(0, 0);
    }

    // UI wiring
    public void OnRestartClicked()
    {
        HideWinImmediate();
        if (ScoreManager.instance != null) ScoreManager.instance.ResetScore();
        CardManager.instance?.RestartGame();
    }

    public void OnNewGameClicked()
    {
        HideWinImmediate();
        ScoreManager.instance?.ResetScore();
        CardManager.instance?.RestartGame();
        SaveSystem.ClearSave();
        AudioManager.Instance?.PlayNewGame();
    }

    public void OnDifficultyChanged(int index)
    {
        HideWinImmediate();
        CardManager.instance?.SetDifficulty(index);
    }

    public void OnSaveClicked()
    {
        SaveSystem.SaveGame();
        AudioManager.Instance?.PlaySave();
    }

    public void OnLoadClicked()
    {
        SaveSystem.LoadGame();
        AudioManager.Instance?.PlayLoad();
    }
   
}
