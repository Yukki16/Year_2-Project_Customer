using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PowerupEvent : MonoBehaviour
{
    Terrain playArea;
    [SerializeField] GameObject TruckPrefab;
    [SerializeField] GameObject ScareCrowPrefab;
    [SerializeField] MasterFlow MasterFlow;
    [SerializeField] SpawnTurtles Turtles;
    [SerializeField] SpawnSeagulls Seagulls;
    [SerializeField] int trashThreshold;
    private GameObject PowerupChildren;
    GameObject scareCrow;
    GameObject iceCreamTruck;

    [SerializeField] float ScareCrowLifeTime;
    [SerializeField] float IceCreamLifeTime;
    private bool scareCrowActive;
    private bool iceCreamTruckActive;


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
        yield return new WaitForSeconds(1);
        switch(Random.Range(0, 3))
        {
            case 0:
                if (MasterFlow.spawnSeagulls.enabled && !scareCrowActive)
                {
                    SpawnScareCrow();
                }
                else
                {
                    StartCoroutine(RandomEvent());
                    yield return null;
                }
                   
                break;

            case 1:
                GhostTurtles();
                break;

            case 2:
                SpawnIceCream();
                break;

        }

        yield return new WaitForFixedUpdate();
    }

    private void SpawnIceCream()
    {
        iceCreamTruckActive = true;

        if (iceCreamTruck == null)
        {
            iceCreamTruck = Instantiate(TruckPrefab, position: new Vector3(25, 3.54f, 32.3f), rotation: Quaternion.Euler(0, 90, 0));
            iceCreamTruck.transform.parent = PowerupChildren.transform;
        }
        else
        {
            iceCreamTruck.GetComponentInChildren<Animator>().SetTrigger("PullUp");
        }

        MasterFlow.ActivateIceCream();

        StartCoroutine(DespawnTruck());

    }

    private IEnumerator DespawnTruck()
    {
        yield return new WaitForSeconds(IceCreamLifeTime);
        //scareCrow.GetComponent<NavMeshObstacle>().enabled = false;
        MasterFlow.DeactivateIceCream();
        if (iceCreamTruck != null)
            iceCreamTruck.GetComponentInChildren<Animator>().SetTrigger("PullOut");
        iceCreamTruckActive = false;
        yield return null;       
    }


    private void SpawnScareCrow()
    {
        scareCrowActive = true;

        if (scareCrow == null)
        {
            scareCrow = Instantiate(ScareCrowPrefab, position: new Vector3(50, -0.28f, 20), rotation: Quaternion.Euler(Vector3.zero));
            if (!MasterFlow.tutorial.scarecowTutorial)
            {
                MasterFlow.tutorial.powerup = scareCrow;
                StartCoroutine(MasterFlow.tutorial.ScareCrowTutorial(MasterFlow.tutorial.powerup));
            }
            scareCrow.transform.parent = PowerupChildren.transform;
        }
        else
        {
            scareCrow.GetComponent<Animator>().SetTrigger("RaiseScareCrow");
        }
        scareCrow.GetComponent<NavMeshObstacle>().enabled = enabled;
        Seagulls.ReppelAllSeagulls();
        MasterFlow.ActivateScareCrow();
        StartCoroutine(DespawnScareCrow());
    }

    private void GhostTurtles()
    {
        foreach (var turtle in Turtles.GetTurtles())
        {
            if (!MasterFlow.tutorial.shieldTutorial)
            {
                MasterFlow.tutorial.powerup = turtle;
                StartCoroutine(MasterFlow.tutorial.ShildedTutorial(MasterFlow.tutorial.powerup));
            }
            turtle.GetComponent<TurtleBehaviour>().ToggleInvincible(true);
        }
    }

 
    private IEnumerator DespawnScareCrow()
    {
        yield return new WaitForSeconds(ScareCrowLifeTime);
        scareCrow.GetComponent<NavMeshObstacle>().enabled = false;
        MasterFlow.DeactivateScareCrow();
        if (scareCrow != null)
            scareCrow.GetComponent<Animator>().SetTrigger("LowerScareCrow");
        scareCrowActive = false;
        yield return null;
    }



}
