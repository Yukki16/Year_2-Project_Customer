using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingGame : MonoBehaviour
{
    public ButtonScripts load;
    private void Start()
    {
        load.LoadLevel("MainMenu");
    }
}
