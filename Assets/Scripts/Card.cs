using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int cardValue;
    public CardManager cardManager;
    public Image cardImage;
    public Button cardButton;

    private bool isFaceUp = false;
    public bool IsFaceUp => isFaceUp;
    public bool IsMatched => !cardButton.interactable;

    public void FlipCard()
    {
        if (!isFaceUp && (cardManager.firstCard == null || cardManager.secondCard == null))
            StartCoroutine(FlipAnimation(true));
    }

    public void HideCard() => StartCoroutine(FlipAnimation(false));

    private IEnumerator FlipAnimation(bool showFace)
    {
        float duration = 0.2f;

        // Play flip sound
        if (showFace)
            AudioManager.Instance?.PlayFlip();

        for (float t = 0; t < 1f; t += Time.deltaTime / duration)
        {
            transform.localScale = new Vector3(Mathf.Lerp(1f, 0f, t), 1f, 1f);
            yield return null;
        }

        isFaceUp = showFace;
        cardImage.sprite = isFaceUp ? cardManager.cardFaces[cardValue] : cardManager.cardBack;

        for (float t = 0; t < 1f; t += Time.deltaTime / duration)
        {
            transform.localScale = new Vector3(Mathf.Lerp(0f, 1f, t), 1f, 1f);
            yield return null;
        }

        if (isFaceUp && showFace)
            cardManager.CardFlipped(this);
    }


    public IEnumerator FlipCardInstantly(bool showFace)
    {
        float duration = 0.15f;
        for (float t = 0; t < 1f; t += Time.deltaTime / duration)
        {
            transform.localScale = new Vector3(Mathf.Lerp(1f, 0f, t), 1f, 1f);
            yield return null;
        }

        isFaceUp = showFace;
        cardImage.sprite = isFaceUp ? cardManager.cardFaces[cardValue] : cardManager.cardBack;

        for (float t = 0; t < 1f; t += Time.deltaTime / duration)
        {
            transform.localScale = new Vector3(Mathf.Lerp(0f, 1f, t), 1f, 1f);
            yield return null;
        }
    }

    public void ForceShow() => cardImage.sprite = cardManager.cardFaces[cardValue];
    public void ForceHide() => cardImage.sprite = cardManager.cardBack;
    public void DisableCard()
    {
        cardButton.interactable = false;
        cardImage.color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }
    public void EnableCard() => cardButton.interactable = true;
}
