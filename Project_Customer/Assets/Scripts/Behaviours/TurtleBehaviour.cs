using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleBehaviour : MonoBehaviour
{
    Rigidbody objectRigidBody = null;

    [SerializeField] private float forwardSpeed = 1;

    [SerializeField] private float sidewaySpeed = 100;

    private Vector3 directionVector = new Vector3(0,0,0);

    private float directionReduction = 0.98f;

    // Start is called before the first frame update
    void Start()
    {
        objectRigidBody = GetComponent<Rigidbody>();
        StartCoroutine(ChangeDirection());
    }
    // Update is called once per frame
    void Update()
    {
        Movement(); 
    }

    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(4);
        directionVector = new Vector3(1f, 0, 0);
        int randomValue = Random.Range(-1, 2);
        directionVector *= randomValue;
        StartCoroutine(ChangeDirection());
    }
    private void Movement()
    {
        Vector3 forceVector = Time.deltaTime * forwardSpeed * transform.forward;
        directionVector *= directionReduction * sidewaySpeed * Time.deltaTime;

        objectRigidBody.AddForce(forceVector + directionVector);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Unspawn")
        {
            Destroy(this.gameObject);
        }
    }
}
