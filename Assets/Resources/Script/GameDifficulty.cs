using UnityEngine;

public static class GameDifficulty
{
    public enum Difficulty { Easy, Hard }

    public static Difficulty CurrentDifficulty = Difficulty.Hard;
}