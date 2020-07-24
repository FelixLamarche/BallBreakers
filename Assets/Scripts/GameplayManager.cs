using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Class that is responsible to keep track of all the other managers
// and the general information unrelated to a specific scene
public class GameplayManager : MonoBehaviour
{
    public static LevelManager LevelManager
    {
        get; private set;
    }

    private void Awake()
    {
        if (FindObjectsOfType<GameplayManager>().Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Called here for now, until multiple scenes are implemented
        OnSceneLoaded();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void LoadGameplayScene()
    {
        SceneManager.LoadScene("GameplayScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Call this when implementing multiple scenes
    private void OnSceneLoaded()
    {
        SetCurrentLevelManager();
    }

    private void SetCurrentLevelManager()
    {
        LevelManager[] levelManagers = FindObjectsOfType<LevelManager>();

        // FOR NOW : Do nothing if there is no Level Manager
        if(levelManagers.Length == 0)
        {
            Debug.LogWarning("No LevelManager here");
            LevelManager = null;
            return;
        }
        else if(levelManagers.Length >= 2)
        {
            Debug.LogError("Multiple LevelManager in current scene");
        }

        LevelManager = levelManagers[0];
    }
}
