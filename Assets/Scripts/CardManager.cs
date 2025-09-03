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
    }

    void CreateCards()
    {
        for (int i = 0; i < cardFaces.Length/2; i++)
        {
            cardValues.Add(i);
            cardValues.Add(i);
        }
        foreach(int id in cardValues)
        {
            Card newCard = Instantiate(cardPrefab, cardHolder);
            newCard.cardManager = this;
            newCard.cardValue = id;
            cards.Add(newCard);
        }
    }
}

