using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBehaviour : MonoBehaviour
{
    Terrain playArea;
    GameObject targetObj;
    [SerializeField] GameObject trashPrefab;
    private GameObject trashParent;

    private void Start()
    {
        playArea = Terrain.activeTerrain;
        trashParent = GameObject.FindGameObjectWithTag("TrashParent");
        SpawnTarget();
        StartCoroutine(spawnTrash());
    }

    void SpawnTarget()
    {
        //if target is to the left 
        if (transform.position.x < playArea.transform.position.x + playArea.terrainData.size.x / 2)
        {
            targetObj = new GameObject();
            targetObj.transform.position = new Vector3(playArea.terrainData.size.x, transform.position.y, transform.position.z);
        }

        //if target is to the right 
        if (transform.position.x > playArea.transform.position.x + playArea.terrainData.size.x / 2)
        {
            targetObj = new GameObject();
            targetObj.transform.position = new Vector3(playArea.transform.position.x, transform.position.y, transform.position.z);
        }

        GetComponent<Mover>().SetTarget(targetObj.transform);
    }

    IEnumerator spawnTrash()
    {
        GameObject newTrash = Instantiate(trashPrefab, gameObject.transform);
        newTrash.transform.parent = trashParent.transform;
        newTrash.transform.localScale = new Vector3(1, 1, 1);
        yield return new WaitForSeconds(Random.Range(1, 5));
        StartCoroutine(spawnTrash());
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, targetObj.transform.position) < 5)
        {
            Destroy(gameObject);
        }
    }
}
