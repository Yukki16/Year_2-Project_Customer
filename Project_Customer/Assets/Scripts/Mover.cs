using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    private Transform target;
    GameObject targetObj;
    NavMeshAgent navMeshAgent;

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    private void Start()
    {
        targetObj = new GameObject();
        targetObj.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        navMeshAgent = GetComponent<NavMeshAgent>();
        target = targetObj.transform;
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

    public void StopAgent()
    {
        navMeshAgent.velocity = Vector3.zero;
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
