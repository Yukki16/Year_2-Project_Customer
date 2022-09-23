using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSeagulls : MonoBehaviour
{
    [SerializeField] GameObject SeagullPrefab;
    private List<GameObject> seagullList;
    GameObject Seagulls;
    MasterFlow masterFlow;
    public bool EnableSpawning;
    public bool SpawnSeagull;
    private bool crRunning;

    void Start()
    {
        masterFlow = GameObject.FindObjectOfType<MasterFlow>();

        seagullList = new List<GameObject>();

        Seagulls = new GameObject(name: "Seagulls");
        Seagulls.transform.SetParent(gameObject.transform);

        StartCoroutine(spawnSeagulls());

        StartCoroutine(pingSeagulls());

    }

    public void ReppelAllSeagulls()
    {
        foreach (var seagull in seagullList)
        {
            if (seagull == null) return;
            seagull.GetComponent<Seagull>().EarlyExit();
        }
    }

    IEnumerator spawnSeagulls()
    {           
        GameObject newSeagull = Instantiate(SeagullPrefab);
        seagullList.Add(newSeagull);
        newSeagull.transform.parent = Seagulls.transform;
        yield return new WaitForSeconds(masterFlow.GetSeagullSpawnRate());
        StartCoroutine(spawnSeagulls());    
    }

    public List<GameObject> GetSeagulls()
    {
        return seagullList;
    }

    IEnumerator pingSeagulls()
    {
        //Debug.Log(seagullList.Count);
        yield return new WaitForSeconds(1);
        StartCoroutine(pingSeagulls());
    }

    private void Update()
    {
        if (!EnableSpawning)
        {
            crRunning = false;
            StopCoroutine(spawnSeagulls());
        }

        if (crRunning && EnableSpawning)
        {
            StartCoroutine(spawnSeagulls());
        }

        if (SpawnSeagull)
        {
            GameObject newSeagull = Instantiate(SeagullPrefab);
            newSeagull.transform.parent = Seagulls.transform;
            seagullList.Add(newSeagull);
            SpawnSeagull = false;
        }        
    }

}
