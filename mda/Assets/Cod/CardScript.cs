using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    public int nr = 0;
    public int GetValueOfCard()
    {
        return nr;
    }

    public void SetValue(int newvalue)
    {
        nr = newvalue;
    }
    public string GetSpriteName()
    {
        return GetComponent<SpriteRenderer>().sprite.name;
    }
    public void SetSprite(Sprite newSprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
    }
    public void ResetCard()
    {
        Sprite back = GameObject.Find("Deck").GetComponent<DeckScript>().GetCardBack();
        gameObject.GetComponent<SpriteRenderer>().sprite = back;
        nr = 0;
    }
}
