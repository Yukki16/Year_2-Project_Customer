using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBehaviour : MonoBehaviour
{
    [SerializeField] GameObject trashPrefab;
    private GameObject trashParent;

    private void Start()
    {
        trashParent = GameObject.FindGameObjectWithTag("TrashParent");
        StartCoroutine(spawnTrash());
    }

    IEnumerator spawnTrash()
    {
        GameObject newTrash = Instantiate(trashPrefab, gameObject.transform);
        newTrash.transform.parent = trashParent.transform;
        newTrash.transform.localScale = new Vector3(1, 1, 1);
        yield return new WaitForSeconds(Random.Range(1, 2));
        StartCoroutine(spawnTrash());
    }
}
