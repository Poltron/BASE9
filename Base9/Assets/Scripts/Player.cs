using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    [Tooltip("Player Name")]
    [SerializeField]
    private string playerName;

    public abstract void BeginTurn();

    public abstract void EndTurn();
}
