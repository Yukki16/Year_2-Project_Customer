using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleBehaviour : MonoBehaviour
{
    private Terrain playArea;

    private float offset;

    Mover mover;

    Vector3 targetObjPosition;

    private GameObject turtleSpawnerParent;

    GameObject targetObj;

    private int direction = 1;

    public int MinWobbleDistance = 5;
    public int MaxWobbleDistance = 10;
    public int WobbleSwitchTimer;

    void Start()
    {
        targetObj = new GameObject();
        turtleSpawnerParent = GameObject.FindGameObjectWithTag("TurtleTargets");
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
        targetObjPosition = new Vector3(transform.position.x, transform.position.y, playArea.terrainData.size.z);
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
        if (Vector3.Distance(transform.position, targetObj.transform.position) < 5)
        {
            DestroyTurtle();
        }
    }

    public void DestroyTurtle()
    {
        Destroy(targetObj);
        Destroy(gameObject);
    }

    IEnumerator WiggleTarget()
    {
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
