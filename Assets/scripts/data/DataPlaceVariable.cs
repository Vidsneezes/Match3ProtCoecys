using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataPlaceVariable : MonoBehaviour {

    public Text textToPlaceIn;

    public string gameVariable;

    public void Start()
    {
        GameDataProvider gameDataProvider = GameObject.FindGameObjectWithTag("GameDataProvider").GetComponent<GameDataProvider>();
        textToPlaceIn = GetComponent<Text>();
        int value;
        if(gameDataProvider.gameData.TryGetInt(gameVariable, out value))
        {
            textToPlaceIn.text = value + "";
        }
    }

}
