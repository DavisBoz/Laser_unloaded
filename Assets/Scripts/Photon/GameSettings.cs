using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    ARENA = 0,
    DEATHMATCH = 1,
    RACE = 2
}

public class GameSettings : MonoBehaviour
{
    public static GameMode GameMode = GameMode.ARENA;
    public static bool IsAwayTeam = false;
}