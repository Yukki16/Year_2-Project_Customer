using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Seagull : MonoBehaviour
{
    #region fields
    private enum SeagullState { Patrol, Detect, Pursue, Pickup, TurtleExit, TrashExit };

    private SeagullState currentState;

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

    public bool ClosestTurtle;
    public bool RandomTurtle;

    private bool foundTurtle;
    private bool hasShrank;
    private bool TurtleTaken;

    private Transform visualTransfom;
    private Terrain playArea;
    private GameObject[] turtles;
    private GameObject trashTarget;
    private GameObject targetTurtle;
    private Animator animator;
    private List<GameObject> turtleList;
    
    #endregion

    void Start()
    {
        turtleList = GameObject.FindGameObjectWithTag("TurtleSpawner").GetComponent<SpawnTurtles>().GetTurtles();

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

    void Update()
    {
        if (targetTurtle == null)
        {
            currentState = SeagullState.Patrol;
        }

        if (ClosestTurtle && RandomTurtle)
        {
            ClosestTurtle = false;
        }

        switch (currentState)
        {
            case SeagullState.TurtleExit:
                CarryAway();
                break;

            case SeagullState.TrashExit:
                TrashCancel();
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

        DetectTurtleLoss();
        DetectAreaExit();
        TargetTurtle();
        ShrinkRadius();
    }

    #region methods

    //Checks whether the seagull is eligible and detects a draggable object's collider
    void OnCollisionEnter(Collision collision)
    {
        if (currentState != SeagullState.Pursue)
            return;

        if (collision.gameObject.tag is "Draggable")
        {
            trashTarget = collision.gameObject;
            currentState = SeagullState.TrashExit;
        }
    }

    //Rotates the seagull in a circular motion around a specific area
    private void CircularMotion()
    {
        timeCounter += Time.deltaTime * speed;

        float x = startingVector.x + Mathf.Cos(timeCounter) * width;
        float y = startingVector.y;
        float z = startingVector.z + Mathf.Sin(timeCounter) * width;

        transform.position = new Vector3(x, y, z);
    }

    //Shrinks the seagulls flying radius as an entry method to the game
    private void ShrinkRadius()
    {
        if (width <= widthCopy)
        {
            hasShrank = true;
        }

        if (!hasShrank)
            width -= 0.02f;
    }

    //Points the seagull toward a specific point
    private void RotateForward()
    {
        transform.LookAt(startingVector);
    }

    //Rotates the seagull in a circular motion around a specific area and points it forward
    private void CircleLoop()
    {
        RotateForward();
        CircularMotion();
    }

    //Checks whether the seagull is eligible, then targets the closest turtle once the targetting timer has reached the threshold.
    private void TargetTurtle()
    {
        if (!hasShrank) return;
        if (foundTurtle) return;


        if (targetTimer <= TimeBeforeTarget)
        {
            targetTimer += Time.deltaTime;
            return;
        }

        if (RandomTurtle)
        targetTurtle = ReturnRandomTurtle();

        if (ClosestTurtle)
        targetTurtle = ReturnClosestTurtle();

        if (targetTurtle == null)
        {
            if (RandomTurtle)
                targetTurtle = ReturnRandomTurtle();

            if (ClosestTurtle)
                targetTurtle = ReturnClosestTurtle();
        }

        targetTurtle.gameObject.GetComponent<TurtleBehaviour>().EnableOutline();
        foundTurtle = true;
        currentState = SeagullState.Detect;
    }

    //Returns a random turtle from available list
    private GameObject ReturnRandomTurtle()
    {
        return turtleList[Random.Range(0, turtleList.Count - 1)];
    }

    //Returns the turtle closest to the seagull
    private GameObject ReturnClosestTurtle()
    {
        targetTurtle = turtleList[0];

        float lowestDist = Mathf.Infinity;

        for (int i = 0; i < turtleList.Count; i++)
        {
            float dist = Vector3.Distance(turtleList[i].transform.position, transform.position);

            if (dist < lowestDist)
            {
                lowestDist = dist;
                targetTurtle = turtleList[i];
            }
        }

        return targetTurtle;

    }

    private void RemoveFromTurtles(GameObject turtle)
    {
            
    }

    void DetectTurtleLoss()
    {
        if (!foundTurtle) return;
        if (targetTurtle == null)
        {
            DestroySeagull();
        }
    }
  
    //Called when trash is presented to the seagull before it can take targeted turtle
    private void TrashCancel()
    {
        targetTurtle.gameObject.GetComponent<TurtleBehaviour>().DisableOutline();
        GameObject.FindGameObjectWithTag("DraggableParent").GetComponent<Dragger>().DisableSelected();
        animator.SetTrigger("Pickup");
        trashTarget.transform.position = transform.position;
        MoveFowardUp();
    }

    //Called when seagull reaches target turtle on pursuit
    private void CarryAway()
    {
        animator.SetTrigger("Pickup");
        TurtleTaken = true;
        targetTurtle.GetComponent<TurtleBehaviour>().DisableOutline();
        targetTurtle.GetComponent<TurtleBehaviour>().DisableTurtle();
        targetTurtle.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        MoveFowardUp();
    }

    //Called as an exit cinematic after picking up an object
    private void MoveFowardUp()
    {
        //visualTransfom.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        
        transform.position += new Vector3(visualTransfom.forward.x, targetYModifier, visualTransfom.forward.z) * Time.deltaTime * 25;
        if (targetYModifier < 100)
            targetYModifier += 0.005f;
    }

    //Called after detection
    private void MoveToTurtle()
    {

        visualTransfom.LookAt(targetTurtle.transform);

        transform.position = Vector3.MoveTowards(transform.position, targetTurtle.transform.position, 0.01f * PickupSpeed);

        if (Vector3.Distance(transform.position, targetTurtle.transform.position) < DistanceToPickUp)
        {
            currentState = SeagullState.TurtleExit;
        }
    }

    //Checks the world area's bounds
    private void DetectAreaExit()
    {
        if (transform.position.x >= playArea.terrainData.size.x || transform.position.x <= playArea.transform.position.x || transform.position.y > 50)
        {
            DestroySeagull();
        }
    }

    //Despawns the seagull along with any carried object
    private void DestroySeagull()
    {
        if (TurtleTaken)
        {
            if (targetTurtle != null)
            targetTurtle.GetComponent<TurtleBehaviour>().DestroyTurtle();
        }
        Object.Destroy(gameObject);
    }

    //Called after patrol (circling) as a pre warning animation before the seagull's pursuit
    private void Detection()
    {
        RemoveFromTurtles(targetTurtle);
        visualTransfom.LookAt(targetTurtle.transform);
        animator.SetTrigger("Detect");
        detectionTimer += Time.deltaTime;
        if (detectionTimer >= detectDuration)
        {
            animator.SetTrigger("Pursue");
            currentState = SeagullState.Pursue;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(visualTransfom.position, visualTransfom.forward * 10);
    }

    #endregion
}
