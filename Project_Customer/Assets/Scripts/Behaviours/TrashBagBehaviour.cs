using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBagBehaviour : MonoBehaviour
{
    public GameObject trashPrefab;
    private GameObject trashParent;
    private TrashBehaviour tb;

    private void Start()
    {
        tb = GetComponent<TrashBehaviour>();
        trashParent = GameObject.FindGameObjectWithTag("DraggableParent");
        StartCoroutine(SpawnTrash());
    }

    private GameObject randomTrash()
    {
        Transform toGameobject = trashPrefab.transform.GetChild(Random.Range(0, trashPrefab.transform.childCount));
        return toGameobject.gameObject;
    }

    IEnumerator SpawnTrash()
    {
        if (tb.GetIsActive())
        {
            GameObject newTrash = Instantiate(randomTrash(), gameObject.transform);
            newTrash.transform.parent = trashParent.transform;
        }
        yield return new WaitForSeconds(Random.Range(0.3f, 1));
        StartCoroutine(SpawnTrash());
    }

}
