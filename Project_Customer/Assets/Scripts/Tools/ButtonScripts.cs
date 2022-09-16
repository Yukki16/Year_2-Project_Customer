using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScripts : MonoBehaviour
{
    #region fields

    [SerializeField] private Canvas pauseCanvas = null;
    [SerializeField] private Canvas settingsCanvas = null;

    private Canvas previousCanvas = null;

    private bool paused = false;
    #endregion
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseCanvas.gameObject.SetActive(!paused);
            if(paused)
            {
                settingsCanvas.gameObject.SetActive(!paused);
            }
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
    
    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Options(Canvas canvas)
    {
        settingsCanvas.gameObject.SetActive(true);
        previousCanvas = canvas;
        previousCanvas.gameObject.SetActive(false);
    }
    public void Back()
    {
        settingsCanvas.gameObject.SetActive(false);
        previousCanvas.gameObject.SetActive(true);
    }
}
