using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScripts : MonoBehaviour
{
    #region fields

    [SerializeField] private Canvas pauseCanvas;

    private bool paused = false;
    #endregion
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseCanvas.gameObject.SetActive(!paused);
            paused = !paused;
            StartCoroutine(PauseGame());
        }
    }

    public void ResumeGame()
    {
        pauseCanvas.gameObject.SetActive(!paused);
        paused = !paused;
        StartCoroutine(PauseGame());
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator PauseGame()
    {
        if(paused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        yield return null;
    }
}
