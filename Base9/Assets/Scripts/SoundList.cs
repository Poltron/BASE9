using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundName
{
    Dice_Idle,
    Dice_Launch,
    Dice_Hit_Ground,
    Dice_Hit_Wood,
    Dice_Hit_Dice,
    Panel_Show,
    Panel_Close,
    Coin_Swap,
    Coin_Gain,
    Coin_Hit_Ground,
    Coin_Hit_Coin,
    Coin_Destroy,
    Phase2_Start,
    Tile_Lock,
    Jingle_Base9,
    Jingle_Loose,
    Jingle_Win,
    Ambiance_Loop,
    Musique_Loop_Phase1,
    Musique_Loop_Phase2,
    Musique_Loop_Menu
}


public struct SoundCue
{
    public SoundName name;
    public AudioClip clip;
}

[CreateAssetMenu(fileName = "SoundList", menuName = "ScriptableObjects/SoundList", order = 1)]
public class SoundList : ScriptableObject
{

    public SoundCue[] cues;
}
