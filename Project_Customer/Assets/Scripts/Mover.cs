using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] GameObject playArea;
    [SerializeField] Transform target;
    GameObject targetObj;
    NavMeshAgent navMeshAgent;

    private void Start()
    {

        targetObj = new GameObject();
        targetObj.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        navMeshAgent = GetComponent<NavMeshAgent>();
        target = targetObj.transform;

    }

    void SpawnTarget()
    {
        targetObj = new GameObject();
        targetObj.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);


    }

    public void MoveTo(Vector3 destination)
    {
        navMeshAgent.destination = destination;
        navMeshAgent.isStopped = false;
    }

    public void Cancel()
    {
        navMeshAgent.isStopped = true;
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        MoveTo(target.transform.position);
    }
    void Update()
    {
        UpdateAnimator();
    }
}
