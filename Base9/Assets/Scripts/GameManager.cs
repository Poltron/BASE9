using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IPunObservable
{
    private Player[] players = new Player[2];
    private bool activePlayer;

    [SerializeField]
    private Text Dice1Text;
    [SerializeField]
    private Text Dice2Text;

    [SerializeField]
    private Button PlayDiceButton;
    [SerializeField]
    private Button BonusDiceButton;

    void Start()
    {
        players[0] = new Human(this, "Human Player", true);
        players[1] = new IA(this, "Computer Player");

        players[0].BeginTurn();
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

    public void StartPlayerTurn(int playerIndex)
    {
        players[playerIndex].BeginTurn();
    }

    public void EndTurn()
    {

    }

    public void SetDices(int dice1, int dice2)
    {
        Dice1Text.text = dice1.ToString();
        Dice2Text.text = dice2.ToString();
    }
}
