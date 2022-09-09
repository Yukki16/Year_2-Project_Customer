using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleBehaviour : MonoBehaviour
{
    Rigidbody objectRigidBody = null;

    private Terrain playArea;

    [SerializeField] private float forwardSpeed = 1;

    [SerializeField] private float sidewaySpeed = 100;

    private Vector3 directionVector = new Vector3(0,0,0);

    private float directionReduction = 0.98f;

    private float offset;

    Mover mover;

    Vector3 targetObjPosition;

    private GameObject turtleSpawnerParent;

    GameObject targetObj;

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

    void DestroyOnTarget()
    {
        if (Vector3.Distance(transform.position, targetObj.transform.position) < 5)
        {
            Destroy(targetObj);
            Destroy(gameObject);
        }
    }

    IEnumerator WiggleTarget()
    {
        int direction = Random.Range(0, 3);
        switch (direction)
        {
            case 1:
                offset = Random.Range(3, 6);
                break;

            case 2:
                offset = Random.Range(-3, -6);
                break;

        }

        targetObjPosition.x += offset;

        SetTargetObjPosition(targetObjPosition);

        yield return new WaitForSeconds(Random.Range(2, 4));

        StartCoroutine(WiggleTarget());
    }


    void Update()
    {
        DestroyOnTarget();
    }


}
