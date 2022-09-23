using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanFootstep : MonoBehaviour
{
    AudioManager am;
    public void Awake()
    {
        am = FindObjectOfType<AudioManager>();
    }
    public void FootStep()
    {
        if (am != null)
        am.PlayRandom(new string[] { "Step1", "Step2", "Step3", "Step4", "Step5", "Step6", "Step7"}, true);
    }
}
