using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public void StartGame()
    {
        UIUtils.LoadSceneWithDelay("MainGame", 0.1f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
