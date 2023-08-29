using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static double flashlightDecay = -0.5;    // Does not work currently
    static public string finalTime = "0:00";
    public bool isActive = false;

    public GameObject loadBar;
    public Image progBar;

    private TextMeshProUGUI text;
    private AsyncOperation toLoad;

    private int counter = 0;

    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "VictoryScreen")
        {
            if (counter == 0)
            {
                text = GameObject.Find("End Screen/End Time").GetComponent<TextMeshProUGUI>();
                text.text = text.text + finalTime;
                counter++;
            }
        }
        else if (counter != 0) counter = 0;
    }
    
    public void startGame()
    {
        loading();
        toLoad = SceneManager.LoadSceneAsync("Horror Map");
        StartCoroutine(loadScreen());
    }

    public void returnToMenu()
    {
        finalTime = "";
        SceneManager.LoadScene("MainMenu");
    }

    public void victoryScreen()
    {
        TextMeshProUGUI timeText = GameObject.Find("UI/Timer").GetComponent<TextMeshProUGUI>();
        finalTime = timeText.text;
        SceneManager.LoadScene("VictoryScreen");
        Cursor.visible = true;
    }

    public void QuitGame()
    {
        finalTime = "";
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
