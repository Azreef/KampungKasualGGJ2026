using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScreenTransition : MonoBehaviour
{
    public CanvasGroup panelGroup;
    public RectTransform radialImage;
    public float duration = 1f;
    public float maxSize = 2000f;

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        Play();
    //    }
    //}

    public void Play()
    {
            StartCoroutine(RevealLose());
    }

    public void Win()
    {
        StartCoroutine(RevealWin());
    }

    
    IEnumerator RevealLose()
    {
        float time = 0f;

        panelGroup.alpha = 0f;
        radialImage.sizeDelta = Vector2.zero;
        radialImage.gameObject.SetActive(true);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // Fade panel
            panelGroup.alpha = t;

            // Expand circle
            float size = Mathf.SmoothStep(0, maxSize, t);
            radialImage.sizeDelta = new Vector2(size, size);

            yield return null;
        }

        panelGroup.alpha = 1f;
        panelGroup.interactable = true;
        panelGroup.blocksRaycasts = true;

        radialImage.gameObject.SetActive(false);

        LoadSceneByName("Game Over");

        
    }

    IEnumerator RevealWin()
    {
        float time = 0f;

        panelGroup.alpha = 0f;
        radialImage.sizeDelta = Vector2.zero;
        radialImage.gameObject.SetActive(true);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // Fade panel
            panelGroup.alpha = t;

            // Expand circle
            float size = Mathf.SmoothStep(0, maxSize, t);
            radialImage.sizeDelta = new Vector2(size, size);

            yield return null;
        }

        panelGroup.alpha = 1f;
        panelGroup.interactable = true;
        panelGroup.blocksRaycasts = true;

        radialImage.gameObject.SetActive(false);

        LoadSceneByName("Win");


    }

    public void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name);
    }
}
