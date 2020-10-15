using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Player
{
    protected bool bListeningForClicks;

    private void Update()
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

                    gameManager.ThrowDice(dice);
                }
            }
        }
    }

    public override void BeginTurn()
    {
        Debug.Log("Human " + PlayerName + " : Begin turn");
        bListeningForClicks = true;
        gameManager.UIManager.EnableTwoDice(this);
        gameManager.UIManager.DisableOperation();
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

    public override void EndTurn()
    {
        Debug.Log("Human : End turn");
        bListeningForClicks = false;

        gameManager.UIManager.DisableThirdDiceButton(this);
        gameManager.UIManager.DisableEndTurnButton(this);

        gameManager.ActivePlayerTurnEnded();
    }
}
