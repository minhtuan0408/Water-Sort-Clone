using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    public void LoadNextLevel()
    {
        int currentLevelNo = GameManager.LoadGameData.Level.no;
        int nextLevelNo = currentLevelNo + 1;

        Level nextLevel = ResourceManager.GetLevel(nextLevelNo);

        if (nextLevel != null)
        {
            GameManager.LoadGame(new LoadGameData
            {
                Level = nextLevel
            });
        }
        else
        {
            Debug.Log("Không có level tiếp theo.");
        }
    }
}
