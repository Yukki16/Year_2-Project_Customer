using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagulBehavior : MonoBehaviour
{
    private Terrain playArea;
    private Mover mover;

    private Vector3 targetPozition;

    public GameObject turtle;
    private Mover turtleMover;

    private GameObject seagulTargets;
    private GameObject targetGameObj;

    private bool pickedUpTurtle = false;
    // Start is called before the first frame update
    void Start()
    {
        turtleMover = turtle.GetComponent<Mover>();
        targetGameObj = new GameObject();

        seagulTargets = GameObject.FindGameObjectWithTag("SeagulTargets");

        playArea = Terrain.activeTerrain;
        mover = GetComponent<Mover>();
        SpawnTarget();

        StartCoroutine(CorrectRoute());
    }

    void SetTargetGameObjPosition(Vector3 position)
    {
        targetGameObj.transform.position = position;
    }

    void SpawnTarget()
    {
        targetGameObj.transform.SetParent(seagulTargets.transform);
        targetPozition = new Vector3(turtle.transform.position.x, turtle.transform.position.y, turtle.transform.position.z);
        targetGameObj.transform.position = targetPozition;
        mover.SetTarget(targetGameObj.transform);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Turtle")
        {
            pickedUpTurtle = true;
            turtle.transform.parent = this.transform;
            turtleMover.Cancel();
            targetPozition = new Vector3(playArea.terrainData.size.x, 10, playArea.terrainData.size.z / 2);
            SetTargetGameObjPosition(targetPozition);
        }
    }

    IEnumerator CorrectRoute()
    {
        if (pickedUpTurtle)
        {
            yield return null;
        }
        else
        {
            targetPozition = new Vector3(turtle.transform.position.x, turtle.transform.position.y, turtle.transform.position.z);
            targetGameObj.transform.position = targetPozition;
            mover.SetTarget(targetGameObj.transform);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(CorrectRoute());
        }



    }

    // Update is called once per frame
    void Update()
    {

    }
}
