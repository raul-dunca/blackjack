using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //cele 4 butoane din joc
    public Button dealbtn;
    public Button hitbtn;
    public Button standbtn;
    public Button betbtn1;
   // public Button betbtn2;
    private int standClicks = 0;
    //accesam mana jucatorului si computerului
    public UserScript player;
    public UserScript dealer;
    //accesam textul din scoreboard/hud
    public Text scoreText;
    public Text dealerscoreText;
    public Text betText;
    public Text cashText;
    public Text standbtnText;
    public Text maintext;

    public GameObject hidecard;//cartea intoarsa a dealeurlui

    int pot = 0;//banii pariati
    int bani = 5000;
    void Start()
    {
        dealbtn.onClick.AddListener(() => DealClicked());
        hitbtn.onClick.AddListener(() => HitClicked());
        standbtn.onClick.AddListener(() => StandClicked());
        betbtn1.onClick.AddListener(() => BetClicked());
        //betbtn2.onClick.AddListener(() => BetClicked());
    }

    private void StandClicked()
    {
        hidecard.GetComponent<Renderer>().enabled = false;
        betbtn1.interactable = false;
        standClicks++;
        if (standClicks > 1) RoundOver();
        HitDealer();
        standbtnText.text = "Call";
    }

    private void HitDealer()
    {
        
        while (dealer.handvalue<=16 && dealer.aux<=5)//cat timp dealeru are sub 17 pct si mai putin de 5 carti mai primeste o carte
        {
            dealer.GetCard();
            dealerscoreText.text ="Hand: " + dealer.handvalue.ToString();
            if (dealer.handvalue > 20) RoundOver();
        }
    }

    private void HitClicked()
    {
        betbtn1.interactable = false;
        //putem da hit de 3 ori adica sa avem maxim 5 carti
        if (player.aux <= 5)
        {
            player.GetCard();
            scoreText.text = "Hand: " + player.handvalue.ToString();
            if (player.handvalue > 20) RoundOver();
        }
        
    }

    private void DealClicked()
    {
        betbtn1.interactable = true;
        //se pregateste o noura runda 
        player.ResetHand();
        dealer.ResetHand();
        //ascundem scorul dealaerului cand incepe jocul
        maintext.gameObject.SetActive(false);
        dealerscoreText.gameObject.SetActive(false);

        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();// se amesteca pachetul dupa fiecare runda terminata
        player.Starthand();
        dealer.Starthand();
        //actualizare scor
        scoreText.text = "Hand: " + player.handvalue.ToString();
        dealerscoreText.text = "Hand: " + dealer.handvalue.ToString();
        hidecard.GetComponent<Renderer>().enabled = true;//activam cartea care acopera o carte a dealerului 
        //vizibilitatea butoanelor
        dealbtn.gameObject.SetActive(false);
        hitbtn.gameObject.SetActive(true);
        standbtn.gameObject.SetActive(true);
        standbtnText.text = "Stand";

        pot = 200;
        if (bani < 100) {
          
            betText.text = "Bets: 0$";
            cashText.text = "0$";
            maintext.text = "GAME OVER";
            maintext.gameObject.SetActive(true);
            hitbtn.gameObject.SetActive(false);
            Time.timeScale = 0; 
        }
        betText.text = "Bets: "+ pot.ToString()+"$";
        player.AdjustMoney(-100);
        bani = bani - 100;
        cashText.text = player.GetMoney().ToString()+"$";
    }


    void RoundOver()
    {
        bool playerbust = false;
        bool dealerbust = false;
        bool player21 = false;
        bool dealer21 = false;
        if (player.handvalue > 21)
            playerbust = true;
        if (dealer.handvalue > 21)
            dealerbust = true;
        if (player.handvalue == 21)
            player21 = true;
        if (dealer.handvalue == 21)
            dealer21 = true;
        if (standClicks < 2 && !playerbust && !player21 && !dealer21 && !dealerbust) return;
        bool roundOver = true;
        if (playerbust || (!dealerbust && dealer.handvalue > player.handvalue))
        {
            if(playerbust)
            { hidecard.GetComponent<Renderer>().enabled = false; }
            maintext.text = "DEALER WINS!";
        }
        else if (dealerbust || player.handvalue > dealer.handvalue)
        {
            maintext.text = "YOU WIN!";
           
            bani += pot;
            player.AdjustMoney(pot);
           
        }
        else if (player.handvalue == dealer.handvalue)
        {
            maintext.text = "PUSH";
            bani += pot / 2;
            player.AdjustMoney(pot / 2);
        }
        else
        {     
            roundOver = false;
        }
        //setam ecranul pentru finalul rundei
        if(roundOver)
        {
            hitbtn.gameObject.SetActive(false);
            standbtn.gameObject.SetActive(false);
            dealbtn.gameObject.SetActive(true);
            maintext.gameObject.SetActive(true);
            dealerscoreText.gameObject.SetActive(true);
            hidecard.GetComponent<Renderer>().enabled = false;
            cashText.text =player.GetMoney().ToString()+"$";
            standClicks = 0;
        }
    }
    void BetClicked()
    {
        
        if (bani >= 100)
        {
            bani = bani - 100;
            Text newBet = betbtn1.GetComponentInChildren(typeof(Text)) as Text;//(schema) luam ca beet suma de pe textul butonului
            int inBet = int.Parse(newBet.text.ToString().Remove(0, 1)); // scoatem semnul $
            player.AdjustMoney(-inBet);
            cashText.text = player.GetMoney().ToString() + "$";
            pot = pot + (inBet * 2);

            betText.text = "Bets: " + pot.ToString() + "$";
        }
        
    }
}
