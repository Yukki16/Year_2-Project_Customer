using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurtleBehaviour : MonoBehaviour
{
    //The terrain area
    private Terrain playArea;

    private float offset;
    //script to help with the AI in NavMesh
    Mover mover;

    Vector3 targetObjPosition;
    //GameObject for when the turtle's target spawns, it becomes it's parent;
    private GameObject turtleSpawnerParent;
    //The empty target who is a child of turtleSpawnerParent;
    //it helps with organization
    GameObject targetObj;

    private GameObject seagulSpawner;

    void Start()
    {
        //creates the empty object as a target
        targetObj = new GameObject();
        
        turtleSpawnerParent = GameObject.FindGameObjectWithTag("TurtleTargets");
        seagulSpawner = GameObject.FindGameObjectWithTag("SeagulSpawner");
        mover = GetComponent<Mover>();

        playArea = Terrain.activeTerrain;

        StartCoroutine(WiggleTarget());

        SpawnTarget();
    }

    void SetTargetObjPosition(Vector3 position)
    {
        targetObj.transform.position = position;
    }
    /// <summary>
    /// Moves the target under a new parent for organization. The target is finally set for the turtle.
    /// </summary>
    void SpawnTarget()
    {
        targetObj.transform.SetParent(turtleSpawnerParent.transform);
        //Moves target to the end of the terrain(Z axis) where is the water, where x and y are the turtle position
        targetObjPosition = new Vector3(transform.position.x, transform.position.y, playArea.terrainData.size.z);
        targetObj.transform.position = targetObjPosition;
        mover.SetTarget(targetObj.transform);
    }
    /// <summary>
    /// Destroys turtle in contact with trash
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "draggable")
        {
            this.gameObject.GetComponent<NavMeshAgent>().acceleration /= 2;
            seagulSpawner.GetComponent<SpawnSeaguls>().SpawnSeagul(this.gameObject);
            //Destroy(gameObject);
        }
    }
    /// <summary>
    /// Arrived on destination, both the target and the turtle are destroyed
    /// </summary>
    void DestroyOnTarget()
    {
        if (Vector3.Distance(transform.position, targetObj.transform.position) < 5)
        {
            Destroy(targetObj);
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Moves the target of the turtle so it gives a "left-right" movement effect to the turtle.
    /// </summary>
    /// <returns></returns>
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
