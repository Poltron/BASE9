using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    private string playerName;
    private GameManager gameManager;
    public bool isLocal;

    public Player(GameManager _gameManager, string _playerName, bool _isLocal)
    {
        playerName = _playerName;
        gameManager = _gameManager;
        isLocal = _isLocal;
    }

    public abstract void BeginTurn();
    public abstract void EndTurn();
}
