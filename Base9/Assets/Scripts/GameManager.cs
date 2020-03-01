using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Doozy.Engine;

public class GameManager : MonoBehaviour, IPunObservable
{
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject aiPrefab;

    private List<Player> players = new List<Player>();
    public Player Player1
    {
        get { return players[0]; }
    }
    public Player Player2
    {
        get { return players[1]; }
    }

    private int activePlayer;
    public int ActivePlayerNumber
    {
        get { return activePlayer + 1; }
    }
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

    private int[] purses = new int[2];
    public int GetPurse(int number)
    {
        return purses[number - 1];
    }

    private bool bPhase2;
    public bool IsPhase2()
    {
        return bPhase2;
    }

    private PUNManager PUNManager;
    private PhotonView photonView;
    public UIManager UIManager;

    void Start()
    {
        photonView = PhotonView.Get(this);
        PUNManager = FindObjectOfType<PUNManager>();
        // IF WE'RE PLAYING ONLINE
        if (PUNManager != null && PUNManager.bPlayingOnline)
        {
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0);
            player.GetComponent<Player>().Init("You");
        }
        else // IF WE'RE PLAYING LOCAL VS AI
        {
            GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            player.GetComponent<Player>().Init("You");

            player = Instantiate(aiPrefab, Vector3.zero, Quaternion.identity);
            player.GetComponent<Player>().Init("Computer");
        }
    }

    public void RegisterNewPlayer(Player player)
    {
        Debug.Log("Register new player !");

        players.Add(player);

        if (players.Count == 2 && (photonView.IsMine || !PhotonNetwork.IsConnected))
        {
            InitGame();
        }
    }

    public void InitGame()
    {
        purses[0] = 70;
        purses[1] = 70;

        banks = new int[5];
        dices = new int[3];

        StartCoroutine(WaitFor(2.0f, ActivePlayer.BeginTurn));
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(this.dices[0]);
            stream.SendNext(this.dices[1]);
            stream.SendNext(this.dices[2]);

            stream.SendNext(this.banks[0]);
            stream.SendNext(this.banks[1]);
            stream.SendNext(this.banks[2]);
            stream.SendNext(this.banks[3]);
            stream.SendNext(this.banks[4]);

            stream.SendNext(this.purses[0]);
            stream.SendNext(this.purses[1]);

            stream.SendNext(this.activePlayer);
        }
        else
        {
            // Network player, receive data
            this.dices[0] = (int)stream.ReceiveNext();
            this.dices[1] = (int)stream.ReceiveNext();
            this.dices[2] = (int)stream.ReceiveNext();

            this.banks[0] = (int)stream.ReceiveNext();
            this.banks[1] = (int)stream.ReceiveNext();
            this.banks[2] = (int)stream.ReceiveNext();
            this.banks[3] = (int)stream.ReceiveNext();
            this.banks[4] = (int)stream.ReceiveNext();

            this.purses[0] = (int)stream.ReceiveNext();
            this.purses[1] = (int)stream.ReceiveNext();

            this.activePlayer = (int)stream.ReceiveNext();
        }
    }

    public void NextTurn()
    {
        if (activePlayer == 0)
        {
            activePlayer = 1;
        }
        else
        {
            activePlayer = 0;
        }

        UIManager.DisableOperation();

        ActivePlayer.BeginTurn();
    }
    
    public void ComputeDices()
    {
        int diceSum = dices[0] + dices[1] + dices[2];
        if (diceSum != 9) // pay coins
        {
            int toPay = Mathf.Abs(diceSum - 9);
            purses[activePlayer] -= toPay;
            
            if (toPay >= 1 && toPay <= 5)
            {
                if (IsBankOpen(toPay - 1))
                    banks[toPay - 1] += toPay;
            }
            else
            {
                if (IsBankOpen(4))
                    banks[4] += 5;

                toPay -= 5;

                if (IsBankOpen(toPay - 1))
                    banks[toPay - 1] += toPay;
            }
        }
        else // get coins
        {
            foreach (var dice in dices)
            {
                if (dice > 0 && dice < 6)
                {
                    Debug.Log(dice);
                    purses[activePlayer] += banks[dice - 1];
                    banks[dice - 1] = 0;
                }
                else if (dice == 6)
                {
                    purses[activePlayer] += banks[4] + banks[0];
                    banks[4] = 0;
                    banks[0] = 0;
                }
            }
        }

        dices = new int[3];

        UIManager.EnableOperation(diceSum);
    }

    private bool IsBankOpen(int BankIndex)
    {
        if (IsPhase2() && banks[BankIndex] == 0)
        {
            return false;
        }

        return true;
    }

    public bool ComputeBanks()
    {
        foreach (var bank in banks)
        {
            if (bPhase2)
            {
                if (bank != 0)
                {
                    return false;
                }
            }
            else
            {
                if (bank == 0)
                {
                    return false;
                }
            }
        }
        
        if (bPhase2)
        {
            return true;
        }
        else
        {
            bPhase2 = true;
            return false;
        }
    }

    public void ActivePlayerTurnEnded()
    {
        ComputeDices();
        if (purses[activePlayer] <= 0)
        {
            EndGame(InactivePlayer, ActivePlayer);
            return;
        }

        bool ended = ComputeBanks();
        if (ended)
        {
            if (purses[0] > purses[1])
                EndGame(Player1, Player2);
            else
                EndGame(Player2, Player1);

            return;
        }

        StartCoroutine(WaitFor(2.0f, NextTurn));
    }

    [PunRPC]
    public void RPC_ThrowDice(int number)
    {
        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            dices[number - 1] = UnityEngine.Random.Range(1, 7);
        }
        else
        {
            photonView.RPC("ThrowDice", RpcTarget.MasterClient, number);
        }
    }

    public void EndGame(Player winner, Player looser)
    {
        RPC_GameEnded(winner, looser);

        if (PhotonNetwork.IsConnected && photonView.IsMine)
        {
            photonView.RPC("GameEnded", RpcTarget.Others, winner, looser);
        }
    }

    [PunRPC]
    public void RPC_GameEnded(Player winner, Player looser)
    {
        UIManager.SetWinnerLooser(winner, looser);
        GameEventMessage.SendEvent("GameEnded");
    }

    public void BackToMenu()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();

        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public IEnumerator WaitFor(float time, Action action)
    {
        yield return new WaitForSeconds(time);

        action();
    }
}
