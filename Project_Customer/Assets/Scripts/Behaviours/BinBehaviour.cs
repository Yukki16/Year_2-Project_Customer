using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinBehaviour : MonoBehaviour
{
    private Transform trashPile;
    private int trashCounter;
    private int trashThreshold = 1;
    private float trashRaise = 0.07f;
    private int timesFilled;

    [SerializeField] PowerupEvent powerupEventManager;

    public void Start()
    {
        trashPile = transform.GetChild(0);
    }

    public void RaiseTrash()
    {
        trashCounter++;
        trashPile.transform.Translate(new Vector3(0, trashRaise, 0));
    }

    public void Update()
    {
        if (trashCounter == trashThreshold)
        {
            powerupEventManager.StartRandomEvent();
            timesFilled++;  
            trashCounter = 0;
            trashPile.transform.Translate(new Vector3(0, -trashRaise * trashThreshold, 0));
        }
    }


}
