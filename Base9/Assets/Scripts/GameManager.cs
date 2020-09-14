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
    [Header("Players")]
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject aiPrefab;

    [Header("Dice")]
    [SerializeField]
    private GameObject dicePrefab;
    [SerializeField]
    private Dice[] dice;
    [SerializeField]
    private Transform[] diceSpawns;

    private int dicePlayed;

    [Header("Coins")]
    [SerializeField]
    private int nbOfCoin;

    [Space]
    [SerializeField]
    private GameObject coinPrefab;
    [SerializeField]
    private Transform purse0CoinSpawn;
    [SerializeField]
    private Transform purse1CoinSpawn;
    private Transform GetPlayerCoinSpawn(int number)
    {
        switch (number)
        {
            case 0:
                return purse0CoinSpawn;
            case 1:
                return purse1CoinSpawn;
        }

        return null;
    }

    [Space]
    [SerializeField]
    private List<Coin> purse0Coins;
    [SerializeField]
    private List<Coin> purse1Coins;
    private List<Coin> GetPlayerCoins(int number)
    {
        switch (number)
        {
            case 0:
                return purse0Coins;
            case 1:
                return purse1Coins;
        }

        return null;
    }

    [SerializeField]
    private List<Coin> bank0Coins;
    [SerializeField]
    private List<Coin> bank1Coins;
    [SerializeField]
    private List<Coin> bank2Coins;
    [SerializeField]
    private List<Coin> bank3Coins;
    [SerializeField]
    private List<Coin> bank4Coins;
    private List<Coin> GetBankCoins(int number)
    {
        switch(number)
        {
            case 0:
                return bank0Coins;
            case 1:
                return bank1Coins;
            case 2:
                return bank2Coins;
            case 3:
                return bank3Coins;
            case 4:
                return bank4Coins;
        }

        return null;
    }

    [SerializeField]
    private Transform bank0CoinSpawn;
    [SerializeField]
    private Transform bank1CoinSpawn;
    [SerializeField]
    private Transform bank2CoinSpawn;
    [SerializeField]
    private Transform bank3CoinSpawn;
    [SerializeField]
    private Transform bank4CoinSpawn;
    private Transform GetBankCoinSpawn(int number)
    {
        switch (number)
        {
            case 0:
                return bank0CoinSpawn;
            case 1:
                return bank1CoinSpawn;
            case 2:
                return bank2CoinSpawn;
            case 3:
                return bank3CoinSpawn;
            case 4:
                return bank4CoinSpawn;
        }

        return null;
    }

    private List<Player> players = new List<Player>();
    public Player Player1
    {
        get { return players[0]; }
    }
    public Player Player2
    {
        get { if (players.Count > 1) return players[1]; else return null; }
    }

    private int activePlayer;
    public int ActivePlayerNumber
    {
        get { return activePlayer; }
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

    [SerializeField]
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

    [SerializeField]
    private bool bPhase2;
    public bool IsPhase2()
    {
        return bPhase2;
    }

    private PUNManager PUNManager;
    private PhotonView photonView;
    private UIManager uiManager;
    public UIManager UIManager { get { return uiManager; } }
    private SoundManager soundManager;
    public SoundManager SoundManager { get { return soundManager; } }

    void Awake()
    {
        uiManager = GetComponent<UIManager>();
    }

    void Start()
    {
        photonView = PhotonView.Get(this);
        PUNManager = FindObjectOfType<PUNManager>();
        // IF WE'RE PLAYING ONLINE
        if (PUNManager != null && PUNManager.bPlayingOnline)
        {
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0);
            player.GetComponent<Player>().Init(PUNManager.userName, 0);
        }
        else if (PUNManager != null && PUNManager.bPlayingVSLocal) // IF WE'RE PLAYING VS LOCAL PLAYER
        {
            GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            player.GetComponent<Player>().Init("Player 1", 0);

            player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            player.GetComponent<Player>().Init("Player 2", 1);
        }
        else //if ( (PUNManager != null && PUNManager.bPlayingVSAI) || (!PUNManager.bPlayingVSLocal && !PUNManager.bPlayingOnline) ) // IF WE'RE PLAYING VS AI
        {
            GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            string name = "Player";
            if (PUNManager != null)
                name = PUNManager.userName;

            player.GetComponent<Player>().Init(name, 0);
            player = Instantiate(aiPrefab, Vector3.zero, Quaternion.identity);
            player.GetComponent<Player>().Init("Computer", 1);
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
        Debug.Log("Init Game");

        banks = new int[5];
        dices = new int[3];
        dicePlayed = 0;

        StartCoroutine(SpawnCoins());
    }

    public bool AreTwoFirstDices9()
    {
        return ( (dices[0] + dices[1]) == 9 );
    }

    public bool AreTwoFirstDicesSuperiorTo9()
    {
        return ((dices[0] + dices[1]) > 9);
    }

    private Vector3 MulVecs(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    public Transform GetRandomDiceSpawn()
    {
        return diceSpawns[UnityEngine.Random.Range(0, diceSpawns.Length - 1)];
    }

    IEnumerator SpawnCoins()
    {
        yield return new WaitForSeconds(1.0f);

        int i = 0;
        while ( i < nbOfCoin)
        {
            for (int j = 0; j < 5; ++j)
            {
                GameObject coin, coin2;
                if (PUNManager != null && PUNManager.bPlayingOnline)
                {
                    coin = PhotonNetwork.Instantiate(coinPrefab.name, purse0CoinSpawn.transform.position + MulVecs(UnityEngine.Random.insideUnitSphere, new Vector3(2.75f, 0.5f, 2.75f)), Quaternion.identity);
                    coin.transform.parent = purse0CoinSpawn;
                    coin2 = PhotonNetwork.Instantiate(coinPrefab.name, purse1CoinSpawn.transform.position + MulVecs(UnityEngine.Random.insideUnitSphere, new Vector3(2.75f, 0.5f, 2.75f)), Quaternion.identity);
                    coin2.transform.parent = purse1CoinSpawn;
                }
                else
                {
                    coin = Instantiate(coinPrefab, purse0CoinSpawn.transform.position + MulVecs(UnityEngine.Random.insideUnitSphere, new Vector3(2.75f, 0.5f, 2.75f)), Quaternion.identity, purse0CoinSpawn);
                    coin2 = Instantiate(coinPrefab, purse1CoinSpawn.transform.position + MulVecs(UnityEngine.Random.insideUnitSphere, new Vector3(2.75f, 0.5f, 2.75f)), Quaternion.identity, purse1CoinSpawn);
                }

                purse0Coins.Add(coin.GetComponent<Coin>());
                purse1Coins.Add(coin2.GetComponent<Coin>());

                i++;
            }
            purses[0] = i;
            purses[1] = i;
            Debug.Log(i);

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(2.0f);

        UIManager.ShowStartTurn();
        ActivePlayer.BeginTurn();
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
        for (int i = 0;i < dice.Length;i++)
        {
            dice[i].gameObject.SetActive(false);
            dice[i].transform.position = new Vector3(0, -10, 0);
        }

        UIManager.ShowEndTurn();

        if (activePlayer == 0)
        {
            activePlayer = 1;
        }
        else
        {
            activePlayer = 0;
        }
        dicePlayed = 0;

        ActivePlayer.BeginTurn();
        UIManager.ShowStartTurn();
    }
    
    IEnumerator ComputeDices()
    {
        Debug.Log("compute dices");
        int diceSum = dices[0] + dices[1] + dices[2];

        UIManager.ShowSidePanel();
        UIManager.EnableOperation(diceSum);

        yield return new WaitForSeconds(1.0f);

        bool longWaitNextTurn = false;

        if (diceSum != 9) // pay coins
        {
            longWaitNextTurn = true;
            int toPay = Mathf.Abs(diceSum - 9);

            purses[activePlayer] -= toPay;
            
            if (toPay >= 1 && toPay <= 5)
            {
                if (IsBankOpen(toPay - 1))
                {
                    banks[toPay - 1] += toPay;
                    StartCoroutine(WaitFor(0.8f, UIManager.HighlightBank, toPay - 1, true));
                    StartCoroutine(MoveCoins(toPay, GetPlayerCoins(ActivePlayerNumber), GetBankCoins(toPay -1), GetBankCoinSpawn(toPay - 1)));
                }
                else
                {
                    StartCoroutine(MoveCoins(toPay, GetPlayerCoins(ActivePlayerNumber), null, GetBankCoinSpawn(toPay - 1)));
                }
            }
            else
            {
                if (IsBankOpen(4))
                {
                    banks[4] += 5;
                    StartCoroutine(WaitFor(0.8f, UIManager.HighlightBank, 4, true));
                    StartCoroutine(MoveCoins(5, GetPlayerCoins(ActivePlayerNumber), GetBankCoins(4), GetBankCoinSpawn(4)));
                }
                else
                {
                    StartCoroutine(MoveCoins(5, GetPlayerCoins(ActivePlayerNumber), null, GetBankCoinSpawn(4)));
                }

                toPay -= 5;

                if (IsBankOpen(toPay - 1))
                {
                    banks[toPay - 1] += toPay;
                    StartCoroutine(WaitFor(0.8f, UIManager.HighlightBank, toPay - 1, true));
                    StartCoroutine(MoveCoins(toPay, GetPlayerCoins(ActivePlayerNumber), GetBankCoins(toPay - 1), GetBankCoinSpawn(toPay - 1)));
                }
                else
                {
                    StartCoroutine(MoveCoins(toPay, GetPlayerCoins(ActivePlayerNumber), null, GetBankCoinSpawn(toPay - 1)));
                }
            }
        }
        else // get coins
        {
            UIManager.Base9Anim();

            yield return new WaitForSeconds(2.5f);

            foreach (var dice in dices)
            {
                if (dice > 0 && dice < 6)
                {
                    Debug.Log(dice);
                    int bankId = dice - 1;
                    if (IsBankOpen(bankId))
                    {
                        if (banks[bankId] > 0)
                        {
                            longWaitNextTurn = true;
                            StartCoroutine(MoveCoins(banks[bankId], GetBankCoins(bankId), GetPlayerCoins(ActivePlayerNumber), GetPlayerCoinSpawn(ActivePlayerNumber)));
                            purses[activePlayer] += banks[bankId];
                        }

                        if (IsPhase2())
                        {
                            UIManager.EnableBankLock(bankId);
                        }

                        if (banks[bankId] > 0 && !bPhase2)
                        {
                            UIManager.HighlightBank(bankId, false);
                        }

                        banks[bankId] = 0;
                    }
                }
            }
        }

        dices = new int[3];

        if (longWaitNextTurn)
        {
            yield return new WaitForSeconds(1.0f);
        }

        TurnEnded(longWaitNextTurn);
    }

    IEnumerator MoveCoins(int number, List<Coin> start, List<Coin> end, Transform endSpawn)
    {
        for (int i = 0; i < number; i++)
        {
            Coin coin = start[start.Count - 1];
            start.RemoveAt(start.Count - 1);

            if (end != null)
                end.Add(coin);

            coin.Teleport( endSpawn.transform.position + MulVecs(UnityEngine.Random.insideUnitSphere, new Vector3(2.75f, 0f, 2.75f)), (end == null));

            yield return new WaitForSeconds(0.1f);
        }
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
        StartCoroutine(ComputeDices());
    }

    public void TurnEnded(bool longWait = true)
    {
        Debug.Log("turn ended");
        if (purses[activePlayer] <= 0)
        {
            EndGame(InactivePlayer, ActivePlayer);
            return;
        }
        bool wasPhase2 = bPhase2;
        bool ended = ComputeBanks();
        if (ended)
        {
            if (purses[0] > purses[1])
                EndGame(Player1, Player2);
            else
                EndGame(Player2, Player1);

            return;
        }

        if (!wasPhase2 && bPhase2) // play anim for Phase2
        {
            StartCoroutine(WaitFor(1.0f, UIManager.ShowPhase2));
            StartCoroutine(WaitFor(3.0f, NextTurn));
        }
        else
        {
            if (longWait)
                StartCoroutine(WaitFor(2.0f, NextTurn));
            else
                StartCoroutine(WaitFor(0.5f, NextTurn));
        }
    }

    [PunRPC]
    public void RPC_ThrowDice(int number)
    {
        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            dice[number - 1].Throw(GetRandomDiceSpawn());
        }
        else
        {
            photonView.RPC("RPC_ThrowDice", RpcTarget.MasterClient, number);
        }
    }

    public void DiceResult(int diceNumber, int result)
    {
        dices[diceNumber - 1] = result;
        dicePlayed++;

        Debug.Log(diceNumber + " : " + dices[diceNumber - 1]);
        if (dicePlayed == 2)
        {
            dices[0] = dice[0].GetTopFace();
            dices[1] = dice[1].GetTopFace();
            dices[2] = 0;

            ActivePlayer.TwoDicePlayed();
        }
        else if (dicePlayed == 3)
        {
            dices[0] = dice[0].GetTopFace();
            dices[1] = dice[1].GetTopFace();
            dices[2] = dice[2].GetTopFace();

            StartCoroutine(WaitFor(1.0f, ActivePlayer.EndTurn));
        }
    }

    public void EndGame(Player winner, Player looser)
    {
        RPC_GameEnded(winner, looser);

        if (PhotonNetwork.IsConnected && photonView.IsMine)
        {
            photonView.RPC("RPC_GameEnded", RpcTarget.Others, winner, looser);
        }
    }

    [PunRPC]
    public void RPC_GameEnded(Player winner, Player looser)
    {
        UIManager.HideLight();
        UIManager.SetWinnerLooser(winner, looser);

        if (GetPlayerCoins(0).Count <= 0)
        {
            UIManager.SetEndReason(Player1.PlayerName + " had 0 coins left.");
        }
        else if (GetPlayerCoins(1).Count <= 0)
        {
            UIManager.SetEndReason(Player2.PlayerName + " had 0 coins left.");
        }
        else
        {
            UIManager.SetEndReason(winner.PlayerName + " had more coins than " + looser.PlayerName);
        }

        GameEventMessage.SendEvent("GameEnded");
    }

    public void GameEndBeforeMenu()
    {
#if UNITY_ANDROID
        UIManager.ShowRatingPrompt(true);
#endif
    }

    public void BackToMenu()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();

        LevelLoader.Instance.LoadNextLevel(0, false);
    }

    public IEnumerator WaitFor(float time, Action action)
    {
        yield return new WaitForSeconds(time);

        action();
    }

    public IEnumerator WaitFor(float time, Action<int, bool> action, int param_int, bool param_bool)
    {
        yield return new WaitForSeconds(time);

        action(param_int, param_bool);        
    }
}
