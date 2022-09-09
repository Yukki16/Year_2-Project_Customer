using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTurtles : MonoBehaviour
{
    private List<GameObject> spawnPoints;

    private Terrain playArea;

    GameObject turtles;

    GameObject targets;

    [SerializeField] private GameObject turtlePrefab;
    [SerializeField] private GameObject eggPrefab;


    public int turtleSpawnMinTime = 10;
    public int turtleSpawnMaxTime = 10;
    public int EggSpawnDistance = 15;
    public int EggSpawnTimer = 15;

    GameObject TurtleSpawnEggs;

    private void Start()
    {
        turtles = new GameObject(name: "Turtles");
        turtles.transform.SetParent(gameObject.transform);

        targets = new GameObject(name: "TurtleTargets");
        targets.transform.SetParent(gameObject.transform);
        targets.tag = "TurtleTargets";

        playArea = Terrain.activeTerrain;

        spawnPoints = new List<GameObject>();

        TurtleSpawnEggs = new GameObject();
        TurtleSpawnEggs.transform.SetParent(transform);

 
        StartCoroutine(SpawnSpawners());
        StartCoroutine(spawnTurtles());
    }

    IEnumerator SpawnSpawners()
    {
        GameObject newEgg = Instantiate(eggPrefab, TurtleSpawnEggs.transform);
        newEgg.transform.position = new Vector3(playArea.terrainData.size.x / 2 + (Random.Range(-EggSpawnDistance, EggSpawnDistance)), 0.15f, 3);
        spawnPoints.Add(newEgg);
        yield return new WaitForSeconds(EggSpawnTimer);
        StartCoroutine(SpawnSpawners());
    }

    IEnumerator spawnTurtles()
    {
        foreach (var spawner in spawnPoints)
        {
            GameObject newTurtle = Instantiate(turtlePrefab, spawner.transform);
        }
        yield return new WaitForSeconds(Random.Range(turtleSpawnMinTime, turtleSpawnMaxTime));
        StartCoroutine(spawnTurtles());
    }
}
