using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    public Sprite[] carti;
    int[] valcarti=new int[53];
    int ok = 0;
    void Start()
    {
        GetCardValue();
    }

    
    void GetCardValue()//functia ii da vectorului de nr valorile corescpuzatoare (ex J de romb are e pe poz 27 atunci valcarti[27]=10)
    {
        int num = 0;
        for(int i=0;i<carti.Length;i++)
        { num = i;
            num = num % 13;
            if (num > 10 || num == 0)
                num = 10;
            valcarti[i] = num++;
        
        }
        
    }
    public void Shuffle()//amestecam pachetul intr o ordine random
    {
        for (int i = carti.Length - 1; i >=1; i--)
        {
            int j = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * carti.Length - 1) + 1;//j primeste o val ranom pe intervalul [1,52]
            if (i != 0 && j != 0)
            {
                Sprite b = carti[i];
                carti[i] = carti[j];
                carti[j] = b;

                int a = valcarti[i];
                valcarti[i] = valcarti[j];
                valcarti[j] = a;
            }
        }
        ok = 1;
    }
    public int DealCard(CardScript cardscript)
    {
        cardscript.SetSprite(carti[ok]);
        cardscript.SetValue(valcarti[ok]);
        ok++;
        return cardscript.GetValueOfCard();
        
    }
    public Sprite GetCardBack()
    {
        return carti[0];
    }
}
