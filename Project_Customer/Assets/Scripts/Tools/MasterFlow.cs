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

    public int intlSpawnEggDelay = 3;
    public int intlSpawnHumanDelay = 10;
    public int intlSpawnSeagullDelay = 30;
    public int intlSpawnCrabDelay = 120;

    public int EggSpawnDelay = 20;
    public int TurtleSpawnDelay = 7;
    public int HumanSpawnDelay = 9;
    public int SeagullSpawnDelay = 12;
    public int CrabSpawnDelay = 20;
    public int TurtleDeathTime = 5;

    public int hmnDropRateMin = 3;
    public int hmnDropRateMax = 7;

    public int ObjSpawnRateIncreaseDelay = 15;

    private GameObject trashSpot;

    private readonly int maxDistanceFromCenter = 15;
    private readonly int intlTrashMaxBottomDistance = 36;
    private readonly int intlTrashMinBottomDistance = 12;

    private bool scareCrowActive;

    private bool preloadCheck { get; }

    void Start()
    {
        Application.targetFrameRate = 60;

        spawnTurtles.enabled = false;
        spawnCrabs.enabled = false;
        spawnHumans.enabled = false;
        spawnSeagulls.enabled = false;

        trashSpot = new GameObject();
        playArea = Terrain.activeTerrain;
        SpawnInitialTrash();

        StartCoroutine(StartSpawns());

        //StartCoroutine(PingSeagullRate());
    }

    private IEnumerator PingSeagullRate()
    {
        Debug.Log(SeagullSpawnDelay);
        yield return new WaitForSeconds(1);
        StartCoroutine(PingSeagullRate());
    }


        #region SpawnSettings

        private IEnumerator StartSpawns()
    {
        StartCoroutine(StartEggSpawn());
        StartCoroutine(StartHumanSpawn());
        StartCoroutine(StartCrabSpawn());
        StartCoroutine(StartSeagullSpawn());
        yield return null;
    }

    private IEnumerator StartEggSpawn()
    {
        yield return new WaitForSeconds(intlSpawnEggDelay);
        spawnTurtles.enabled = true;
        StartCoroutine(IncreaseEggSpawnRate());
    }

    private IEnumerator StartHumanSpawn()
    {
        yield return new WaitForSeconds(intlSpawnHumanDelay);
        spawnHumans.enabled = true;
        StartCoroutine(IncreaseHumanSpawnRate());
    }

    private IEnumerator StartSeagullSpawn()
    {
        yield return new WaitForSeconds(intlSpawnSeagullDelay);
        spawnSeagulls.enabled = true;
        StartCoroutine(IncreaseSeagullSpawnRate());
    }

    private IEnumerator StartCrabSpawn()
    {
        yield return new WaitForSeconds(intlSpawnCrabDelay);
        spawnCrabs.enabled = true;  
        StartCoroutine(IncreaseCrabSpawnRate());
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
            //SeagullSpawnDelay -= 1;
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
    private IEnumerator IncreaseEggSpawnRate()
    {
        yield return new WaitForSeconds(ObjSpawnRateIncreaseDelay);
        if (EggSpawnDelay > 10)
        {
            EggSpawnDelay -= 1;
            StartCoroutine(IncreaseEggSpawnRate());
        }
    }

    #endregion

    #region ReturnRates

    public int ReturnHumanDropRate()
    {
        return (Random.Range(hmnDropRateMin, hmnDropRateMax)); 
    }

    public int ReturnTurtleDeathTime()
    {
        return TurtleDeathTime;
    }
    public int GetEggSpawnRate()
    {
        return EggSpawnDelay;
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


    public void ActivateScareCrow()
    {
        spawnSeagulls.EnableSpawning = false;
    }

    public void DeactivateScareCrow()
    {
        spawnSeagulls.EnableSpawning = true;
    }

    public void ActivateIceCream()
    {
        spawnHumans.EnableSpawning = false;
    }

    public void DeactivateIceCream()
    {
        spawnHumans.EnableSpawning = true;
    }

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
            playArea.transform.position.y + 5, Random.Range(playArea.transform.position.z + intlTrashMinBottomDistance,
            playArea.transform.position.z + intlTrashMaxBottomDistance));
            GameObject newTrash = Instantiate(randomTrash(), trashSpot.transform);
            newTrash.transform.SetParent(draggablesParent.transform);
        }
    }
}
