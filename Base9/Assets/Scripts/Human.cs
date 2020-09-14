using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Human : Player
{
    protected bool bListeningForClicks;

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
                            gameManager.UIManager.DisableEndTurnButton(this);
                            break;
                        case "EndTurn":
                            EndTurn();
                            break;
                    }

                    if (dice != 0)
                    {
                        hit.transform.parent.parent.parent.GetComponent<Animator>().SetBool("bEnabled", false);

                        SoundManager.Instance.RemoveCue(SoundName.Dice_Idle);

                        gameManager.RPC_ThrowDice(dice);
                    }
                }
            }
        }
    }

    [PunRPC]
    public override void BeginTurn()
    {
        Debug.Log("Human " + PlayerName + " : Begin turn");
        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            bListeningForClicks = true;
            gameManager.UIManager.EnableTwoDice(this);
            gameManager.UIManager.DisableOperation();
        }
        else
        {
            photonView.RPC("BeginTurn", RpcTarget.Others);
        }
    }

    public override void TwoDicePlayed()
    {
        if (gameManager.AreTwoFirstDices9() || gameManager.AreTwoFirstDicesSuperiorTo9())
        {
            EndTurn();
        }
        else
        {
            gameManager.UIManager.EnableThirdDiceButton(this);
            gameManager.UIManager.EnableEndTurnButton(this);
        }
    }

    public override void ThirdDicePlayed()
    {
        gameManager.UIManager.DisableEndTurnButton(this);
        StartCoroutine(WaitFor(1.0f, EndTurn));
    }

    [PunRPC]
    public override void EndTurn()
    {
        Debug.Log("Human : End turn");
        bListeningForClicks = false;

        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            gameManager.UIManager.DisableThirdDiceButton(this);
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
