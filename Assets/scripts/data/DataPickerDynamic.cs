using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataPickerDynamic : MonoBehaviour {

    public Text textToPlace;

    public string gameVariable;
    private GameDataProvider gameDataProvider;

	// Use this for initialization
	void Start () {
        gameDataProvider = GameObject.FindObjectOfType<GameDataProvider>();
        textToPlace = GetComponent<Text>();
        int value;
        if(gameDataProvider.gameData.TryGetInt(gameVariable, out value))
        {
            textToPlace.text = value + "";
        }
	}
	
	// Update is called once per frame
	void Update () {
        int value;
        if (gameDataProvider.gameData.TryGetInt(gameVariable, out value))
        {
            textToPlace.text = value + "";
        }
    }
}
