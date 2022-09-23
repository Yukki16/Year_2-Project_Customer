using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckBehaviour : MonoBehaviour
{
    GameObject targetObj;
    Terrain playArea;
    float TargetDistanceFromBorder = 25;

    void Start()
    {
        playArea = Terrain.activeTerrain;
        SpawnTarget();
    }

    public GameObject returnTargetObj()
    {
        return targetObj;
    }

    void SpawnTarget()
    {
        //if human is to the left, spawn to the right
        if (transform.position.x < playArea.transform.position.x + playArea.terrainData.size.x / 2)
        {
            Debug.Log("spawn1");
            targetObj = new GameObject(name: "TruckTarget");
            targetObj.transform.position = new Vector3(playArea.terrainData.size.x - TargetDistanceFromBorder, transform.position.y, transform.position.z);
        }

        //if human is to the right, spawn to the left
        if (transform.position.x > playArea.transform.position.x + playArea.terrainData.size.x / 2)
        {
            Debug.Log("spawn2");
            targetObj = new GameObject(name: "TruckTarget");
            targetObj.transform.position = new Vector3(playArea.transform.position.x + TargetDistanceFromBorder, transform.position.y, transform.position.z);
        }

        GetComponent<Mover>().SetTarget(targetObj.transform);
    }
}
