using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScripts : MonoBehaviour
{
    #region fields
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private Canvas pauseCanvas = null;
    [SerializeField] private Canvas settingsCanvas = null;

    private Canvas previousCanvas = null;

    private bool paused = false;

    public GameObject loadingCanvas = null;
    public Slider loadingBar = null;
    public GameObject currentCanvas = null;

    [SerializeField] private AudioManager audioManager;

    #endregion
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseCanvas.gameObject.SetActive(!paused);
            if (paused)
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
        if (paused)
        {
            Time.timeScale = 0f;
            tutorial.paused = true;
        }
        else
        {
            Time.timeScale = 1f;
            tutorial.paused = false;
        }

        yield return null;
    }

    public void LoadLevel(string levelName)
    {
        Time.timeScale = 1f;
        loadingCanvas.SetActive(true);
        if(currentCanvas != null)
        currentCanvas.SetActive(false);
        StartCoroutine(LoadSceneAsync(levelName));
    }

    IEnumerator LoadSceneAsync(string levelName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(levelName);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);
            loadingBar.value = progress;
            //Debug.Log(op.progress);

            yield return null;
        }
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

    public void ChangeVolume(Slider slider)
    {
        audioManager.ChangeVolume(slider.value / 10);
    }
}
