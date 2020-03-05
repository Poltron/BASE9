using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Human : Player
{
    protected bool bListeningForClicks;
    protected int nbOfDicePlayed;

    private void Update()
    {
        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            if (bListeningForClicks && (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.transform.gameObject.name + " " + hit.transform.tag);

                    int dice = 0;
                    switch (hit.transform.tag)
                    {
                        case "Dice1":
                            dice = 1;
                            break;
                        case "Dice2":
                            dice = 2;
                            break;
                        case "Dice3":
                            dice = 3;
                            break;
                        case "EndTurn":
                            EndTurn();
                            break;
                    }

                    if (dice != 0)
                    {
                        hit.transform.parent.parent.gameObject.SetActive(false);
                        gameManager.RPC_ThrowDice(dice);

                        nbOfDicePlayed++;
                        if (nbOfDicePlayed == 2)
                        {
                            gameManager.UIManager.EnablePlayBonusDiceButton(this);
                            gameManager.UIManager.EnableEndTurnButton(this);
                        }
                        else if (nbOfDicePlayed == 3)
                        {
                            gameManager.UIManager.DisableEndTurnButton(this);
                            StartCoroutine(WaitFor(1.0f, EndTurn));
                        }
                    }
                }
            }
        }
    }

    [PunRPC]
    public override void BeginTurn()
    {
        nbOfDicePlayed = 0;

        Debug.Log("Human : Begin turn");
        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            bListeningForClicks = true;
            gameManager.UIManager.EnableDice(this);
            gameManager.UIManager.DisableOperation();
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
            gameManager.UIManager.EnablePlayBonusDiceButton(this);
            gameManager.UIManager.EnableEndTurnButton(this);
        }
    }

    public override void PlayBonusDice()
    {
        Debug.Log("Human : Play bonus dice");
        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            gameManager.UIManager.DisablePlayBonusDiceButton(this);
            gameManager.UIManager.DisableEndTurnButton(this);
        }
    }

    [PunRPC]
    public override void EndTurn()
    {
        Debug.Log("Human : End turn");

        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            gameManager.UIManager.DisablePlayBonusDiceButton(this);
            gameManager.UIManager.DisableEndTurnButton(this);
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
