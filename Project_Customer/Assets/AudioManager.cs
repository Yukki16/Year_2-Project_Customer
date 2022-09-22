using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name, bool randomizePitch = false)
    {
        foreach (var sound in sounds)
        {
            if (sound.name == name)
            {
                if (randomizePitch)
                {
                    sound.source.pitch = 1;
                    sound.source.pitch += UnityEngine.Random.Range(-0.1f, 0.1f);
                }

                sound.source.Play();
            }
        }
    }

    public void PlayRandom(string[] name, bool randomizePitch = false)
    {
        foreach (var sound in sounds)
        {
            if (sound.name == name[UnityEngine.Random.Range(0, name.Length)])
            {
                if (randomizePitch)
                {
                    sound.source.pitch = 1;
                    sound.source.pitch += UnityEngine.Random.Range(-0.1f, 0.1f);
                }

                sound.source.Play();
            }
        }
    }

}
