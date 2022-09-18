using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabBehaviour : MonoBehaviour
{
    #region Fields
    private enum CrabState { Stationary, Capturing, Exit}

    private CrabState currentState;

    private Terrain playArea;

    private ParticleSystem particles;

    private MeshCollider mc;

    #endregion

    void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        particles.Stop();
        currentState = CrabState.Stationary;
        playArea = Terrain.activeTerrain;
    }


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
