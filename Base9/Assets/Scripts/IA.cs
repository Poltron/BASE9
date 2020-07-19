using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class IA : Player
{
    public override void BeginTurn()
    {
        Debug.Log("AI : Begin turn");
        StartCoroutine(WaitFor(1.0f, FirstPlay));
    }

    private void FirstPlay()
    {
        Debug.Log("AI : Play dice");
        gameManager.RPC_ThrowDice(1);

        DG.Tweening.DOVirtual.DelayedCall(1.0f, PlayDice2);
    }

    private void PlayDice2()
    {
        gameManager.RPC_ThrowDice(2);
    }

    public override void TwoDicePlayed()
    {
        int d1 = gameManager.GetDice(1);
        int d2 = gameManager.GetDice(2);
        int sum = d1 + d2;

        if (sum < 8)
        {
            StartCoroutine(WaitFor(1.0f, SecondPlay));
        }
        else
        {
            StartCoroutine(WaitFor(1.0f, EndTurn));
        }
    }

    private void SecondPlay()
    {
        gameManager.RPC_ThrowDice(3);
    }

    public override void ThirdDicePlayed()
    {
        StartCoroutine(WaitFor(1.0f, EndTurn));
    }

    public override void EndTurn()
    {
        Debug.Log("AI : End turn");
        gameManager.ActivePlayerTurnEnded();
    }
}
