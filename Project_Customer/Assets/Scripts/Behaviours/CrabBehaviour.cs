using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabBehaviour : MonoBehaviour
{
    #region Fields
    private enum CrabState { Stationary, Capturing, Exit}

    private bool entered;

    private bool triggered;

    private CrabState currentState;

    private Terrain playArea;

    private ParticleSystem particles;

    private Animator animator;

    private MeshCollider mc;

    private LivesSystem livesSystem;
    private Highscore highscore;
    #endregion

    void Start()
    {
        livesSystem = FindObjectOfType<LivesSystem>();
        highscore = FindObjectOfType<Highscore>();
        particles = GetComponentInChildren<ParticleSystem>();
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(EntryDelay());
        currentState = CrabState.Stationary;
        playArea = Terrain.activeTerrain;

        FindObjectOfType<AudioManager>().Play("SandEmerge", true);
    }


    private void OnTriggerStay (Collider collision)
    {
        if (!entered) return;
        if (triggered) return;
        if (collision.gameObject.tag is "Turtle")
        {
            animator.SetTrigger("DetectTurtle");
            //Destroy(gameObject); 
            StartCoroutine(DetectTurtle(collision));

        }

        if (collision.gameObject.tag is "Draggable")
        {
            if (!collision.gameObject.GetComponent<TrashBehaviour>().hasBeenPickedUp)
                return;

            StartCoroutine(DetectTrash(collision));
        }

    }

    IEnumerator EntryDelay()
    {
        yield return new WaitForSeconds(3f);
        entered = true;
    }

    IEnumerator DetectTrash(Collider collision)
    {
        //wait until excecution animation is played
        triggered = true;
        yield return new WaitForSeconds(0.23f);
        if (collision.gameObject != null)
        Destroy(collision.gameObject);
        animator.SetTrigger("ShrinkHill");
        yield return new WaitForSeconds(0.33f);
        StartCoroutine(highscore.AddScore());
        Destroy(gameObject);
    }

    IEnumerator DetectTurtle(Collider collision)
    {
        triggered = true;
        animator.SetTrigger("DetectTurtle");
        TurtleBehaviour tb = collision.gameObject.GetComponent<TurtleBehaviour>();
        if (!tb.GetInvincibleMode())
        {
            tb.animator.SetTrigger("CrabDeath");
            tb.StartCoroutine(tb.MoveTowards(transform.position));
            //wait until excecution animation is played
            yield return new WaitForSeconds(1f);
            tb.DestroyTurtle();
            yield return new WaitForSeconds(0.53f);
            animator.SetTrigger("ShrinkHill");
            yield return new WaitForSeconds(0.33f);
            StartCoroutine(livesSystem.UpdateLives());
            Destroy(gameObject);
        }
        else
        {
            //wait until excecution animation is played
            yield return new WaitForSeconds(1f);
            yield return new WaitForSeconds(0.53f);
            animator.SetTrigger("ShrinkHill");
            yield return new WaitForSeconds(0.33f);
            Destroy(gameObject);
        }
    }
}
