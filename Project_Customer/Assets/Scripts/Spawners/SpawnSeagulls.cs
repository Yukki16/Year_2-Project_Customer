using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSeagulls : MonoBehaviour
{
    [SerializeField] GameObject SeagullPrefab;
    private Terrain playArea;
    private GameObject[] Turtles;
    GameObject Seagulls;
    public int SeagullSpawnTimer = 5;
    public bool EnableSpawning;
    public bool SpawnSeagull;

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
        if (EnableSpawning)
        {
            GameObject newSeagull = Instantiate(SeagullPrefab);
            newSeagull.transform.parent = Seagulls.transform;        
        }      
        StartCoroutine(spawnSeagulls());
    }

    private void Update()
    {
        if (SpawnSeagull)
        {
            GameObject newSeagull = Instantiate(SeagullPrefab);
            newSeagull.transform.parent = Seagulls.transform;
            SpawnSeagull = false;
        }        
    }

}
