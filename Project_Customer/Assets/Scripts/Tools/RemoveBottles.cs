using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBottles : MonoBehaviour
{
    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).transform.position.y < -2)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
 

    }
}
