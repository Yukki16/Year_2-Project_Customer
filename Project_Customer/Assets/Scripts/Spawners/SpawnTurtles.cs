using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTurtles : MonoBehaviour
{
    #region fields
    private List<GameObject> spawnPoints;
    private List<GameObject> turtleList;

    private Terrain playArea;

    GameObject turtles;

    GameObject targets;

    [SerializeField] private GameObject turtlePrefab;
    [SerializeField] private GameObject eggPrefab;

    private int spawnEggStep, leftEggs = 1, rightEggs;

    public int turtleSpawnTimeMin = 8;
    public int turtleSpawnTimeMax = 14;
    public int EggSeperationDistance = 5;
    public float TurtleSizeScale = 3.5f;
    public int MaxEggsTotal = 6;
    public int EggSpawnTimer;

    GameObject TurtleSpawnEggs;
    #endregion

    private void Start()
    {
        turtles = new GameObject(name: "Turtles");
        turtles.transform.SetParent(gameObject.transform);

        targets = new GameObject(name: "TurtleTargets");
        targets.transform.SetParent(gameObject.transform);
        targets.tag = "TurtleTargets";

        playArea = Terrain.activeTerrain;

        spawnPoints = new List<GameObject>();
        turtleList = new List<GameObject>();

        TurtleSpawnEggs = new GameObject();
        TurtleSpawnEggs.transform.SetParent(transform);

 
        StartCoroutine(SpawnSpawners());
    }

    public List<GameObject> GetTurtles()
    {
        return turtleList;
    }

    public void AddTurtle(GameObject turtle)
    {
        turtleList.Add(turtle);
    }

    public GameObject GetTurtlePrefab()
    {
        return turtlePrefab;
    }

    IEnumerator SpawnSpawners()
    {
        GameObject newEgg = Instantiate(eggPrefab, TurtleSpawnEggs.transform);



        if (spawnEggStep % 2 == 0)
        {
            //Spawn Egg on Right Side
            newEgg.transform.position = new Vector3((playArea.terrainData.size.x / 2) + rightEggs * EggSeperationDistance, 0.15f, 3);
            rightEggs++;
        }
        else
        {
            //Spawn Egg on Left Side
            newEgg.transform.position = new Vector3(playArea.terrainData.size.x / 2 + leftEggs * -EggSeperationDistance, 0.15f, 3);
            leftEggs++;
        }

        spawnPoints.Add(newEgg);

        if (spawnEggStep == MaxEggsTotal)
        {
            yield break;
        }

        yield return new WaitForSeconds(EggSpawnTimer);
        spawnEggStep++;
        StartCoroutine(SpawnSpawners());

    }  
}
