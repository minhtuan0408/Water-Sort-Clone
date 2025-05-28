using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static LoadGameData LoadGameData { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // ReSharper disable once FlagArgument
    public static void LoadScene(string sceneName, bool showLoading = true, float loadingScreenSpeed = 5f)
    {
            SceneManager.LoadScene(sceneName);
    }
    public static void LoadGame(LoadGameData data, bool showLoading = true, float loadingScreenSpeed = 1f)
    {
        LoadGameData = data;
        LoadScene("GamePlay", showLoading, loadingScreenSpeed);
    }
}
public struct LoadGameData
{
    public Level Level { get; set; }
}