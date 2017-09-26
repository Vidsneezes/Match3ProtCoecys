using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSetValue : MonoBehaviour {

    public string gameVariable;
    public int value;

    public void OnSet()
    {
        GameDataProvider gameDataProvider = GameObject.FindGameObjectWithTag("GameDataProvider").GetComponent<GameDataProvider>();
        gameDataProvider.gameData.SetInt(gameVariable, value);
    }
}
