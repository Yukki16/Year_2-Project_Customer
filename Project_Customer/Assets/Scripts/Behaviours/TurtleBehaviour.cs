using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class TurtleBehaviour : MonoBehaviour
{
    private Terrain playArea;

    private float offset;

    Mover mover;

    Vector3 targetObjPosition;

    private GameObject turtleSpawnerParent;

    GameObject targetObj;

    public Renderer rend;

    [System.NonSerialized] public Animator animator;

    private int direction = 1;

    int EndDistanceFromTop = 15;

    private MasterFlow masterFlow;

    public int MinWobbleDistance = 5;
    public int MaxWobbleDistance = 10;
    public int WobbleSwitchTimer;

    Outline outline;
    private bool excecution;

    void Start()
    {
        masterFlow = FindObjectOfType<MasterFlow>();
        //rend = GetComponentInChildren<Renderer>();
        animator = GetComponentInChildren<Animator>();
        targetObj = new GameObject();
        turtleSpawnerParent = GameObject.FindGameObjectWithTag("TurtleTargets");
        outline = GetComponent<Outline>();
        mover = GetComponent<Mover>();
        playArea = Terrain.activeTerrain;
        //StartCoroutine(WiggleTarget());
        SpawnTarget();
    }

    void SetTargetObjPosition(Vector3 position)
    {
        targetObj.transform.position = position;
    }

    void SpawnTarget()
    {
        targetObj.transform.SetParent(turtleSpawnerParent.transform);
        targetObjPosition = new Vector3(transform.position.x, transform.position.y, playArea.terrainData.size.z - EndDistanceFromTop);
        targetObj.transform.position = targetObjPosition;
        mover.SetTarget(targetObj.transform);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag is "TurtleUntargetable")
        {
            RemoveFromList();
        }
    }
    private DG.Tweening.Core.TweenerCore<Color, Color, DG.Tweening.Plugins.Options.ColorOptions> tween;
    private DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> speedTween;
        
        private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag is "Draggable")
        {
            Debug.Log("contact");
            DisableTurtle();
            if (tween != null && tween.active)
            {               
                tween.Kill();
            }
            
            tween = rend.material.DOColor(Color.red, masterFlow.ReturnTurtleDeathTime());
            DOTween.To(() => animator.GetFloat("WalkSpeed"), value => { animator.SetFloat("WalkSpeed", value); }, 0.1f, masterFlow.ReturnTurtleDeathTime());
            //rend.material.shader = Shader.Find("Universal Render Pipeline/Simple Lit");
            //rend.material.SetColor("_BaseColor", new Color(255, 0.4f, 0.6f, 255f));
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag is "Draggable")
        {
            if (tween != null && tween.active)
            {
                tween.Kill();
            }
            tween = rend.material.DOColor(Color.white, 1.0f);
            EnableTurtle();
        }
    }

    void DestroyOnTarget()
    {
        if (Vector3.Distance(transform.position, targetObj.transform.position) < 1 && excecution)
        {
            DisableTurtle();
        }

        if (Vector3.Distance(transform.position, targetObj.transform.position) < 1 && !excecution)
        {
            DestroyTurtle();
        }
    }

    public void RemoveFromList()
    {
        GameObject.FindGameObjectWithTag("TurtleSpawner").GetComponent<SpawnTurtles>().GetTurtles().Remove(gameObject);
    }

    public void DestroyTurtle()
    {
        RemoveFromList();
        Destroy(targetObj);
        Destroy(gameObject);
    }

    public void EnableOutline()
    {
        outline.OutlineWidth = 2;
    }

    public void DisableOutline()
    {
        outline.OutlineWidth = 0;
    }

    public void DisableTurtle()
    {
        mover.Cancel();
    }

    public void EnableTurtle()
    {
        mover.Enable();
    }

    //TODO
    public IEnumerator MoveTowards(Vector3 position)
    {
        excecution = true;
        GetComponent<NavMeshAgent>().speed = 5;
        targetObj.transform.position = position;
        yield return null;
    }

    IEnumerator WiggleTarget()
    {
        if (excecution)
             yield return null ;

        switch (direction)
        {
            case 1:
                offset = Random.Range(MinWobbleDistance, MaxWobbleDistance);
                direction = 2;
                break;

            case 2:               
                offset = Random.Range(-MinWobbleDistance, -MaxWobbleDistance);
                direction = 1;
                break;

        }

        targetObjPosition.x += offset;

        SetTargetObjPosition(targetObjPosition);

        yield return new WaitForSeconds(WobbleSwitchTimer);

        StartCoroutine(WiggleTarget());
    }


    void Update()
    {
        DestroyOnTarget();
    }


}
