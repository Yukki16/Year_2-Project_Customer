using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupEvent : MonoBehaviour
{
    Terrain playArea;
    [SerializeField] GameObject ScareCrowPrefab;
    private GameObject activeScareCrow;


    private void Start()
    {
        playArea = Terrain.activeTerrain;
    }

    public void StartRandomEvent()
    {
        StartCoroutine(RandomEvent());
        return;
    }

    public IEnumerator RandomEvent()
    {
        GameObject scrareCrow = Instantiate(ScareCrowPrefab, position: new Vector3(50, 0, 20), rotation: Quaternion.Euler(Vector3.zero));
        yield return null;
    }




}
