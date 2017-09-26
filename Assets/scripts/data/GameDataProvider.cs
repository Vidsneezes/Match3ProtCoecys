using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataProvider : MonoBehaviour {

    public GameData gameData;

    public void Awake()
    {
        gameData = new GameData();
        DontDestroyOnLoad(gameObject);
    }

}
