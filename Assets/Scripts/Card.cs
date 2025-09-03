using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] Image card;
    [SerializeField] Sprite a;
    [SerializeField] Sprite b;

    public void OnMouseDown()
    {
        card.sprite = card.sprite == a ? b : a; 
    }
    
}
 