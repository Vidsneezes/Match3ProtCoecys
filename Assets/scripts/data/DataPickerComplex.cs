using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataPickerComplex : MonoBehaviour {

    public string mainSource;
    public Text textToPlaceIn;

    public List<DataPickerWidget> possibleChoices;

    private string choiceSource;
    private GameDataProvider gameDataProvider;

    private void Start()
    {
        gameDataProvider = GameObject.FindGameObjectWithTag("GameDataProvider").GetComponent<GameDataProvider>();
        int mainSourceValue;
        if (gameDataProvider.gameData.TryGetInt(mainSource, out mainSourceValue)) {
            for (int i = 0; i < possibleChoices.Count; i++)
            {
                if (possibleChoices[i].checkValue == mainSourceValue)
                {
                    choiceSource = possibleChoices[i].gameVariableToGet;
                }
            }
        }
        else
        {
            choiceSource = possibleChoices[0].gameVariableToGet;
        }
    }

    // Update is called once per frame
    void Update () {
        int choiceSourceValue;
        if (gameDataProvider.gameData.TryGetInt(choiceSource, out choiceSourceValue))
        {
            textToPlaceIn.text = choiceSourceValue + "";
        }
    }
}

[System.Serializable]
public class DataPickerWidget
{
    public int checkValue;
    public string gameVariableToGet;
}
