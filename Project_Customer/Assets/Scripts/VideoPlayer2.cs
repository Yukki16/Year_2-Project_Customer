using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayer2 : MonoBehaviour
{
    public RawImage image;
    public VideoClip videoToPlay;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;

    void Start()
    {
        StartCoroutine(PlayVideo());
    }

    IEnumerator PlayVideo()
    {
        videoPlayer.clip = videoToPlay;
        videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            yield return waitForSeconds;
            break;
            //yield return null;
        }

        image.texture = videoPlayer.texture;
        videoPlayer.Play();
        //audioSource.Play();

    }
}
