using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinBehaviour : MonoBehaviour
{
    private Transform trashPile;
    private int trashCounter;
    private float trashRaise = 0.05f;
    private int timesFilled;

    [SerializeField] PowerupEvent powerupEventManager;

    public void Start()
    {
        trashPile = transform.GetChild(0);
    }

    public void RaiseTrash()
    {
        FindObjectOfType<AudioManager>().PlayRandom(new string[] { "Trash1", "Trash2", "Trash3" }, true);
        trashCounter++;
        trashPile.transform.Translate(new Vector3(0, trashRaise, 0));
    }

    public void Update()
    {
        if (trashCounter == powerupEventManager.ReturnTrashThreshold())
        {
            powerupEventManager.StartRandomEvent();
            timesFilled++;  
            trashCounter = 0;
            trashPile.transform.Translate(new Vector3(0, -trashRaise * powerupEventManager.ReturnTrashThreshold(), 0));
        }
    }


}
