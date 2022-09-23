using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHumans : MonoBehaviour
{
    [SerializeField] GameObject HumanPrefab;
    GameObject leftSpawnPoints, rightSpawnPoints, humans, targets;

    MasterFlow masterFlow;
    private List<GameObject> spawnPoints;
    private Terrain playArea;
    public int DistanceFromBorder = 25;
    public int MinHeightFromBottom = 10;
    public int MaxHeightFromBottom = 30;
    public int SpawnsPerSide = 5;
    public bool EnableSpawning;
    private bool crRunning;

    private void Start()
    {
        masterFlow = FindObjectOfType<MasterFlow>();
        EnableSpawning = true;
        spawnPoints = new List<GameObject>();

        humans = new GameObject(name: "Humans");
        humans.transform.SetParent(gameObject.transform);

        targets = new GameObject(name: "HumanTargets");
        targets.transform.SetParent(gameObject.transform);
        targets.tag = "HumanTargets";

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
            humanSpawn.transform.position = new Vector3(playArea.transform.position.x + DistanceFromBorder,
            playArea.transform.position.y, Random.Range(playArea.transform.position.z + MinHeightFromBottom,
            playArea.transform.position.z + MaxHeightFromBottom));
            spawnPoints.Add(humanSpawn);
            humanSpawn.transform.SetParent(leftSpawnPoints.transform);
        }

        //spawn spawns on the right 
        for (int i = 0; i < SpawnsPerSide; i++)
        {
            GameObject humanSpawn = new GameObject();
            humanSpawn.transform.position = new Vector3(playArea.terrainData.size.x - DistanceFromBorder,
            playArea.transform.position.y, Random.Range(playArea.transform.position.z + MinHeightFromBottom,
            playArea.transform.position.z + MaxHeightFromBottom));
            spawnPoints.Add(humanSpawn);
            humanSpawn.transform.SetParent(rightSpawnPoints.transform);
        }
    }

    IEnumerator spawnHumans()
    {
        yield return new WaitUntil(() => EnableSpawning == true);
        GameObject newHuman = Instantiate(HumanPrefab, spawnPoints[Random.Range(0, spawnPoints.Count - 1)].transform);
        newHuman.transform.parent = humans.transform;
        yield return new WaitForSeconds(masterFlow.GetHumanSpawnRate());
        StartCoroutine(spawnHumans());
    }

    private void Update()
    {


    }
}