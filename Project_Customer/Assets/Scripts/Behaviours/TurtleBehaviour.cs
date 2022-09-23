using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class TurtleBehaviour : MonoBehaviour
{
    #region fields
    private Terrain playArea;
    private float offset;

    Mover mover;

    Vector3 targetObjPosition;

    private GameObject turtleSpawnerParent;


    GameObject targetObj;

    private bool invincibleMode;

    public Renderer rend;

    [System.NonSerialized] public Animator animator;

    private int direction = 1;

    int EndDistanceFromTop = 10;

    private MasterFlow masterFlow;

    public int MinWobbleDistance = 5;
    public int MaxWobbleDistance = 10;
    public int WobbleSwitchTimer;
    public int TurtleRemovalPoint = 35;

    Outline outline;

    GameObject turtleShield;

    private bool excecution;

    private LivesSystem livesSystem;
    private Highscore highscore;
    #endregion

    void Start()
    {
        livesSystem = FindObjectOfType<LivesSystem>();
        highscore = FindObjectOfType<Highscore>();
        masterFlow = FindObjectOfType<MasterFlow>();
        animator = GetComponentInChildren<Animator>();
        targetObj = new GameObject();
        turtleSpawnerParent = GameObject.FindGameObjectWithTag("TurtleTargets");
        turtleShield = transform.GetChild(1).gameObject;
        turtleShield.SetActive(false);
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
            Debug.Log("removed");
            RemoveFromList();
        }
    }

    private DG.Tweening.Core.TweenerCore<Color, Color, DG.Tweening.Plugins.Options.ColorOptions> tween;
    private Tween tweenTo;

    public bool GetInvincibleMode()
    {
        return invincibleMode;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag is "Draggable" && !invincibleMode)
        {
            DisableTurtle();
            RemoveFromList();

            if (tween != null && tween.active)
            {               
                tween.Kill();
            }
            
            tween = rend.material.DOColor(Color.red, masterFlow.ReturnTurtleDeathTime());
            tweenTo = DOTween.To(() => animator.GetFloat("WalkSpeed"), value => { animator.SetFloat("WalkSpeed", value); }, 0.1f, masterFlow.ReturnTurtleDeathTime());
            
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

            if (tweenTo != null && tweenTo.active)
            {
                tweenTo.Kill();
                animator.SetFloat("WalkSpeed", 1);
            }

            tween = rend.material.DOColor(Color.white, 1.0f);
            AddSelfToList();
            EnableTurtle();
        }
    }

    public void ToggleInvincible(bool setting)
    {

        turtleShield.SetActive(setting);

        if (setting)
        {
            invincibleMode = true;            
        }

        if (!setting)
        {
            invincibleMode = false;
        }
    }

    void DetectRemovalPoint()
    {
        if (transform.position.z >= TurtleRemovalPoint)
        {
            RemoveFromList();
        }
    }

    void DetectTurtleSlow()
    {
        if (animator.GetFloat("WalkSpeed") <= 0.11f)
        {
            StartCoroutine(TurtleTrashDeath());
        }
    }

    IEnumerator TurtleTrashDeath()
    {
        tweenTo.Kill();
        tween.Kill();
        tween = rend.material.DOColor(Color.white, 0.1f);
        animator.SetFloat("WalkSpeed", 1);
        animator.SetTrigger("TrashDeath");
        yield return new WaitForSeconds(5.01f);
        //StartCoroutine(livesSystem.UpdateLives());
        DestroyTurtle();
    }

    void DestroyOnTarget()
    {
        if (Vector3.Distance(transform.position, targetObj.transform.position) < 1 && excecution)
        {
            DisableTurtle();
        }

        if (Vector3.Distance(transform.position, targetObj.transform.position) < 1 && !excecution)
        {
            //StartCoroutine(highscore.AddScore());
            DestroyTurtle();
        }
    }

    public void RemoveFromList()
    {
        List<GameObject> list = GameObject.FindGameObjectWithTag("TurtleSpawner").GetComponent<SpawnTurtles>().GetTargetableTurtles();

        if (list.Contains(gameObject))
            list.Remove(gameObject);
    }

    public void AddSelfToList()
    {
        GameObject.FindGameObjectWithTag("TurtleSpawner").GetComponent<SpawnTurtles>().GetTargetableTurtles().Add(gameObject);
    }

    public void DestroyTurtle()
    {
        if (tween != null && tween.active)
        {
            tween.Kill();
        }

        if (tweenTo != null && tweenTo.active)
        {
            tweenTo.Kill();
        }

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
        DetectRemovalPoint();
        DestroyOnTarget();
        DetectTurtleSlow();
    }


}
