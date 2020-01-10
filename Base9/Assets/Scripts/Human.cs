using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Human : Player
{
    [PunRPC]
    public override void BeginTurn()
    {
        Debug.Log("Human : Begin turn");
        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            gameManager.UIManager.EnablePlayDiceButton(this, true);
        }
        else
        {
            photonView.RPC("BeginTurn", RpcTarget.Others);
        }
    }

    public override void PlayDice()
    {
        Debug.Log("Human : Play dice");
        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            gameManager.UIManager.EnablePlayDiceButton(this, false);
            gameManager.UIManager.EnablePlayBonusDiceButton(this, true);
            gameManager.UIManager.EnableEndTurnButton(this, true);

            gameManager.RPC_ThrowDice(1);
            gameManager.RPC_ThrowDice(2);
        }
    }

    public override void PlayBonusDice()
    {
        Debug.Log("Human : Play bonus dice");
        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            gameManager.UIManager.EnablePlayBonusDiceButton(this, false);

            gameManager.RPC_ThrowDice(3);
        }
    }

    [PunRPC]
    public override void EndTurn()
    {
        Debug.Log("Human : End turn");

        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            gameManager.UIManager.EnablePlayBonusDiceButton(this, false);
            gameManager.UIManager.EnableEndTurnButton(this, false);
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
