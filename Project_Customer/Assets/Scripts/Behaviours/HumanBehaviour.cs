using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBehaviour : MonoBehaviour
{
    Terrain playArea;
    GameObject targetObj;
    public GameObject trashPrefab;
    public int TargetDistanceFromBorder;
    private GameObject trashParent;

    private void Start()
    {
        playArea = Terrain.activeTerrain;
        trashParent = GameObject.FindGameObjectWithTag("DraggableParent");
        SpawnTarget();
        StartCoroutine(spawnTrash());
    }   

    void SpawnTarget()
    {
        //if human is to the left, spawn to the right
        if (transform.position.x < playArea.transform.position.x + playArea.terrainData.size.x / 2)
        {
            targetObj = new GameObject(name: "HumanTarget");
            targetObj.transform.position = new Vector3(playArea.terrainData.size.x - TargetDistanceFromBorder, transform.position.y, transform.position.z);
            targetObj.transform.parent = GameObject.FindGameObjectWithTag("HumanTargets").transform;
        }

        //if human is to the right, spawn to the left
        if (transform.position.x > playArea.transform.position.x + playArea.terrainData.size.x / 2)
        {
            targetObj = new GameObject(name: "HumanTarget");
            targetObj.transform.position = new Vector3(playArea.transform.position.x + TargetDistanceFromBorder, transform.position.y, transform.position.z);
            targetObj.transform.parent = GameObject.FindGameObjectWithTag("HumanTargets").transform;
        }

        GetComponent<Mover>().SetTarget(targetObj.transform);
    }

    private GameObject randomTrash()
    {
        Transform toGameobject = trashPrefab.transform.GetChild(Random.Range(0, trashPrefab.transform.childCount));
        return toGameobject.gameObject;
    }

    IEnumerator spawnTrash()
    {
        GameObject newTrash = Instantiate(randomTrash(), gameObject.transform);
        newTrash.transform.parent = trashParent.transform;
        newTrash.transform.localScale *= 0.3f;
        yield return new WaitForSeconds(Random.Range(1, 5));
        StartCoroutine(spawnTrash());
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, targetObj.transform.position) < 5)
        {
            Destroy(targetObj);
            Destroy(gameObject);
        }
    }
}
