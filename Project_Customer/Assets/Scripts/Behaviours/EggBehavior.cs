using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehavior : MonoBehaviour
{
    private SpawnTurtles SpawnerParent;
    public float TurtleSizeScale;
    public GameObject turtlePrefab;
    
    MasterFlow masterFlow;

    public void Start()
    {
        
        masterFlow = GameObject.FindObjectOfType<MasterFlow>();
        SpawnerParent = GameObject.FindGameObjectWithTag("TurtleSpawner").GetComponent<SpawnTurtles>();
        turtlePrefab = SpawnerParent.GetTurtlePrefab();
        TurtleSizeScale = SpawnerParent.TurtleSizeScale;
        StartCoroutine(IntSandSFX());
        StartCoroutine(spawnTurtles());
    }

    IEnumerator IntSandSFX()
    {
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<AudioManager>().Play("SandEmerge2", true);
        yield return null;
    }

    IEnumerator spawnTurtles()
    {
        yield return new WaitForSeconds(1.5f);
        FindObjectOfType<AudioManager>().Play("EggCrack", true);
        GameObject newTurtle = Instantiate(turtlePrefab, transform);
        newTurtle.transform.localScale = new Vector3(TurtleSizeScale, TurtleSizeScale, TurtleSizeScale);
        SpawnerParent.AddTurtle(newTurtle);    
        yield return new WaitForSeconds(masterFlow.GetTurtleSpawnRate());
        StartCoroutine(spawnTurtles());     
    }


}
