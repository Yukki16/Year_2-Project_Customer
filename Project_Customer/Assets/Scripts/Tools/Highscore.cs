using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Highscore : MonoBehaviour
{
    public float highscore = 0;
    public TMP_Text highscoreText;
    public TMP_Text endscreenHSText;
    private void Start()
    {
        highscoreText.text = highscore.ToString();
        endscreenHSText.text = highscore.ToString();
    }
    void Update()
    {
        
    }

    public IEnumerator AddScore()
    {
        highscore += 500;
        highscoreText.text = highscore.ToString();
        endscreenHSText.text = highscore.ToString();
        yield return null;
    }
}
