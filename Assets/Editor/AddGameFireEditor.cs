using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class AddGameFireEditor : Editor {

    [MenuItem("ScenePrep/Add GameFire")]
    static void AddGameFireToScene()
    {
        GameDataFire gameFire = GameObject.FindObjectOfType<GameDataFire>();
        if(gameFire == null)
        {
            GameObject gamedatafire = new GameObject("GameDataFire");
            gamedatafire.AddComponent<GameDataFire>();
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }

}
