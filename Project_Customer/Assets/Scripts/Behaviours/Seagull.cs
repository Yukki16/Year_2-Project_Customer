using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagull : MonoBehaviour
{
    #region fields
    public enum SeagullState { Patrol, Detect, Pursue, Pickup, Exit };

    public SeagullState currentState;

    public Vector3 startingVector; 
    private Vector3 lastPosition;

    private float timeCounter;
    private float targetTimer;
    private float widthCopy;
    private float targetYModifier;
    private float detectDuration = 0.867f;
    private float detectionTimer;

    public float TimeBeforeTarget;
    public float DistanceToPickUp;
    public float speed = 1.4f;
    public float width = 8;
    public float startingDistance = 50;

    public int PickupSpeed = 1;

    private bool foundTurtle;
    private bool hasShrank;
    private bool TurtleTaken;

    private Transform visualTransfom;
    private Transform forward;
    private Terrain playArea;
    private GameObject[] turtles;
    private GameObject exitTarget;
    private GameObject closestTurtle;
    private Animator animator;
    #endregion

    void Start()
    {
        playArea = Terrain.activeTerrain;

        widthCopy = width;

        width = startingDistance;

        visualTransfom = gameObject.transform.GetChild(1);

        animator = visualTransfom.GetComponent<Animator>();

        if (startingVector == Vector3.zero)
        {
            startingVector = transform.position;
        }
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

        currentState = SeagullState.Detect;
    }

    private void CarryAway()
    {
        currentState = SeagullState.Exit;

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
        visualTransfom.LookAt(closestTurtle.transform);

        transform.position = Vector3.MoveTowards(transform.position, closestTurtle.transform.position, 0.01f * PickupSpeed);

        if (Vector3.Distance(transform.position, closestTurtle.transform.position) < DistanceToPickUp)
        {
            animator.SetTrigger("Pickup");
            currentState = SeagullState.Exit;
        }
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

    private void Detection()
    {
        visualTransfom.LookAt(closestTurtle.transform);
        animator.SetTrigger("Detect");
        detectionTimer += Time.deltaTime;
        if (detectionTimer >= detectDuration)
        {
            animator.SetTrigger("Pursue");
            currentState = SeagullState.Pursue;
        }

    }

    void Update()
    {

        if (closestTurtle == null)
        {
            currentState = SeagullState.Patrol;
        }

        switch (currentState)
        {
            case SeagullState.Exit:
                CarryAway();
                break;

            case SeagullState.Patrol:
                CircleLoop();
                break;

            case SeagullState.Pursue:
                MoveToTurtle();
                break;

            case SeagullState.Detect:
                Detection();
                break;


        }

        DetectAreaExit();
        TargetTurtle();
        ShrinkRadius();
    }


}
