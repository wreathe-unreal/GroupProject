using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static double flashlightDecay = -0.5;
    public bool isActive = false;
    public int gameStartScene;

    // Update is called once per frame
    public void startGame()
    {
        SceneManager.LoadScene(gameStartScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public double decay()
    {
        return flashlightDecay;
    }

    public void doubleDrain(bool toggle)
    {
        if (toggle) flashlightDecay = -1;
        else flashlightDecay = -0.5;
    }
}
