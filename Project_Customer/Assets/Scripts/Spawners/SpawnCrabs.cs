using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCrabs : MonoBehaviour
{
    [SerializeField] GameObject CrabPrefab;
    GameObject crabs;

    MasterFlow masterFlow;
    private GameObject targets;
    private Terrain playArea;

    public int MaxDistanceFromCenter = 20;
    public int MinHeightFromBottom = 32;
    public int MaxHeightFromBottom = 42;

    void Start()
    {
        masterFlow = FindObjectOfType<MasterFlow>();

        playArea = Terrain.activeTerrain;
        crabs = new GameObject(name: "Crabs");
        crabs.transform.SetParent(gameObject.transform);

        targets = new GameObject(name: "CrabTargets");
        targets.transform.SetParent(gameObject.transform);
        targets.tag = "CrabTargets";
            
        StartCoroutine(spawnCrabs());   
    }

    IEnumerator spawnCrabs()
    {        
        yield return new WaitForSeconds(masterFlow.GetCrabSpawnRate());
        GameObject newCrab = Instantiate(CrabPrefab, position: new Vector3(playArea.terrainData.size.x / 2 + Random.Range(-MaxDistanceFromCenter, MaxDistanceFromCenter),
        playArea.transform.position.y, Random.Range(playArea.transform.position.z + MinHeightFromBottom,
        playArea.transform.position.z + MaxHeightFromBottom)), rotation: Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
        newCrab.transform.parent = crabs.transform;
        StartCoroutine(spawnCrabs());
    }

    private void Update()
    {
        Debug.Log("Active");
    }
}
    