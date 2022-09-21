using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragger : MonoBehaviour
{
    private GameObject selectedObject;

    private Vector3 lastMousePos;

    Vector3 previousGrabPosition;

    public int GrabHeight = 3;

    public float MaxVelocity;

    public int MinThrowVelocity = 20;

    [SerializeField] private GameObject blobShadowPrefab;
    private bool addedBlob = false;

    private GameObject blob = null;
    private bool doneOnce = false;

    private void Start()
    {
        lastMousePos = Input.mousePosition;
    }

    void EnableOutline(GameObject trashObject)
    {
        if (trashObject == null)
            return;

        if (!trashObject.CompareTag("Draggable"))
            return;

        trashObject.gameObject.GetComponent<Outline>().OutlineWidth = 3;
    }

    public void DisableSelected()
    {
        Cursor.visible = true;
        //lrOfTrash.enabled = false;
        selectedObject = null;
    }

    private void Update()
    {
        lastMousePos = Input.mousePosition;

        RaycastHit hit = CastRay();
        if (hit.collider != null)
            //EnableOutline(hit.collider.gameObject);
            if (Input.GetMouseButtonDown(0))
            {
                if (selectedObject == null)
                {
                    if (hit.collider != null)
                    {
                        if (!hit.collider.CompareTag("Draggable"))
                        {
                            return;
                        }
                        hit.collider.GetComponent<TrashBehaviour>().DisableBinCollection();
                        selectedObject = hit.collider.gameObject;
                        Cursor.visible = false;
                    }
                }
            }

        //TODO create methods for scripts below

        if (Input.GetMouseButtonUp(0) && selectedObject != null)
        {
            selectedObject.GetComponent<TrashBehaviour>().EnableBinCollection();

            Rigidbody rb = selectedObject.GetComponent<Rigidbody>();

            Vector3 position = new Vector3(Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);

            selectedObject.transform.position = new Vector3(worldPosition.x, 0f, worldPosition.z);

            //Vector3 throwVector = selectedObject.transform.position - previousGrabPosition;
            //float speed = throwVector.magnitude / Time.deltaTime;
            //Vector3 throwVelocity = speed * throwVector.normalized;
            //rb.velocity = throwVelocity;

            selectedObject.transform.position = new Vector3(worldPosition.x, GrabHeight, worldPosition.z);


            //throwVector = selectedObject.transform.position - previousGrabPosition;
            //speed = throwVector.magnitude / Time.deltaTime;

            //throwVelocity = speed * throwVector.normalized;

            //if (speed <= MaxVelocity)
            //{
            //    rb.velocity = throwVelocity;
            //}

           
            rb.velocity = Vector3.zero;
            


            selectedObject.GetComponent<Rigidbody>().useGravity = true;
            selectedObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            selectedObject = null;
            blob.SetActive(false);
            Cursor.visible = true;
            doneOnce = false;
        }

        if (selectedObject != null)
        {
            previousGrabPosition = selectedObject.transform.position;

            Vector3 position = new Vector3(Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);

            selectedObject.transform.position = new Vector3(worldPosition.x, 0.25f, worldPosition.z);

            selectedObject.transform.position = new Vector3(worldPosition.x, GrabHeight, worldPosition.z);

            selectedObject.GetComponent<Rigidbody>().useGravity = false;
            AddBlobShadow();
        }
    }

    private void AddBlobShadow()
    {
        Vector3 blobPoz = new Vector3(selectedObject.transform.position.x, 0, selectedObject.transform.position.z);
        if(!addedBlob)
        {
            addedBlob = true;
            blob = Instantiate(blobShadowPrefab, blobPoz, blobShadowPrefab.transform.rotation);
        }
        else
        {
            if (!doneOnce)
            {
                selectedObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                blob.SetActive(true);
                doneOnce = true;
            }
        }

        blob.transform.position = blobPoz;
    }
    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);

        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);

        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }


}