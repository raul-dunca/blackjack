using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserScript : MonoBehaviour
{
    //acces la restul codului
    public CardScript cardscript;
    public DeckScript deckscript;
    // valoarea mainii
    public int handvalue = 0;
    //suma totala
    private int money = 5000;
    //vectorul retine cartile de pe masa
    public GameObject[] hand;

    public int aux = 0;
   
    //tinem cont daca as e 1 sau 11
    List<CardScript> aceList = new List<CardScript>();
    public void Starthand()
    {
        GetCard();
        GetCard();
    }

    //arata cartea pe masa
    public int GetCard()
    {
        //alege o carte
        int cardValue = deckscript.DealCard(hand[aux].GetComponent<CardScript>());
        //arata cartea pe ecran
        hand[aux].GetComponent<Renderer>().enabled = true;
        //adaugam valorea carti la totalul mainii
        handvalue += cardValue;
        //verific daca e as si daca e il bagam in lista
        if(cardValue==1)
        {
            aceList.Add(hand[aux].GetComponent<CardScript>());
        }
        AceCheck();
        aux++;
        return handvalue;
    }

    public void AceCheck()
    {
        foreach(CardScript ace in aceList)
        {
            if(handvalue +10 < 22 && ace.GetValueOfCard()==1)//verficam daca in loc de valoarea asului 1 ar fi 11 si daca e sub 22 inseamna ca o facem 11(valoarea asului)
            {
                ace.SetValue(11);
                handvalue = handvalue + 10;
            }else if(handvalue>21 && ace.GetValueOfCard()==11)//daca avem peste 21 dar avem un as de valoare 11 schimbam valoarea lui in 1
            {
                ace.SetValue(1);
                handvalue = handvalue - 10;
            }
        }
    }
    // adauga sau scade bani pierduti/castigati
    public void AdjustMoney(int amount)
    {
        money += amount;

    }
    public int GetMoney()
    {
        return money;
    }
    //resetare runda (intoarcem cartile)
    public void ResetHand()
    {
        for (int i=0;i<hand.Length;i++)
        {
            hand[i].GetComponent<CardScript>().ResetCard();
            hand[i].GetComponent<Renderer>().enabled = false;
        }
        aux = 0;
        handvalue = 0;
        aceList = new List<CardScript>();
    }

}
