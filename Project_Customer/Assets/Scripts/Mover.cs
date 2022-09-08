using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameObject targetPrefab;
    NavMeshAgent navMeshAgent;

    private void Start()
    {
        GameObject physicalTarget = Instantiate(targetPrefab);
        physicalTarget.transform.position = new Vector3(transform.position.x * 50, transform.position.y, transform.position.z);
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = physicalTarget.transform;

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
