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

    private Animator animator;

    private MeshCollider mc;

    #endregion

    void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        animator = GetComponentInChildren<Animator>();
        //particles.Stop();
        currentState = CrabState.Stationary;
        playArea = Terrain.activeTerrain;
    }


    private void OnTriggerEnter (Collider collision)
    {
        if (collision.gameObject.tag is "Turtle")
        {
            animator.SetTrigger("DetectTurtle");
            //Destroy(gameObject); 
            StartCoroutine(DetectTurtle(collision));

        }

        if (collision.gameObject.tag is "Draggable")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

    }

    IEnumerator DetectTurtle(Collider collision)
    {
        animator.SetTrigger("DetectTurtle");
        TurtleBehaviour tb = collision.gameObject.GetComponent<TurtleBehaviour>();
        tb.animator.SetTrigger("CrabDeath");    

        tb.StartCoroutine(tb.MoveTowards(transform.position));

        //wait until excecution animation is played
        yield return new WaitForSeconds(1.90f);
        tb.DestroyTurtle();
        Destroy(gameObject); 

    }
}
