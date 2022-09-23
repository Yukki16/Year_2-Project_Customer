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
        if (!EnableSpawning)
        {
            yield return new WaitForSeconds(3);
            StartCoroutine(spawnSeagulls());
        }

        GameObject newSeagull = Instantiate(SeagullPrefab);
        seagullList.Add(newSeagull);
        newSeagull.transform.parent = Seagulls.transform;
        yield return new WaitForSeconds(masterFlow.GetSeagullSpawnRate());
        if (EnableSpawning)
        {
            GameObject newSeagull = Instantiate(SeagullPrefab);
            seagullList.Add(newSeagull);
            if(masterFlow.tutorial.seagul == null)
            {
                masterFlow.tutorial.seagul = newSeagull;
                StartCoroutine(masterFlow.tutorial.SeagulTutorial(masterFlow.tutorial.seagul));
            }
            newSeagull.transform.parent = Seagulls.transform;           
        }      
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
        if (SpawnSeagull)
        {
            GameObject newSeagull = Instantiate(SeagullPrefab);
            newSeagull.transform.parent = Seagulls.transform;
            seagullList.Add(newSeagull);
            SpawnSeagull = false;
        }        
    }

}
