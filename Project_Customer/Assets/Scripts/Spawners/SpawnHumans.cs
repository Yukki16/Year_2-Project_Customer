using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHumans : MonoBehaviour
{
    [SerializeField] GameObject HumanPrefab;
    private List<Transform> spawnPoints;

    private void Start()
    {
        foreach (Transform child in this.gameObject.transform)
        {
            spawnPoints.Add(child);
        }

        StartCoroutine(spawnHumans());
    }

    IEnumerator spawnHumans()
    {
        GameObject newTurtle = Instantiate(HumanPrefab, spawnPoints[Random.Range(0, spawnPoints.Count - 1)].transform);
        newTurtle.transform.parent = null;
        yield return new WaitForSeconds(Random.Range(5, 10));
        StartCoroutine(spawnHumans());
    }
    void Update()
    {

    }
}
