using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Text wintext;

    private void Start()
    {
        if (wintext != null)
            wintext.text = PlayerPrefs.GetInt("Won") == 1 ? "You won the Challenge !" : "You FAILED...";
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LeaveGameOver()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
