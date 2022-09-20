using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBehaviour : MonoBehaviour
{
    private bool locked;
    private bool hasBeenPickedUp;

    public void DisableBinCollection()
    {
        hasBeenPickedUp = true;
        locked = true;
    }

    public void EnableBinCollection()
    {
        locked = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bin") && !locked && hasBeenPickedUp)
        {
            Destroy(gameObject);
        }
    }
}
