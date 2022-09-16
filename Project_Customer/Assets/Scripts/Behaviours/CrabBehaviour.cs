using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabBehaviour : MonoBehaviour
{
    #region Fields
    private enum CrabState
    {
        Moving,
        Stationary,
        CapturedTurtle
    }

    private CrabState currentState = CrabState.Stationary;
    private Vector3 targetPosition;
    private GameObject targetObj;
    private GameObject crabTargetsParent;

    private Terrain playArea;

    private float offsetX;
    private float offsetZ;

    Mover mover;

    private float minXMovement = 0.5f;
    private float maxXMovement = 4f;
    private float minZMovement = 0.2f;
    private float maxZMovement = 2f;

    private int direction = 1;

    #endregion

    void Start()
    {
        targetObj = new GameObject();
        crabTargetsParent = GameObject.FindGameObjectWithTag("CrabTargets");
        mover = GetComponent<Mover>();
        playArea = Terrain.activeTerrain;
        SpawnTarget();
        StartCoroutine(CrabMovement());
    }

    void SetTargetObjPosition(Vector3 position)
    {
        targetObj.transform.position = position;
        mover.SetTarget(targetObj.transform);
    }

    void SpawnTarget()
    {
        targetObj.transform.SetParent(crabTargetsParent.transform);
        targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        targetObj.transform.position = targetPosition;
        mover.SetTarget(targetObj.transform);
    }
    //Particle effects 
    private void OnTriggerEnter (Collider collision)
    {
        if (collision.gameObject.tag is "Turtle")
        {
            Destroy(gameObject); 
            Destroy(targetObj);
            collision.gameObject.GetComponent<TurtleBehaviour>().DestroyTurtle();
        } 

        if (collision.gameObject.tag is "Draggable")
        {
            Destroy(collision.gameObject);
            Destroy(targetObj);
            Destroy(gameObject);
        }

    }

    IEnumerator CrabMovement()
    {
        switch (direction)
        {
            case 1:
                offsetX = Random.Range(minXMovement, maxXMovement);
                offsetZ = Random.Range(minZMovement, maxZMovement);
                direction = 2;
                break;

            case 2:
                offsetX = Random.Range(-minXMovement, -maxXMovement);
                offsetZ = Random.Range(-minZMovement, -maxZMovement);
                direction = 1;
                break;
        }

        targetPosition.x += offsetX;
        targetPosition.z += offsetZ;

        SetTargetObjPosition(targetPosition);

        yield return new WaitForSeconds(2);
        StartCoroutine(CrabMovement());
    }

}
