using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagull : MonoBehaviour
{
    #region fields
    public enum SeagulState { Exit, Pickup, Patrol};

    public SeagulState currentState;

    float timeCounter;
    public float speed = 1.4f;
    public float width = 8;
    public float startingDistance = 50;
    private float widthCopy;
    private bool hasShrank;
    public Vector3 startingVector; 
    public int PickupSpeed = 1;
    public float TimeBeforeTarget;
    public float DistanceToPickUp;
    private bool TurtleTaken;
    private Vector3 lastPosition;

    private Transform visualTransfom;
    private Transform forward;

    private float targetTimer;
    private bool foundTurtle;
    private Terrain playArea;
    private GameObject[] turtles;

    GameObject exitTarget;
    GameObject closestTurtle;
    private float targetYModifier;
    #endregion
    void Start()
    {
        playArea = Terrain.activeTerrain;

        SeagulState currentState = SeagulState.Patrol;

        widthCopy = width;

        width = startingDistance;

        visualTransfom = gameObject.transform.GetChild(1);

        if (startingVector == Vector3.zero)
        {
            startingVector = transform.position;
        }
    }

    private void RotateVisual(float x = 0, float y = 0, float z = 0, float w = 0)
    {
        visualTransfom.transform.rotation = new Quaternion(-z, y, x, w);
    }

    private void CircularMotion()
    {
        timeCounter += Time.deltaTime * speed;

        float x = startingVector.x + Mathf.Cos(timeCounter) * width;
        float y = startingVector.y;
        float z = startingVector.z + Mathf.Sin(timeCounter) * width;

        transform.position = new Vector3(x, y, z);
    }

    private void ShrinkRadius()
    {
        if (width <= widthCopy)
        {
            hasShrank = true;
        }

        if (!hasShrank)
        width -= 0.02f;
    }

    private void RotateForward()
    {
        transform.LookAt(startingVector);
    }

    private void CircleLoop()
    {
        RotateForward();
        CircularMotion();
    }

    private void TargetTurtle()
    {
        if (!hasShrank) return;
        if (foundTurtle) return;

        if(targetTimer <= TimeBeforeTarget)
        {
            targetTimer += Time.deltaTime;
            return;
        }

        if (turtles == null)
        {
            turtles = GameObject.FindGameObjectsWithTag("Turtle");
        }

        foreach (var turtle in turtles)
        {
            closestTurtle = turtles[0]; 

            if (Vector3.Distance(transform.position, closestTurtle.transform.position) < Vector3.Distance(transform.position, turtle.transform.position))
            {
                closestTurtle = turtle;               
            }
        }

        closestTurtle.gameObject.GetComponent<TurtleBehaviour>().EnableOutline();
        foundTurtle = true;
    }

    private void CarryAway()
    {
        currentState = SeagulState.Exit;

        if (exitTarget == null)
        {
            exitTarget = new GameObject();
            exitTarget.transform.position = new Vector3(transform.position.x * 5, transform.position.y, transform.position.z *-5);
        }
        else
        {
            TurtleTaken = true;
            closestTurtle.GetComponent<TurtleBehaviour>().DisableOutline();
            closestTurtle.GetComponent<TurtleBehaviour>().DisableTurtle();
            closestTurtle.transform.position = transform.position;
            exitTarget.transform.position = new Vector3(exitTarget.transform.position.x, targetYModifier, exitTarget.transform.position.z);;
            visualTransfom.LookAt(exitTarget.transform);
            transform.position = Vector3.MoveTowards(transform.position, exitTarget.transform.position, 0.1f * speed / 2);
            if (targetYModifier < 500)
            targetYModifier += 0.5f;
        }
    }

    private void MoveToTurtle()
    {
        if (closestTurtle == null)
        {
            CircleLoop();
        }

        else
        {
            if (Vector3.Distance(transform.position, closestTurtle.transform.position) < DistanceToPickUp)
            {
                currentState = SeagulState.Exit;
            }
            else
            {
                if (currentState != SeagulState.Exit)
                currentState = SeagulState.Pickup;
            }
        }
    }

    private void PickupTurtle()
    {
        visualTransfom.LookAt(closestTurtle.transform);
        transform.position = Vector3.MoveTowards(transform.position, closestTurtle.transform.position, 0.01f * PickupSpeed);
    }

    private void DetectAreaExit()
    {
        if (transform.position.x >= playArea.terrainData.size.x || transform.position.x <= playArea.transform.position.x)
        {
            DestroySeagull();
        }
    }

    private void DestroySeagull()
    {
        if (TurtleTaken)
        {
            closestTurtle.GetComponent<TurtleBehaviour>().DestroyTurtle();
        }
        Object.Destroy(gameObject);
    }

    void Update()
    {
        switch (currentState)
        {
            case SeagulState.Exit:
                CarryAway();
            break;

            case SeagulState.Pickup:
                PickupTurtle();
                break;
        }

        DetectAreaExit();
        TargetTurtle();
        ShrinkRadius();
        MoveToTurtle();       
    }


}
