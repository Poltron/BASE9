using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum GameMode
{
    AI,
    Local,
    Online
}

[CreateAssetMenu(fileName = "GameState", menuName = "ScriptableObjects/GameState", order = 2)]
public class GameState : ScriptableObject
{
    [SerializeField]
    public GameMode gameMode;
}
