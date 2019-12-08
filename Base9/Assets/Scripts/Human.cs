using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Human : Player
{
    public Human()
    { }

    [PunRPC]
    public override void BeginTurn()
    {
        Debug.Log("Human turn begin...");
        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            gameManager.UIManager.EnableInputUI(this);
        }
        else
        {
            photonView.RPC("BeginTurn", RpcTarget.Others);
        }
    }

    public override void PlayDice()
    {
        Debug.Log("play dice");
        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            gameManager.ThrowDice(1);
            gameManager.ThrowDice(2);
        }
    }

    public override void PlayBonusDice()
    {
        Debug.Log("play bonus dice");
        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            gameManager.ThrowDice(3);
        }
    }

    [PunRPC]
    public override void EndTurn()
    {
        Debug.Log("Human turn ended.");

        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            gameManager.UIManager.DisableInputUI(this);
        }

        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            gameManager.ActivePlayerTurnEnded();
        }
        else
        {
            photonView.RPC("EndTurn", RpcTarget.Others);
        }
    }
}
