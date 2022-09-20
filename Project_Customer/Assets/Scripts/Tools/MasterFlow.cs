using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterFlow : MonoBehaviour
{
    private Terrain playArea;
    public SpawnCrabs spawnCrabs;
    public SpawnHumans spawnHumans;
    public SpawnSeagulls spawnSeagulls;
    public SpawnTurtles spawnTurtles;
    public GameObject trashPrefab;
    public GameObject draggablesParent;
    public int intlTrashPieces;

    private int intlSpawnHumanDelay = 10;
    private int intlSpawnSeagullDelay = 30;
    private int intlSpawnCrabDelay = 50;

    public int TurtleSpawnDelay = 15;
    public int HumanSpawnDelay = 9;
    public int SeagullSpawnDelay = 12;
    public int CrabSpawnDelay = 15;

    public int ObjSpawnRateIncreaseDelay = 15;

    private GameObject trashSpot;

    int maxDistanceFromCenter = 15;
    int intlTrashMaxBottomDistance = 36;
    int intlTrashMinBottomDistance = 12;

    private bool preloadCheck { get; }

    void Start()
    {
        spawnCrabs.enabled = false;
        spawnHumans.enabled = false;
        spawnSeagulls.enabled = false;

        trashSpot = new GameObject();
        playArea = Terrain.activeTerrain;
        SpawnInitialTrash();

        StartCoroutine(StartSpawns());
        StartCoroutine(IncreaseSpawnRates());
    }

    #region SpawnSettings

    private IEnumerator StartSpawns()
    {
        StartCoroutine(StartHumanSpawn());
        StartCoroutine(StartCrabSpawn());
        StartCoroutine(StartSeagullSpawn());
        yield return null;
    }
    private IEnumerator IncreaseSpawnRates()
    {
        StartCoroutine(IncreaseTurtleSpawnRate());
        StartCoroutine(IncreaseHumanSpawnRate());
        StartCoroutine(IncreaseSeagullSpawnRate());
        StartCoroutine(IncreaseCrabSpawnRate());
        yield return null;
    }

    private IEnumerator StartHumanSpawn()
    {
        yield return new WaitForSeconds(intlSpawnHumanDelay);
        spawnHumans.enabled = true;
    }

    private IEnumerator StartSeagullSpawn()
    {
        yield return new WaitForSeconds(intlSpawnSeagullDelay);
        spawnSeagulls.enabled = true;
    }

    private IEnumerator StartCrabSpawn()
    {
        yield return new WaitForSeconds(intlSpawnCrabDelay);
        spawnCrabs.enabled = true;
    }

    private IEnumerator IncreaseTurtleSpawnRate()
    {
        yield return new WaitForSeconds(ObjSpawnRateIncreaseDelay);
        if (TurtleSpawnDelay > 5)
        {
            TurtleSpawnDelay -= 1;
            StartCoroutine(IncreaseTurtleSpawnRate());
        }
    }

    private IEnumerator IncreaseHumanSpawnRate()
    {
        yield return new WaitForSeconds(ObjSpawnRateIncreaseDelay);
        if (HumanSpawnDelay > 2)
        {
            HumanSpawnDelay -= 1;
            StartCoroutine(IncreaseHumanSpawnRate());
        }
    }

    private IEnumerator IncreaseSeagullSpawnRate()
    {
        yield return new WaitForSeconds(ObjSpawnRateIncreaseDelay);
        if (SeagullSpawnDelay > 2)
        {
            SeagullSpawnDelay -= 1;
            StartCoroutine(IncreaseSeagullSpawnRate());
        }
    }

    private IEnumerator IncreaseCrabSpawnRate()
    {
        yield return new WaitForSeconds(ObjSpawnRateIncreaseDelay);
        if (CrabSpawnDelay > 2)
        {
            CrabSpawnDelay -= 1;
            StartCoroutine(IncreaseCrabSpawnRate());
        }
    }

    #endregion

    #region ReturnRates

    public int GetEggSpawnRate()
    {
        return TurtleSpawnDelay;
    }

    public int GetTurtleSpawnRate()
    {
        return TurtleSpawnDelay;
    }

    public int GetSeagullSpawnRate()
    {
        return SeagullSpawnDelay;
    }

    public int GetHumanSpawnRate()
    {
        return HumanSpawnDelay;
    }

    public int GetCrabSpawnRate()
    {
        return CrabSpawnDelay;
    }
    #endregion

    private GameObject randomTrash()
    {
        Transform toGameobject = trashPrefab.transform.GetChild(Random.Range(0, trashPrefab.transform.childCount));
        return toGameobject.gameObject;
    }

    private void SpawnInitialTrash()
    {
        for (int i = 0; i < intlTrashPieces; i++)
        {
            trashSpot.transform.position = new Vector3(playArea.terrainData.size.x / 2 + Random.Range(-maxDistanceFromCenter, maxDistanceFromCenter),
            playArea.transform.position.y + 3, Random.Range(playArea.transform.position.z + intlTrashMinBottomDistance,
            playArea.transform.position.z + intlTrashMaxBottomDistance));
            GameObject newTrash = Instantiate(randomTrash(), trashSpot.transform);
            newTrash.transform.SetParent(draggablesParent.transform);
        }
    }
}
