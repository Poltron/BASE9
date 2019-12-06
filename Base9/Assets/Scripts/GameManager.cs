using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IPunObservable
{
    private Player[] players = new Player[2];
    public Player Player1
    {
        get { return players[0]; }
    }
    public Player Player2
    {
        get { return players[1]; }
    }

    private int activePlayer;
    public Player ActivePlayer
    {
        get { return players[activePlayer]; }
    }
    public Player InactivePlayer
    {
        get
        {
            if (activePlayer == 1)
                return players[0];
            else
                return players[1];
        }
    }

    private int[] dices = new int[3];
    public int GetDice(int number)
    {
        return dices[number - 1];
    }

    private int[] banks = new int[5];
    public int GetBank(int number)
    {
        return banks[number - 1];
    }

    private PUNManager PUNManager;
    public UIManager UIManager;

    void Start()
    {
        PUNManager = FindObjectOfType<PUNManager>();
        if (PUNManager != null && PUNManager.bPlayingAgainstHuman)
        {
            players[0] = new Human(this, "Human Player", true);
            players[1] = new Human(this, "Human Player", false);
        }
        else
        {
            players[0] = new Human(this, "Human Player", true);
            players[1] = new IA(this, "Computer Player");
        }

        StartActivePlayerTurn();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            //stream.SendNext(this.IsFiring);
            //stream.SendNext(this.Health);
        }
        else
        {
            // Network player, receive data
            //this.IsFiring = (bool)stream.ReceiveNext();
            //this.Health = (float)stream.ReceiveNext();
        }
    }

    public void StartActivePlayerTurn()
    {
        dices = new int[3];
        UIManager.HideDices();

        ActivePlayer.BeginTurn();
    }

    public void ActivePlayerTurnEnded()
    {
        int diceSum = dices[0] + dices[1] + dices[2];
        
        if (diceSum != 9) // pay coins
        {
            int toPay = Mathf.Abs(diceSum - 9);
            ActivePlayer.Purse -= toPay;
            if (toPay >= 1 && toPay <= 5)
            {
                banks[toPay - 1] += toPay;
            }
            else
            {
                banks[4] += 5;
                toPay -= 5;
                banks[toPay - 1] = toPay;
            }
        }
        else // get coins
        {
            Debug.Log("Jackpot : " + diceSum);

            foreach(var dice in dices)
            {
                Debug.Log("Dice : " + dice);
                if (dice > 0 && dice < 6)
                {
                    Debug.Log(dice);
                    ActivePlayer.Purse += banks[dice - 1];
                    banks[dice - 1] = 0;
                }
                else if (dice == 6)
                {
                    Debug.Log("1 + 5");
                    ActivePlayer.Purse += banks[4] + banks[0];
                    banks[4] = 0;
                    banks[0] = 0;
                }
            }
        }

        if (ActivePlayer.Purse <= 0)
        {
            GameEnded(InactivePlayer, ActivePlayer);
        }

        if (activePlayer == 0)
        {
            activePlayer = 1;
        }
        else
        {
            activePlayer = 0;
        }

        StartCoroutine(WaitFor(2.0f, StartActivePlayerTurn)); 
    }

    public void ThrowDice(int number)
    {
        dices[number - 1] = UnityEngine.Random.Range(1, 7);
        UIManager.ShowDice(number, dices[number - 1]);
    }

    public void GameEnded(Player winner, Player looser)
    {
        UIManager.SetWinnerLooser(winner, looser);
        UIManager.ShowEndPanel(true);
    }

    // HACK FOR ADDLISTENER BUG
    public void ActivePlayerThrowDice()
    {
        ActivePlayer.PlayDice();
    }

    public void ActivePlayerThrowBonusDice()
    {
        ActivePlayer.PlayBonusDice();
    }

    public void ActivePlayerEndTurn()
    {
        ActivePlayer.EndTurn();
    }

    public void TogglePause()
    {
        UIManager.TogglePausePanel();
    }

    public void BackToMenu()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public IEnumerator WaitFor(float time, Action action)
    {
        yield return new WaitForSeconds(time);

        action();
    }
}
