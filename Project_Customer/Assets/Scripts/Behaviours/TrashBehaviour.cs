using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBehaviour : MonoBehaviour
{
    private bool isActive;
    private bool locked;
    public bool hasBeenPickedUp;

    private Highscore highscore;

    private void Start()
    {
        highscore = FindObjectOfType<Highscore>();
    }

    public void DisableBinCollection()
    {
        isActive = true;
        hasBeenPickedUp = true;
        locked = true;
    }

    public void EnableBinCollection()
    {
        isActive = false;
        locked = false;
    }

    public bool GetIsActive()
    {
        return isActive;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("bin") && !locked && hasBeenPickedUp)
        {
            collision.gameObject.GetComponent<BinBehaviour>().RaiseTrash();
            StartCoroutine(highscore.AddScore());
            Destroy(gameObject);
        }
    }
}
