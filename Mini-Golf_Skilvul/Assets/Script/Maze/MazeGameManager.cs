using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject player;
    [SerializeField] Hole hole;

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (hole.Entered && gameOverPanel.activeInHierarchy == false)
        {
            gameOverPanel.SetActive(true);
        }

    }

    public void BackToMainMenu()
    {
        SceneLoader.Load("MainMenu");
    }

    public void Replay()
    {
        SceneLoader.ReloadLevel();
    }

    public static void Load(string sceneName)
    {
        SceneLoader.Load(sceneName);
    }
}
