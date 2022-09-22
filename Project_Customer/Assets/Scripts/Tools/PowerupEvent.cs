using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupEvent : MonoBehaviour
{
    Terrain playArea;
    [SerializeField] GameObject ScareCrowPrefab;
    [SerializeField] MasterFlow MasterFlow;
    [SerializeField] SpawnTurtles Turtles;
    [SerializeField] SpawnSeagulls Seagulls;
    [SerializeField] int trashThreshold;
    private GameObject PowerupChildren;
    private GameObject activeScareCrow;


    [SerializeField] float ScareCrowLifeTime;


    private void Start()
    {
        PowerupChildren = new GameObject(name: "PowerupChildren");
        PowerupChildren.transform.SetParent(gameObject.transform);
        playArea = Terrain.activeTerrain;
    }

    public int ReturnTrashThreshold()
    {
        return trashThreshold;
    }

    public void StartRandomEvent()
    {
        StartCoroutine(RandomEvent());
        return;
    }

    public IEnumerator RandomEvent()
    {
        switch(Random.Range(0, 2))
        {
            case 0:
                if (!MasterFlow.spawnSeagulls.enabled)
                {
                    GhostTurtles();
                }
                else
                {
                    SpawnScareCrow();
                }
                   
                break;

            case 1:
                GhostTurtles();
                break;

        }
        yield return null;
    }

    private void SpawnScareCrow()
    {
        GameObject scareCrow = Instantiate(ScareCrowPrefab, position: new Vector3(50, -0.28f, 20), rotation: Quaternion.Euler(Vector3.zero));
        scareCrow.transform.parent = PowerupChildren.transform;
        Seagulls.ReppelAllSeagulls();
        MasterFlow.ActivateScareCrow();
        activeScareCrow = scareCrow;
        StartCoroutine(DespawnScareCrow(scareCrow));
    }

    private void GhostTurtles()
    {
        foreach(var turtle in Turtles.GetTurtles())
        {
            turtle.GetComponent<TurtleBehaviour>().ToggleGhost(true);
        }
    }

    public IEnumerator DespawnScareCrow(GameObject scareCrow)
    {
        yield return new WaitForSeconds(ScareCrowLifeTime);
        MasterFlow.DeactivateScareCrow();
        scareCrow.GetComponent<Animator>().SetTrigger("LowerScareCrow");
        yield return new WaitForSeconds(1f);
        Destroy(scareCrow);
       
    }



}
