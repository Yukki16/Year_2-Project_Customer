using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehavior : MonoBehaviour
{
    public SpawnTurtles SpawnerParent;
    public int SpawnTimerMin;
    public int SpawnTimerMax;
    public float TurtleSizeScale;
    public GameObject turtlePrefab;

    public void Start()
    {
        SpawnerParent = GameObject.FindGameObjectWithTag("TurtleSpawner").GetComponent<SpawnTurtles>();
        turtlePrefab = SpawnerParent.GetTurtlePrefab();
        TurtleSizeScale = SpawnerParent.TurtleSizeScale;
        SpawnTimerMin = SpawnerParent.turtleSpawnTimeMin;
        SpawnTimerMax = SpawnerParent.turtleSpawnTimeMax;

        StartCoroutine(spawnTurtles());
    }

    IEnumerator spawnTurtles()
    {
        GameObject newTurtle = Instantiate(turtlePrefab, transform);
        newTurtle.transform.localScale = new Vector3(TurtleSizeScale, TurtleSizeScale, TurtleSizeScale);
        SpawnerParent.AddTurtle(newTurtle);    
       
        yield return new WaitForSeconds(Random.Range(SpawnTimerMin, SpawnTimerMax));
        StartCoroutine(spawnTurtles());     
    }


}
