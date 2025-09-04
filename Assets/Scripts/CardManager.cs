using System.Collections;
using System.Collections.Generic;
using UnityEditor.Media;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    public Card cardPrefab;

    public Sprite cardBack;
    public Sprite[] cardFaces;
    private List<Card> cards;
    private List<int> cardValues;
    public Card firstCard, secondCard;
    public Transform cardHolder;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        cards = new List<Card>();
        cardValues = new List<int>();
        CreateCards();
        ShuffleCards();
    }

    void CreateCards()
    {
        for (int i = 0; i < cardFaces.Length / 2; i++)
        {
            cardValues.Add(i);
            cardValues.Add(i);
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
            firstCard = null;
            secondCard = null;
        }
        else
        {
            Debug.Log("No Match.");
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

