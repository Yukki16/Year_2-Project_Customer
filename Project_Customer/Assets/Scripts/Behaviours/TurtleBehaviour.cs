using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurtleBehaviour : MonoBehaviour
{
    private Terrain playArea;

    private float offset;

    Mover mover;

    Vector3 targetObjPosition;

    private GameObject turtleSpawnerParent;

    GameObject targetObj;

    public Animator animator;

    private int direction = 1;

    int EndDistanceFromTop = 15;


    public int MinWobbleDistance = 5;
    public int MaxWobbleDistance = 10;
    public int WobbleSwitchTimer;

    Outline outline;
    private bool excecution;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        targetObj = new GameObject();
        turtleSpawnerParent = GameObject.FindGameObjectWithTag("TurtleTargets");
        outline = GetComponent<Outline>();
        mover = GetComponent<Mover>();
        playArea = Terrain.activeTerrain;
        StartCoroutine(WiggleTarget());
        SpawnTarget();
    }

    void SetTargetObjPosition(Vector3 position)
    {
        targetObj.transform.position = position;
    }

    void SpawnTarget()
    {
        targetObj.transform.SetParent(turtleSpawnerParent.transform);
        targetObjPosition = new Vector3(transform.position.x, transform.position.y, playArea.terrainData.size.z - EndDistanceFromTop);
        targetObj.transform.position = targetObjPosition;
        mover.SetTarget(targetObj.transform);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag is "Draggable")
        {
            Destroy(gameObject);
        }
    }


    void DestroyOnTarget()
    {
        if (Vector3.Distance(transform.position, targetObj.transform.position) < 1 && excecution)
        {
            DisableTurtle();
        }

        if (Vector3.Distance(transform.position, targetObj.transform.position) < 1 && !excecution)
        {
            DestroyTurtle();
        }
    }

    public void RemoveFromList()
    {
        GameObject.FindGameObjectWithTag("TurtleSpawner").GetComponent<SpawnTurtles>().GetTurtles().Remove(gameObject);
    }

    public void DestroyTurtle()
    {
        RemoveFromList();
        Destroy(targetObj);
        Destroy(gameObject);
    }

    public void EnableOutline()
    {
        outline.OutlineWidth = 2;
    }

    public void DisableOutline()
    {
        outline.OutlineWidth = 0;
    }

    public void DisableTurtle()
    {
        mover.Cancel();
    }

    //TODO
    public IEnumerator MoveTowards(Vector3 position)
    {
        excecution = true;
        GetComponent<NavMeshAgent>().speed = 5;
        targetObj.transform.position = position;
        yield return null;
    }

    IEnumerator WiggleTarget()
    {
        if (excecution)
             yield return null ;

        switch (direction)
        {
            case 1:
                offset = Random.Range(MinWobbleDistance, MaxWobbleDistance);
                direction = 2;
                break;

            case 2:               
                offset = Random.Range(-MinWobbleDistance, -MaxWobbleDistance);
                direction = 1;
                break;

        }

        targetObjPosition.x += offset;

        SetTargetObjPosition(targetObjPosition);

        yield return new WaitForSeconds(WobbleSwitchTimer);

        StartCoroutine(WiggleTarget());
    }


    void Update()
    {
        DestroyOnTarget();
    }


}
