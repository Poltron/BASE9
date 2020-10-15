using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 3)]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    public string AIName;
    [SerializeField]
    public string PlayerName;
    [SerializeField]
    public string LocalPlayer1Name;
    [SerializeField]
    public string LocalPlayer2Name;
}
