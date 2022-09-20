using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehavior : MonoBehaviour
{
    public SpawnTurtles SpawnerParent;
    public float TurtleSizeScale;
    public GameObject turtlePrefab;
    
    MasterFlow masterFlow;

    public void Start()
    {
        masterFlow = GameObject.FindObjectOfType<MasterFlow>();
        SpawnerParent = GameObject.FindGameObjectWithTag("TurtleSpawner").GetComponent<SpawnTurtles>();
        turtlePrefab = SpawnerParent.GetTurtlePrefab();
        TurtleSizeScale = SpawnerParent.TurtleSizeScale;

        StartCoroutine(spawnTurtles());
    }

    IEnumerator spawnTurtles()
    {
        GameObject newTurtle = Instantiate(turtlePrefab, transform);
        newTurtle.transform.localScale = new Vector3(TurtleSizeScale, TurtleSizeScale, TurtleSizeScale);
        SpawnerParent.AddTurtle(newTurtle);    
       
        yield return new WaitForSeconds(masterFlow.GetTurtleSpawnRate());
        StartCoroutine(spawnTurtles());     
    }


}
