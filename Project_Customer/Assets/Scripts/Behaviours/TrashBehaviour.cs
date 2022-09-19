using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBehaviour : MonoBehaviour
{
    private bool locked;

    public void DisableBinCollection()
    {
        locked = true;
    }

    public void EnableBinCollection()
    {
        locked = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bin") && !locked)
        {
            Destroy(gameObject);
        }
    }
}
