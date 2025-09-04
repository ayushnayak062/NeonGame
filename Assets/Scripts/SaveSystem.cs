using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int[] cardValues;
    public bool[] cardIsFaceUp;
    public bool[] cardIsMatched;
    public int matchedPairs;
    public int totalPairs;
    public int score;
    public int combo;
    public int difficultyIndex;
}

public static class SaveSystem
{
    private static string savePath => Application.persistentDataPath + "/save.json";

    public static void SaveGame()
    {
        SaveData data = new SaveData();
        CardManager cm = CardManager.instance;

        int count = cm.Cards.Count;
        data.cardValues = new int[count];
        data.cardIsFaceUp = new bool[count];
        data.cardIsMatched = new bool[count];

        for (int i = 0; i < count; i++)
        {
            Card c = cm.Cards[i];
            data.cardValues[i] = c.cardValue;
            data.cardIsFaceUp[i] = c.IsFaceUp;
            data.cardIsMatched[i] = c.IsMatched;
        }

        data.matchedPairs = cm.MatchedPairs;
        data.totalPairs = cm.TotalPairs;
        data.score = ScoreManager.instance.GetScore();
        data.combo = ScoreManager.instance.GetCombo();
        data.difficultyIndex = (int)cm.currentDifficulty;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }

    public static void LoadGame()
    {
        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        CardManager cm = CardManager.instance;
        ScoreManager sm = ScoreManager.instance;

        cm.isLoading = true;

        // Set difficulty first
        cm.currentDifficulty = (Difficulty)data.difficultyIndex;
        sm.ResetScore();
        sm.AddMatchPoints(); // optional: set initial points if needed

        cm.RestartGame(showPreview: false); // cards recreated, no preview

        // Now restore card states
        for (int i = 0; i < cm.Cards.Count && i < data.cardValues.Length; i++)
        {
            Card c = cm.Cards[i];
            c.cardValue = data.cardValues[i];

            if (data.cardIsMatched[i])
            {
                c.DisableCard();
                c.StartCoroutine(c.FlipCardInstantly(true));
            }
            else if (data.cardIsFaceUp[i])
                c.StartCoroutine(c.FlipCardInstantly(true));
            else
                c.StartCoroutine(c.FlipCardInstantly(false));
        }

        cm.MatchedPairs = data.matchedPairs;
        cm.TotalPairs = data.totalPairs;

        sm.ResetScore();
        sm.AddMatchPoints(); // optional: restore actual score
        cm.isLoading = false;
    }

    public static void ClearSave()
    {
        if (File.Exists(savePath)) File.Delete(savePath);
    }
}
