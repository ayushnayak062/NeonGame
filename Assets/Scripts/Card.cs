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
    public Button cardButton;
    private void Start()
    {
        //isFaceUp = false;
        //cardImage.sprite = cardManager.cardBack;
    }

    public void FlipCard()
    {
        if (!isFaceUp && cardManager.firstCard == null || cardManager.secondCard == null)
        {
            /*isFaceUp = true;
            cardImage.sprite = cardManager.cardFaces[cardValue];
            cardManager.CardFlipped(this);
            Debug.Log("Card flipped! Value: " + cardValue);*/
            StartCoroutine(FlipAnimation(true));

        }
    }

    public void HideCard()
    {
        /* isFaceUp = false;
         cardImage.sprite = cardManager.cardBack;
         Debug.Log("Card hidden.");*/
        StartCoroutine(FlipAnimation(false));
    }

    private IEnumerator FlipAnimation(bool showFace)
    {
        // Shrink X to 0
        float duration = 0.2f; // speed (smaller = faster)
        for (float t = 0; t < 1f; t += Time.deltaTime / duration)
        {
            float scaleX = Mathf.Lerp(1f, 0f, t);
            transform.localScale = new Vector3(scaleX, 1f, 1f);
            yield return null;
        }

        // Switch sprite
        isFaceUp = showFace;
        cardImage.sprite = isFaceUp ? cardManager.cardFaces[cardValue] : cardManager.cardBack;

        // Expand back to 1
        for (float t = 0; t < 1f; t += Time.deltaTime / duration)
        {
            float scaleX = Mathf.Lerp(0f, 1f, t);
            transform.localScale = new Vector3(scaleX, 1f, 1f);
            yield return null;
        }

        // Notify manager only when showing face
        if (isFaceUp && showFace)
        {
            cardManager.CardFlipped(this);
            Debug.Log("Card flipped! Value: " + cardValue);
        }
    }
    public void ForceShow()
    {
        isFaceUp = true;
        cardImage.sprite = cardManager.cardFaces[cardValue];
    }

    public void ForceHide()
    {
       /* isFaceUp = false;
        cardImage.sprite = cardManager.cardBack;*/
       StartCoroutine(FlipAnimation(false));
    }
    public void DisableCard()
    {
        cardButton.interactable = false;
        if (cardImage != null)
            cardImage.color = new Color(0.5f, 0.5f, 0.5f, 1f);// lock card
    }

    public void EnableCard()
    {
        cardButton.interactable = true; // optional if you ever reset a card
    }

}
