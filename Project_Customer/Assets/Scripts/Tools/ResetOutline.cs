using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOutline : MonoBehaviour
{
    Outline outline;

    private void Start()
    {
        outline = GetComponent<Outline>();
    }

    void Update()
    {
        outline.OutlineWidth = 0;
    }
}
