using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int cardValue;

    public CardManager cardManager;

    private bool isFaceUp = false;
    public Image cardImage;

    private void Start()
    {
        isFaceUp = false;
        cardImage.sprite = cardManager.cardBack;
    }

    public void FlipCard()
    {
        if (!isFaceUp && cardManager.firstCard == null || cardManager.secondCard == null)
        {
            isFaceUp = true;
            cardImage.sprite = cardManager.cardFaces[cardValue];
            cardManager.CardFlipped(this);
            Debug.Log("Card flipped! Value: " + cardValue);

        }
    }

    public void HideCard()
    {
        isFaceUp = false;
        cardImage.sprite = cardManager.cardBack;
        Debug.Log("Card hidden.");
    }
}
 