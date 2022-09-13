using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSeagulls : MonoBehaviour
{
    [SerializeField] GameObject SeagullPrefab;
    private Terrain playArea;
    GameObject Seagulls;
    public int SeagullSpawnTimer = 5;

    void Start()
    {
        playArea = Terrain.activeTerrain;
        Seagulls = new GameObject(name: "Seagulls");
        Seagulls.transform.SetParent(gameObject.transform);
        StartCoroutine(spawnSeagulls());
    }

    IEnumerator spawnSeagulls()
    {
        yield return new WaitForSeconds(SeagullSpawnTimer);
        GameObject newSeagull = Instantiate(SeagullPrefab);
        newSeagull.transform.parent = Seagulls.transform;
        StartCoroutine(spawnSeagulls());
    }

}
