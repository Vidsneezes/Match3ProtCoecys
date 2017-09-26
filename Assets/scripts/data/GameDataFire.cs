using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataFire : MonoBehaviour {

    public void Awake()
    {
        GameObject existing = GameObject.FindGameObjectWithTag("GameDataProvider");
        if(existing == null)
        {
            Debug.Log("to sche");
            GameDataProvider gameDataProvider = Resources.Load<GameDataProvider>("gamedataprovider/GameDataProvider");
            GameObject.Instantiate(gameDataProvider);
        }
    }

}
