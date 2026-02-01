using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenLevelManager : MonoBehaviour
{
    public void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
