using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadWIn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            LoadSceneByName("Win");
        }
    }

    private void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name);
    }
}
