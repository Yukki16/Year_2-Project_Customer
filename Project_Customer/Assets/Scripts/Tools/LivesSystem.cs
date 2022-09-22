using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LivesSystem : MonoBehaviour
{
    private float lives = 10;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private GameObject endsScreenCanvas;

    public IEnumerator UpdateLives()
    {
        lives--;
        livesText.text = lives.ToString();
        if (lives == 0)
            StartCoroutine(EndScreen());
        yield return null;
    }

    IEnumerator EndScreen()
    {
        Time.timeScale = 0f;
        endsScreenCanvas.SetActive(true);
        yield return null;
    }
}
