using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class IA : Player
{
    public IA()
    { }

    public override void BeginTurn()
    {
        Debug.Log("AI turn begins...");
        PlayDice();
        EndTurn();
    }

    public override void PlayDice()
    {
        Debug.Log("play dice1");
        gameManager.ThrowDice(1);
        Debug.Log("play dice2");
        gameManager.ThrowDice(2);
    }

    public override void PlayBonusDice()
    {
        gameManager.ThrowDice(3);
    }

    public override void EndTurn()
    {
        Debug.Log("AI turn ended.");
        gameManager.ActivePlayerTurnEnded();
    }
}
