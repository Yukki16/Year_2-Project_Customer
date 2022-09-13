using UnityEngine;

public class Seagull : MonoBehaviour
{
    #region fields
    public enum SeagullState { Patrol, Detect, Pursue, Pickup, TurtleExit, TrashExit };

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
    private GameObject trashTarget;
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

    void Update()
    {

        if (closestTurtle == null)
        {
            currentState = SeagullState.Patrol;
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

        DetectAreaExit();
        TargetTurtle();
        ShrinkRadius();
    }

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

        if (targetTimer <= TimeBeforeTarget)
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


    private void TrashCancel()
    {
        GameObject.FindGameObjectWithTag("DraggableParent").GetComponent<Dragger>().DisableSelected();
        animator.SetTrigger("Pickup");
        trashTarget.transform.position = transform.position;
        MoveFowardUp();
    }

    private void CarryAway()
    {
        animator.SetTrigger("Pickup");
        TurtleTaken = true;
        closestTurtle.GetComponent<TurtleBehaviour>().DisableOutline();
        closestTurtle.GetComponent<TurtleBehaviour>().DisableTurtle();
        closestTurtle.transform.position = transform.position;
        MoveFowardUp();
    }

    private void MoveFowardUp()
    {
        transform.position += new Vector3(visualTransfom.forward.x, targetYModifier, visualTransfom.forward.y) * Time.deltaTime * 25;
        if (targetYModifier < 100)
            targetYModifier += 0.005f;
    }

    private void MoveToTurtle()
    {
        visualTransfom.LookAt(closestTurtle.transform);

        transform.position = Vector3.MoveTowards(transform.position, closestTurtle.transform.position, 0.01f * PickupSpeed);

        if (Vector3.Distance(transform.position, closestTurtle.transform.position) < DistanceToPickUp)
        {
            currentState = SeagullState.TurtleExit;
        }
    }

    private void DetectAreaExit()
    {
        if (transform.position.x >= playArea.terrainData.size.x || transform.position.x <= playArea.transform.position.x || transform.position.y > 50)
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
}
