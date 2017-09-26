using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataVariablePicker : MonoBehaviour {

    public string gameVariable;
    public List<string> pickValues;
    public Text textToPlaceIn;

    public void Start()
    {
        GameDataProvider gameDataProvider = GameObject.FindGameObjectWithTag("GameDataProvider").GetComponent<GameDataProvider>();
        textToPlaceIn = GetComponent<Text>();
        int value;
        if (gameDataProvider.gameData.TryGetInt(gameVariable, out value))
        {
            if (value < pickValues.Count && value >= 0)
            {
                textToPlaceIn.text = pickValues[value] + "";
            }
        }
    }

}
