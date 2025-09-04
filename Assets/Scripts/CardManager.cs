using System.Collections;
using System.Collections.Generic;
using UnityEditor.Media;
using UnityEngine;
using UnityEngine.UI;

public enum Difficulty { Easy, Medium, Hard , Insane}

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    public Card cardPrefab;
    public Sprite cardBack;
    public Sprite[] cardFaces;
    public Transform cardHolder;

    public Difficulty currentDifficulty = Difficulty.Easy;

    private List<Card> cards;
    private List<int> cardValues;
    public Card firstCard, secondCard;

    private int rows, cols;

    private int matchedPairs;
    private int totalPairs;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    void Start()
    {
        SetupLayout();
        CreateCards();
        ShuffleCards();
        Debug.Log(currentDifficulty);
    }

    void SetupLayout()
    {
        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                rows = 3; cols = 2;
                break;
            case Difficulty.Medium:
                rows = 4; cols = 3;
                break;
            case Difficulty.Hard:
                rows = 5; cols = 4;
                break;
            case Difficulty.Insane:
                rows = 5; cols = 6;
                break;
        }

        // Adjust GridLayoutGroup
        GridLayoutGroup grid = cardHolder.GetComponent<GridLayoutGroup>();
        if (grid != null)
        {
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = cols;

            // auto-size cell depending on holder
            float cellWidth = (cardHolder.GetComponent<RectTransform>().rect.width - grid.spacing.x * (cols - 1)) / cols;
            float cellHeight = (cardHolder.GetComponent<RectTransform>().rect.height - grid.spacing.y * (rows - 1)) / rows;
            /*grid.cellSize = new Vector2(cellWidth, cellHeight);*/
        }
    }
    public void SetDifficulty(int difficultyIndex)
    {
        currentDifficulty = (Difficulty)difficultyIndex;
        RestartGame();
    }


    void CreateCards()
    {
        cards = new List<Card>();
        cardValues = new List<int>();

        int totalCards = rows * cols;
        totalPairs = totalCards / 2;
        matchedPairs = 0; // reset at start

        // pick random unique card IDs
        List<int> availableFaces = new List<int>();
        for (int i = 0; i < cardFaces.Length; i++)
            availableFaces.Add(i);

        // shuffle available faces
        for (int i = 0; i < availableFaces.Count; i++)
        {
            int randomIndex = Random.Range(i, availableFaces.Count);
            int temp = availableFaces[i];
            availableFaces[i] = availableFaces[randomIndex];
            availableFaces[randomIndex] = temp;
        }

        // take only the required number of pairs
        for (int i = 0; i < totalPairs; i++)
        {
            cardValues.Add(availableFaces[i]);
            cardValues.Add(availableFaces[i]);
        }

        // now instantiate cards
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
        {
            cards[i].cardValue = cardValues[i];
        }
    }

    public void CardFlipped(Card flippedCard)
    {
        if (firstCard == null)
        {
            firstCard = flippedCard;
        }
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
            Debug.Log("Match Found!");
            matchedPairs++;
            ScoreManager.instance.AddMatchPoints();

            // Check for win
            if (matchedPairs >= totalPairs)
            {
                OnWin();
            }

            firstCard = null;
            secondCard = null;
        }
        else
        {
            Debug.Log("No Match.");
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

    public void RestartGame()
    {
        // Destroy old cards
        foreach (Transform child in cardHolder)
            Destroy(child.gameObject);

        // Reset state
        firstCard = null;
        secondCard = null;

        // Re-setup
        SetupLayout();
        CreateCards();
        ShuffleCards();
        
    }
  

    IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RestartGame();
    }

    void OnWin()
    {
        Debug.Log("🎉 You Win!");
        StartCoroutine(RestartAfterDelay(2f));
        // TODO: show win UI, play sound, restart menu, etc.
    }
}

