using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleFootStep : MonoBehaviour
{
    AudioManager am;
    public void Start()
    {
        am = FindObjectOfType<AudioManager>();
    }
    public void FootStep()
    {
        am.Play("turtlestep", true);
    }
}
