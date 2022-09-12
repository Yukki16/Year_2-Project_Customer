using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabBehaviour : MonoBehaviour
{
    private void OnTriggerEnter (Collider collision)
    {
        Debug.Log("crabDetect");

        if (collision.gameObject.tag is "Turtle")
        {
            Destroy(gameObject); 
            collision.gameObject.GetComponent<TurtleBehaviour>().DestroyTurtle();
        } 

        if (collision.gameObject.tag is "Draggable")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

    }

}
