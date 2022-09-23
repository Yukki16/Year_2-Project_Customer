using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PowerupEvent : MonoBehaviour
{
    Terrain playArea;
    [SerializeField] GameObject ScareCrowPrefab;
    [SerializeField] MasterFlow MasterFlow;
    [SerializeField] SpawnTurtles Turtles;
    [SerializeField] SpawnSeagulls Seagulls;
    [SerializeField] int trashThreshold;
    private GameObject PowerupChildren;
    GameObject scareCrow;

    [SerializeField] float ScareCrowLifeTime;
    private bool scareCrowActive;


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
        switch(Random.Range(0, 2))
        {
            case 0:
                if (MasterFlow.spawnSeagulls.enabled && !scareCrowActive)
                {
                    SpawnScareCrow();
                }
                else
                {
                    GhostTurtles();
                }
                   
                break;

            case 1:
                GhostTurtles();
                break;

        }

        yield return new WaitForFixedUpdate();
    }
   private void GhostTurtles()
    {
        foreach(var turtle in Turtles.GetTurtles())
        {
            if (turtle == null) return; 
            turtle.GetComponent<TurtleBehaviour>().ToggleInvincible(true);
        }
    }

    private void SpawnScareCrow()
    {
        GameObject scareCrow = Instantiate(ScareCrowPrefab, position: new Vector3(50, -0.28f, 20), rotation: Quaternion.Euler(Vector3.zero));
        scareCrow.transform.parent = PowerupChildren.transform;
        if(!MasterFlow.tutorial.scarecowTutorial)
        {
            MasterFlow.tutorial.powerup = scareCrow;
            StartCoroutine(MasterFlow.tutorial.ScareCrowTutorial(MasterFlow.tutorial.powerup));
        }
        Seagulls.ReppelAllSeagulls();
        MasterFlow.ActivateScareCrow();
        activeScareCrow = scareCrow;
        StartCoroutine(DespawnScareCrow(scareCrow));
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
            turtle.GetComponent<TurtleBehaviour>().ToggleGhost(true);
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

 
    public IEnumerator DespawnScareCrow()
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
