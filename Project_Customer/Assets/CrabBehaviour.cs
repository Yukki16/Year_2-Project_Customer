using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabBehaviour : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Turtle")
        {
            Debug.Log("crabDetect");
            collision.gameObject.GetComponent<TurtleBehaviour>().DestroyTurtle();
        } 

        if (collision.gameObject.tag == "Draggable")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

    }

}
