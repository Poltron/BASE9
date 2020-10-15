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

    private int id;
    public int Id
    {
        get { return id; }
    }

    protected GameManager gameManager;

    public Player()
    {
        playerName = "Opponent";
    }

    private void Start()
    {
        RegisterPlayer();
    }

    private void RegisterPlayer()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.RegisterNewPlayer(this);
        }
        else
        {
            StartCoroutine(WaitFor(1.0f, RegisterPlayer));
        }
    }

    public void Init(string _playerName, int _id)
    {
        playerName = _playerName;
        id = _id;
    }

    public IEnumerator WaitFor(float time, System.Action action)
    {
        yield return new WaitForSeconds(time);

        action();
    }

    public abstract void BeginTurn();
    public abstract void TwoDicePlayed();
    public abstract void ThirdDicePlayed();
    public abstract void EndTurn();
}
