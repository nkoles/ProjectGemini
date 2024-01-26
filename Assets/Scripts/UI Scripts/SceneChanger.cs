using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.WebRequestMethods;

public class SceneChanger : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void QuitGame()
    {
        string GamePageUrl = "https://damegev.itch.io/cutthroat";
        Application.OpenURL(GamePageUrl);
        Application.Quit();
    }
}
