using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {

    public const string SCORE = "score";
    public const string TIMER = "time";

    private Dictionary<string, int> gameVariables;

    public GameData()
    {
        gameVariables = new Dictionary<string, int>();
    }

    public void SetInt(string key, int value)
    {
        if (gameVariables.ContainsKey(key))
        {
            gameVariables[key] = value;
        }
        else
        {
            gameVariables.Add(key, value);
        }
    }

    public bool TryGetInt(string key, out int value)
    {
        return gameVariables.TryGetValue(key, out value);
    }

    public bool HasKey(string key)
    {
        return gameVariables.ContainsKey(key);
    }

}
