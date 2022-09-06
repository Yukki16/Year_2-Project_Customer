using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTurtles : MonoBehaviour
{
    [SerializeField]
    private List<Transform> spawnPoints;
    [SerializeField]
    private GameObject turtlePrefab;


    private void Start()
    {
        foreach(Transform child in this.gameObject.transform)
        {
            spawnPoints.Add(child);
        }
        StartCoroutine(spawnTurtles());
    }

    IEnumerator spawnTurtles()
    {
        GameObject newTurtle = Instantiate(turtlePrefab, spawnPoints[Random.Range(0, spawnPoints.Count - 1)].transform);
        newTurtle.transform.parent = null;
        yield return new WaitForSeconds(Random.Range(5, 10));
        StartCoroutine(spawnTurtles());
    }
    void Update()
    {
          
    }
}
