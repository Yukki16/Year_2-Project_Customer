using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    #region variables 
    private bool trashTutorial = false;
    private bool seagulTutorial = false;
    private bool crabTutorial = false;
    private bool turtleInTrashTutorial = false;
    private bool vacumTutorial, shieldTutorial, scarecowTutorial, icecreamTutorial = false;


    [SerializeField] Camera cam;
    private Camera defaultCamSettings;

    [SerializeField] GameObject trash;
    [SerializeField] GameObject bin;
    [SerializeField] GameObject seagul;
    [SerializeField] GameObject crab;
    [SerializeField] GameObject powerup;
    [SerializeField] GameObject turtle;
    #endregion

    public IEnumerator TrashTutorial(GameObject trashObj)
    {
        if (!trashTutorial)
        {
            trashTutorial = true;
            Vector3 pointToMove = CalculatePointBetweenObjects(trashObj, 3 / 4);
            cam.transform.LookAt(trashObj.transform.position);
            cam.transform.position = new Vector3(Mathf.MoveTowards(cam.transform.position.x, pointToMove.x, 1 * Time.unscaledDeltaTime),
                                                    Mathf.MoveTowards(cam.transform.position.y, pointToMove.y, 1 * Time.unscaledDeltaTime),
                                                    Mathf.MoveTowards(cam.transform.position.z, pointToMove.z, 1 * Time.unscaledDeltaTime));
            yield return new WaitForSeconds(2f);
            cam.transform.position = new Vector3(Mathf.MoveTowards(cam.transform.position.x, defaultCamSettings.transform.position.x, 1 * Time.unscaledDeltaTime),
                                                    Mathf.MoveTowards(cam.transform.position.y, defaultCamSettings.transform.position.y, 1 * Time.unscaledDeltaTime),
                                                    Mathf.MoveTowards(cam.transform.position.z, defaultCamSettings.transform.position.z, 1 * Time.unscaledDeltaTime));
            yield return new WaitForSeconds(1f);
            cam.transform.LookAt(bin.transform.position);
            cam.transform.position = new Vector3(Mathf.MoveTowards(cam.transform.position.x, bin.transform.position.x, 1 * Time.unscaledDeltaTime),
                                                    Mathf.MoveTowards(cam.transform.position.y, bin.transform.position.y, 1 * Time.unscaledDeltaTime),
                                                    Mathf.MoveTowards(cam.transform.position.z, bin.transform.position.z, 1 * Time.unscaledDeltaTime));
            yield return new WaitForSeconds(2f);
            cam.transform.position = new Vector3(Mathf.MoveTowards(cam.transform.position.x, defaultCamSettings.transform.position.x, 1 * Time.unscaledDeltaTime),
                                                    Mathf.MoveTowards(cam.transform.position.y, defaultCamSettings.transform.position.y, 1 * Time.unscaledDeltaTime),
                                                    Mathf.MoveTowards(cam.transform.position.z, defaultCamSettings.transform.position.z, 1 * Time.unscaledDeltaTime));

            cam.transform.Rotate(CalculateAngleBetweenCameras());

        }
        yield return null;
    }

    private Vector3 CalculatePointBetweenObjects(GameObject theObject, float distance)
    {
        return new Vector3((cam.transform.position.x + theObject.transform.position.x) * distance,
            (cam.transform.position.y + theObject.transform.position.y) * distance,
            (cam.transform.position.z + theObject.transform.position.z) * distance);
    }

    private Vector3 CalculateAngleBetweenCameras()
    {
        return new Vector3((cam.transform.rotation.x + defaultCamSettings.transform.rotation.x),
            (cam.transform.rotation.y + defaultCamSettings.transform.rotation.y),
            (cam.transform.rotation.z + defaultCamSettings.transform.rotation.z));
    }
    // Start is called before the first frame update 
    void Start()
    {
        defaultCamSettings = cam;
    }

    // Update is called once per frame 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(TrashTutorial(trash));
        }
    }
}
