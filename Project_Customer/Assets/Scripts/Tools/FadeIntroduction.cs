using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIntroduction : MonoBehaviour
{
    public Image imageToFade;
    public float fadeDuration = 3f;
    // Start is called before the first frame update
    void Start()
    {
        imageToFade.CrossFadeAlpha(0, fadeDuration, false);
        StartCoroutine(DisableFadeCanvas());
    }

    IEnumerator DisableFadeCanvas()
    {
        yield return new WaitForSeconds(fadeDuration);
        this.gameObject.SetActive(false);
    }
}
