using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataProvider : MonoBehaviour {

    public GameData gameData;

	void Awake() {
        gameData = new GameData();
	}
}
