using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public int gameStartScene;

    // Update is called once per frame
    public void startGame()
    {
        SceneManager.LoadScene("Horror Map");
    }
}
