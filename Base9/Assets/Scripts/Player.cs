using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public abstract class Player : MonoBehaviour, IPunObservable
{
    private string playerName;
    public string PlayerName
    {
        get { return playerName; }
    }

    protected GameManager gameManager;
    protected PhotonView photonView;

    public bool IsLocal
    {
        get {
            if (photonView != null)
                return photonView.IsMine;
            else
                return true;
        }
    }

    public Player()
    {
        playerName = "Opponent";
    }

    private void Awake()
    {
        photonView = PhotonView.Get(this);
    }

    private void Start()
    {
        RegisterPlayer();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            //stream.SendNext(this.dices[0]);
        }
        else
        {
            // Network player, receive data
            //this.dices[0] = (int)stream.ReceiveNext();
        }
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

    public void Init(string _playerName)
    {
        playerName = _playerName;
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
