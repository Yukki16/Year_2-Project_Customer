using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupEvent : MonoBehaviour
{
    Terrain playArea;
    [SerializeField] GameObject ScareCrowPrefab;
    [SerializeField] MasterFlow MasterFlow;
    [SerializeField] SpawnTurtles Turtles;
    private GameObject PowerupChildren;
    private GameObject activeScareCrow;

    [SerializeField] float ScareCrowLifeTime;


    private void Start()
    {
        PowerupChildren = new GameObject(name: "PowerupChildren");
        PowerupChildren.transform.SetParent(gameObject.transform);
        playArea = Terrain.activeTerrain;
    }

    public void StartRandomEvent()
    {
        StartCoroutine(RandomEvent());
        return;
    }

    public IEnumerator RandomEvent()
    {
        GhostTurtles();
        yield return null;
    }

    private void SpawnScareCrow()
    {
        GameObject scareCrow = Instantiate(ScareCrowPrefab, position: new Vector3(50, 0, 20), rotation: Quaternion.Euler(Vector3.zero));
        scareCrow.transform.parent = PowerupChildren.transform;
        MasterFlow.ActivateScareCrow();
        StartCoroutine(DespawnScareCrow());
        activeScareCrow = scareCrow;
    }

    private void GhostTurtles()
    {
        foreach(var turtle in Turtles.GetTurtles())
        {
            turtle.GetComponent<TurtleBehaviour>().ToggleGhost(true);
        }
    }

    public IEnumerator DespawnScareCrow()
    {
        yield return new WaitForSeconds(ScareCrowLifeTime);
        Destroy(activeScareCrow);
        MasterFlow.DeactivateScareCrow();
    }



}
