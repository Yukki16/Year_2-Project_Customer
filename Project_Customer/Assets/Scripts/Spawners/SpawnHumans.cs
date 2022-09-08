using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHumans : MonoBehaviour
{
    [SerializeField] GameObject HumanPrefab;
    private List<GameObject> spawnPoints;
    private Terrain playArea;
    public int MinHeightFromFinish = 10;
    public int MaxHeightFromFinish = 40;

    private void Start()
    {
       

        //StartCoroutine(spawnHumans());

        playArea = Terrain.activeTerrain;

        SpawnHumanSpawns();
    }

    void SpawnHumanSpawns()
    {

        //spawn spawns on the left 
        for (int i = 0; i < 10; i++)
        {
            GameObject humanSpawn = new GameObject();
            humanSpawn.transform.position = new Vector3(playArea.transform.position.x, 
            Random.Range(playArea.terrainData.size.z - MinHeightFromFinish,
            playArea.terrainData.size.z - MaxHeightFromFinish), playArea.terrainData.size.z);
            spawnPoints.Add(humanSpawn);
        }

    }

    IEnumerator spawnHumans()
    {
        GameObject newTurtle = Instantiate(HumanPrefab, spawnPoints[Random.Range(0, spawnPoints.Count - 1)].transform);
        newTurtle.transform.parent = null;
        yield return new WaitForSeconds(Random.Range(5, 10));
        StartCoroutine(spawnHumans());
    }
    void Update()
    {

    }
}
