using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabBehaviour : MonoBehaviour
{
    #region Fields
    //private enum CrabState


    #endregion

    private void OnTriggerEnter (Collider collision)
    {
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
