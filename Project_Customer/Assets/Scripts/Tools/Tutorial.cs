using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    #region fields 
    private bool trashTutorial = false;
    private bool seagulTutorial = false;
    private bool crabTutorial = false;
    private bool turtleInTrashTutorial = false;
    public bool vacumTutorial, shieldTutorial, scarecowTutorial, icecreamTutorial = false;

    private bool towardsObject = false;
    private bool backToPlace = false;

    private GameObject objectToMoveTowards;


    private GameObject defaultCamSettings;
    [SerializeField] GameObject pivotCam;
    [SerializeField] GameObject cam;
    [SerializeField] private float camSpeed = 10000;

    [SerializeField] public GameObject trash;
    [SerializeField] public GameObject bin;
    [SerializeField] public GameObject seagul;
    [SerializeField] public GameObject crab;
    [SerializeField] public GameObject powerup;
    [SerializeField] public GameObject turtle;

    private Vector3 pointToMove;

    Outline outline;

    [SerializeField] private TMP_Text trashText;
    [SerializeField] private TMP_Text turtleText;
    [SerializeField] private TMP_Text seagulText;
    [SerializeField] private TMP_Text crabText;
    [SerializeField] private TMP_Text scareCrowText;
    [SerializeField] private TMP_Text shieldText;
    #endregion

    public IEnumerator TrashTutorial(GameObject trashObj)
    {
        if (!trashTutorial)
        {
            yield return new WaitForSecondsRealtime(3f);
            Time.timeScale = 0f;
            trashTutorial = true;
            trashText.gameObject.SetActive(true);

            cam.transform.LookAt(trashObj.transform.position);
            objectToMoveTowards = trashObj;
            if (!(outline = objectToMoveTowards.AddComponent<Outline>()))
            {
                outline = objectToMoveTowards.GetComponent<Outline>();
            }
            EnableOutline();
            towardsObject = true;

            yield return new WaitUntil(() => towardsObject == false);
            yield return new WaitForSecondsRealtime(2f);
            DisableOutline();


            objectToMoveTowards = defaultCamSettings;
            if (!(outline = objectToMoveTowards.AddComponent<Outline>()))
            {
                outline = objectToMoveTowards.GetComponent<Outline>();
            }
            EnableOutline();
            towardsObject = true;

            yield return new WaitUntil(() => towardsObject == false);
            yield return new WaitForSecondsRealtime(2f);
            DisableOutline();

            cam.transform.LookAt(bin.transform.position);
            objectToMoveTowards = bin;
            if (!(outline = objectToMoveTowards.AddComponent<Outline>()))
            {
                outline = objectToMoveTowards.GetComponent<Outline>();
            }
            EnableOutline();
            towardsObject = true;

            yield return new WaitUntil(() => towardsObject == false);
            yield return new WaitForSecondsRealtime(2f);
            DisableOutline();

            objectToMoveTowards = defaultCamSettings;
            if (!(outline = objectToMoveTowards.AddComponent<Outline>()))
            {
                outline = objectToMoveTowards.GetComponent<Outline>();
            }
            EnableOutline();
            towardsObject = true;

            yield return new WaitUntil(() => towardsObject == false);
            DisableOutline();
            cam.transform.rotation = defaultCamSettings.transform.rotation;
            trashText.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        yield return null;
    }

    public IEnumerator SeagulTutorial(GameObject theSeagul)
    {
        if (!seagulTutorial)
        {
            yield return new WaitForSecondsRealtime(8f);
            Time.timeScale = 0f;
            seagulTutorial = true;
            seagulText.gameObject.SetActive(true);

            cam.transform.LookAt(theSeagul.transform.position);
            objectToMoveTowards = theSeagul;
            if (!(outline = objectToMoveTowards.AddComponent<Outline>()))
            {
                outline = objectToMoveTowards.GetComponent<Outline>();
            }
            EnableOutline();
            towardsObject = true;

            yield return new WaitUntil(() => towardsObject == false);
            yield return new WaitForSecondsRealtime(5f);
            DisableOutline();


            objectToMoveTowards = defaultCamSettings;
            if (!(outline = objectToMoveTowards.AddComponent<Outline>()))
            {
                outline = objectToMoveTowards.GetComponent<Outline>();
            }
            EnableOutline();
            towardsObject = true;

            yield return new WaitUntil(() => towardsObject == false);
            DisableOutline();
            cam.transform.rotation = defaultCamSettings.transform.rotation;
            seagulText.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        yield return null;
    }

    public IEnumerator CrabTutorial(GameObject theCrab)
    {
        if (!crabTutorial)
        {
            yield return new WaitForSecondsRealtime(2f);
            Time.timeScale = 0f;
            crabTutorial = true;

            crabText.gameObject.SetActive(true);

            cam.transform.LookAt(theCrab.transform.position);
            objectToMoveTowards = theCrab;
            /*if (!(outline = objectToMoveTowards.AddComponent<Outline>()))
            {
                outline = objectToMoveTowards.GetComponent<Outline>();
            }*/
            //EnableOutline();
            towardsObject = true;

            yield return new WaitUntil(() => towardsObject == false);
            yield return new WaitForSecondsRealtime(2f);
            //DisableOutline();


            objectToMoveTowards = defaultCamSettings;
            /*if(!(outline = objectToMoveTowards.AddComponent<Outline>()))
            {
                outline = objectToMoveTowards.GetComponent<Outline>();
            }
            EnableOutline();*/
            towardsObject = true;

            yield return new WaitUntil(() => towardsObject == false);
            //DisableOutline();

            cam.transform.rotation = defaultCamSettings.transform.rotation;
            crabText.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        yield return null;
    }

    public IEnumerator TurtleTutorial(GameObject theTurtle)
    {
        if (!turtleInTrashTutorial)
        {
            yield return new WaitForSecondsRealtime(2f);
            Time.timeScale = 0f;
            turtleInTrashTutorial = true;

            turtleText.gameObject.SetActive(true);

            cam.transform.LookAt(theTurtle.transform.position);
            objectToMoveTowards = theTurtle;
            if (!(outline = objectToMoveTowards.AddComponent<Outline>()))
            {
                outline = objectToMoveTowards.GetComponent<Outline>();
            }
            EnableOutline();
            towardsObject = true;

            yield return new WaitUntil(() => towardsObject == false);
            yield return new WaitForSecondsRealtime(5f);
            DisableOutline();


            objectToMoveTowards = defaultCamSettings;
            if (!(outline = objectToMoveTowards.AddComponent<Outline>()))
            {
                outline = objectToMoveTowards.GetComponent<Outline>();
            }
            EnableOutline();
            towardsObject = true;

            yield return new WaitUntil(() => towardsObject == false);
            DisableOutline();

            cam.transform.rotation = defaultCamSettings.transform.rotation;
            turtleText.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        yield return null;
    }

    public IEnumerator ScareCrowTutorial(GameObject theScareCrow)
    {
        if (!scarecowTutorial)
        {
            yield return new WaitForSecondsRealtime(2f);
            Time.timeScale = 0f;
            scarecowTutorial = true;

            scareCrowText.gameObject.SetActive(true);

            cam.transform.LookAt(theScareCrow.transform.position);
            objectToMoveTowards = theScareCrow;
            if (!(outline = objectToMoveTowards.AddComponent<Outline>()))
            {
                outline = objectToMoveTowards.GetComponent<Outline>();
            }
            EnableOutline();
            towardsObject = true;

            yield return new WaitUntil(() => towardsObject == false);
            yield return new WaitForSecondsRealtime(5f);
            DisableOutline();


            objectToMoveTowards = defaultCamSettings;
            if (!(outline = objectToMoveTowards.AddComponent<Outline>()))
            {
                outline = objectToMoveTowards.GetComponent<Outline>();
            }
            EnableOutline();
            towardsObject = true;

            yield return new WaitUntil(() => towardsObject == false);
            DisableOutline();

            cam.transform.rotation = defaultCamSettings.transform.rotation;
            scareCrowText.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        yield return null;
    }

    public IEnumerator ShildedTutorial(GameObject theTurtle)
    {
        if (!shieldTutorial)
        {
            shieldTutorial = true;
            yield return new WaitForSecondsRealtime(2f);
            Time.timeScale = 0f;

            shieldText.gameObject.SetActive(true);

            cam.transform.LookAt(theTurtle.transform.position);
            objectToMoveTowards = theTurtle;
            if (!(outline = objectToMoveTowards.AddComponent<Outline>()))
            {
                outline = objectToMoveTowards.GetComponent<Outline>();
            }
            EnableOutline();
            towardsObject = true;

            yield return new WaitUntil(() => towardsObject == false);
            yield return new WaitForSecondsRealtime(5f);
            DisableOutline();


            objectToMoveTowards = defaultCamSettings;
            if (!(outline = objectToMoveTowards.AddComponent<Outline>()))
            {
                outline = objectToMoveTowards.GetComponent<Outline>();
            }
            EnableOutline();
            towardsObject = true;

            yield return new WaitUntil(() => towardsObject == false);
            DisableOutline();

            cam.transform.rotation = defaultCamSettings.transform.rotation;
            shieldText.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        yield return null;
    }

    /*private Vector3 CalculatePointBetweenObjects(GameObject theObject, float distance)
    {
        //return theObject.transform.position;
        *//*return new Vector3((cam.transform.position.x + theObject.transform.position.x) * distance,
            (cam.transform.position.y + theObject.transform.position.y) * distance,
            (cam.transform.position.z + theObject.transform.position.z) * distance);*//*
    }*/

    private void MoveCameraTowardsObject(GameObject objectToMoveTowards)
    {
        pointToMove = objectToMoveTowards.transform.position;
        Debug.Log(pointToMove.ToString());
        //pivotCam.transform.LookAt(objectToMove.transform.position);
        pivotCam.transform.position = Vector3.MoveTowards(pivotCam.transform.position, pointToMove, camSpeed * Time.unscaledDeltaTime);

        if (!objectToMoveTowards.Equals(defaultCamSettings) && (objectToMoveTowards.transform.position - pivotCam.transform.position).magnitude < 20f)
        {
            towardsObject = false;
        }

        if(objectToMoveTowards.Equals(defaultCamSettings) && (objectToMoveTowards.transform.position - pivotCam.transform.position).magnitude == 0)
        {
            towardsObject = false;
        }
    }

    // Start is called before the first frame update 
    void Start()
    {
        defaultCamSettings = new GameObject();
        defaultCamSettings.transform.position = pivotCam.transform.position;
        defaultCamSettings.transform.rotation = cam.transform.rotation;
    }

    // Update is called once per frame 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(TrashTutorial(trash));
        }
        if (towardsObject)
        {
            MoveCameraTowardsObject(objectToMoveTowards);
        }
    }

    public void EnableOutline()
    {
        outline.OutlineWidth = 20;
        outline.OutlineColor = Color.black;
    }

    public void DisableOutline()
    {
        outline.OutlineWidth = 0;
    }
}
