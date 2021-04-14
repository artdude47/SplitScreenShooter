using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    public Text winText;
    public WinConditionSO win;

    private void Start()
    {
        Cursor.visible = true;
        winText.text = "Player " + win.playerWhoWon + " has won!!";
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
