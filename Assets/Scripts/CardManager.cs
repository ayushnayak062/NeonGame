using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Difficulty { Easy, Medium, Hard, Insane }

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    public Card cardPrefab;
    public Sprite cardBack;
    public Sprite[] cardFaces;
    public Transform cardHolder;

    public Difficulty currentDifficulty = Difficulty.Easy;

    private List<Card> cards;
    public List<Card> Cards => cards;

    private List<int> cardValues;

    public Card firstCard, secondCard;
    private int rows, cols;

    private int matchedPairs;
    public int MatchedPairs { get => matchedPairs; set => matchedPairs = value; }
    private int totalPairs;
    public int TotalPairs { get => totalPairs; set => totalPairs = value; }

    [HideInInspector]
    public bool isLoading = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start() => RestartGame();

    public void SetDifficulty(int index)
    {
        currentDifficulty = (Difficulty)index;
        RestartGame();
    }

    public void RestartGame(bool showPreview = true)
    {
        if (!isLoading)
        {
            foreach (Transform child in cardHolder)
                Destroy(child.gameObject);
        }

        firstCard = null;
        secondCard = null;
        matchedPairs = 0;

        if (!isLoading)
        {
            SetupLayout();
            CreateCards();
            ShuffleCards();
        }

        UIManager.Instance?.UpdatePairs(matchedPairs, totalPairs);
        UIManager.Instance?.UpdateScore(ScoreManager.instance?.GetScore() ?? 0,
                                        ScoreManager.instance?.GetCombo() ?? 0);
        UIManager.Instance?.HideWinImmediate();

        if (showPreview && !isLoading)
            StartCoroutine(PreviewCards(.5f));
    }

    void SetupLayout()
    {
        switch (currentDifficulty)
        {
            case Difficulty.Easy: rows = 3; cols = 2; break;
            case Difficulty.Medium: rows = 4; cols = 3; break;
            case Difficulty.Hard: rows = 5; cols = 4; break;
            case Difficulty.Insane: rows = 5; cols = 6; break;
        }

        GridLayoutGroup grid = cardHolder.GetComponent<GridLayoutGroup>();
        if (grid != null)
        {
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = cols;
        }
    }

    void CreateCards()
    {
        cards = new List<Card>();
        cardValues = new List<int>();

        int totalCards = rows * cols;
        totalPairs = totalCards / 2;

        List<int> availableFaces = new List<int>();
        for (int i = 0; i < cardFaces.Length; i++) availableFaces.Add(i);

        for (int i = 0; i < availableFaces.Count; i++)
        {
            int randomIndex = Random.Range(i, availableFaces.Count);
            int temp = availableFaces[i];
            availableFaces[i] = availableFaces[randomIndex];
            availableFaces[randomIndex] = temp;
        }

        for (int i = 0; i < totalPairs; i++)
        {
            cardValues.Add(availableFaces[i]);
            cardValues.Add(availableFaces[i]);
        }

        foreach (int id in cardValues)
        {
            Card newCard = Instantiate(cardPrefab, cardHolder);
            newCard.cardManager = this;
            newCard.cardValue = id;
            cards.Add(newCard);
        }
    }

    void ShuffleCards()
    {
        for (int i = 0; i < cardValues.Count; i++)
        {
            int randomIndex = Random.Range(i, cardValues.Count);
            int temp = cardValues[i];
            cardValues[i] = cardValues[randomIndex];
            cardValues[randomIndex] = temp;
        }

        for (int i = 0; i < cards.Count; i++)
            cards[i].cardValue = cardValues[i];
    }

    IEnumerator PreviewCards(float delay)
    {
        foreach (Card card in cards)
            card.ForceShow();

        yield return null;
        yield return new WaitForSeconds(delay);

        foreach (Card card in cards)
            card.ForceHide();
    }

    public void CardFlipped(Card flippedCard)
    {
        if (firstCard == null) firstCard = flippedCard;
        else if (secondCard == null && flippedCard != firstCard)
        {
            secondCard = flippedCard;
            CheckMatching();
        }
    }

    void CheckMatching()
    {
        if (firstCard.cardValue == secondCard.cardValue)
        {
            AudioManager.Instance?.PlayMatch();

            matchedPairs++;
            ScoreManager.instance.AddMatchPoints();
            UIManager.Instance?.UpdatePairs(matchedPairs, totalPairs);

            firstCard.DisableCard();
            secondCard.DisableCard();

            firstCard = null;
            secondCard = null;

            if (matchedPairs >= totalPairs)
            {
                AudioManager.Instance?.PlayGameOver();
                UIManager.Instance?.ShowWin(ScoreManager.instance.GetScore());
                SaveSystem.ClearSave();
            }
        }
        else
        {
            AudioManager.Instance?.PlayMismatch();
            ScoreManager.instance.AddMismatchPenalty();
            StartCoroutine(UnflipCards());
        }
    }


    IEnumerator UnflipCards()
    {
        yield return new WaitForSeconds(1f);
        firstCard.HideCard();
        secondCard.HideCard();
        firstCard = null;
        secondCard = null;
    }
}
