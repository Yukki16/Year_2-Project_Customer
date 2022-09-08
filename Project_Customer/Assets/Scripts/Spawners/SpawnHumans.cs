using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHumans : MonoBehaviour
{
    [SerializeField] GameObject HumanPrefab;
    GameObject leftSpawnPoints, rightSpawnPoints, humans;
        
    private List<GameObject> spawnPoints;
    private Terrain playArea;
    public int MinHeightFromBottom = 10;
    public int MaxHeightFromBottom = 40;
    public int SpawnsPerSide = 5;

    private void Start()
    {
        spawnPoints = new List<GameObject>();

        //StartCoroutine(spawnHumans());

        humans = new GameObject(name: "Humans");
        humans.transform.SetParent(gameObject.transform);

        //instantiate spawn objects
        leftSpawnPoints = new GameObject(name: "LeftSpawnPoints");
        rightSpawnPoints = new GameObject(name: "RightSpawnPoints");
        leftSpawnPoints.transform.SetParent(gameObject.transform);
        rightSpawnPoints.transform.SetParent(gameObject.transform);

        playArea = Terrain.activeTerrain;

        SpawnHumanSpawns();

        StartCoroutine(spawnHumans());
    }

    void SpawnHumanSpawns()
    {

        //spawn spawns on the left 
        for (int i = 0; i < SpawnsPerSide; i++)
        {
            GameObject humanSpawn = new GameObject();
            humanSpawn.transform.position = new Vector3(playArea.transform.position.x, 
            playArea.transform.position.y, Random.Range(playArea.transform.position.z + MinHeightFromBottom,
            playArea.transform.position.z + MaxHeightFromBottom));
            spawnPoints.Add(humanSpawn);
            humanSpawn.transform.SetParent(leftSpawnPoints.transform);
        }

        //spawn spawns on the right 
        for (int i = 0; i < SpawnsPerSide; i++)
        {
            GameObject humanSpawn = new GameObject();
            humanSpawn.transform.position = new Vector3(playArea.terrainData.size.x,
            playArea.transform.position.y, Random.Range(playArea.transform.position.z + MinHeightFromBottom,
            playArea.transform.position.z + MaxHeightFromBottom));
            spawnPoints.Add(humanSpawn);
            humanSpawn.transform.SetParent(rightSpawnPoints.transform);
        }
    }

    IEnumerator spawnHumans()
    {
        GameObject newHuman = Instantiate(HumanPrefab, spawnPoints[Random.Range(0, spawnPoints.Count - 1)].transform);
        newHuman.transform.parent = humans.transform;
        yield return new WaitForSeconds(Random.Range(5, 10));
        StartCoroutine(spawnHumans());
    }
}
