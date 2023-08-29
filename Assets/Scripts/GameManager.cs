using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static double flashlightDecay = -0.5;    // Does not work currently
    public bool isActive = false;
    public int gameStartScene;                      // Not needed if scene remains the same name

    public GameObject loadBar;
    public Image progBar;

    private AsyncOperation toLoad;

    // Update is called once per frame
    public void startGame()
    {
        loading();
        toLoad = SceneManager.LoadSceneAsync("Horror Map");
        StartCoroutine(loadScreen());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void loading()
    {
        loadBar.SetActive(true);
    }

    IEnumerator loadScreen()
    {
        float progress = 0f;
        while (!toLoad.isDone)
        {
            progress += toLoad.progress;
            progBar.fillAmount = progress;
            yield return null;
        }
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
