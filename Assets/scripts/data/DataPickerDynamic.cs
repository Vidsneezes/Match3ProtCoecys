using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataPickerDynamic : MonoBehaviour {

    public Text textToPlaceIn;

    public string gameVariable;
    private GameDataProvider gameDataProvider;
    public void Start()
    {
        gameDataProvider = GameObject.FindGameObjectWithTag("GameDataProvider").GetComponent<GameDataProvider>();
        textToPlaceIn = GetComponent<Text>();
        int value;
        if (gameDataProvider.gameData.TryGetInt(gameVariable, out value))
        {
            textToPlaceIn.text = value + "";
        }
    }
    // Update is called once per frame
    void Update () {
        int value;
        if (gameDataProvider.gameData.TryGetInt(gameVariable, out value))
        {
            textToPlaceIn.text = value + "";
        }
    }
}
