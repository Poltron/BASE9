using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Player
{
    public Human(GameManager manager, string name, bool isLocal)
        : base(manager, name, isLocal)
    {

    }

    public override void BeginTurn()
    {
        Debug.Log("Human turn begin...");
        gameManager.UIManager.EnableInputUI(this);
    }

    public override void PlayDice()
    {
        Debug.Log("play dice");
        gameManager.ThrowDice(1);
        gameManager.ThrowDice(2);
    }

    public override void PlayBonusDice()
    {
        gameManager.ThrowDice(3);
    }

    public override void EndTurn()
    {
        Debug.Log("Human turn ended.");

        gameManager.UIManager.DisableInputUI(this);
        gameManager.ActivePlayerTurnEnded();

    }
}
