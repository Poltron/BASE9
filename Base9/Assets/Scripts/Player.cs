using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    private string playerName;
    public string PlayerName
    {
        get { return playerName; }
    }

    protected GameManager gameManager;
    public bool isLocal;
    public int Purse;

    public Player(GameManager _gameManager, string _playerName, bool _isLocal)
    {
        playerName = _playerName;
        gameManager = _gameManager;
        isLocal = _isLocal;
        Purse = 15;
    }

    public abstract void BeginTurn();
    public abstract void PlayDice();
    public abstract void PlayBonusDice();
    public abstract void EndTurn();
}
