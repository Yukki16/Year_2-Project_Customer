using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinBehaviour : MonoBehaviour
{
    private Transform trashPile;
    private int trashCounter;

    public void Start()
    {

        trashPile = transform.GetChild(0);
    }

    public void RaiseTrash()
    {
        trashCounter++;
        trashPile.transform.Translate(new Vector3(0, 0.05f, 0));
    }

    

}
