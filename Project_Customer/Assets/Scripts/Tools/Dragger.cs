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


    public Color lineColor;
    private LineRenderer lrOfTrash;
    [SerializeField] private Material lrShader;

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
                        selectedObject = hit.collider.gameObject;
                        Cursor.visible = false;
                    }
                }
            }

        //TODO create methods for scripts below

        if (Input.GetMouseButtonUp(0) && selectedObject != null)
        {
            Rigidbody rb = selectedObject.GetComponent<Rigidbody>();

            Vector3 position = new Vector3(Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);

            selectedObject.transform.position = new Vector3(worldPosition.x, 0f, worldPosition.z);

            Vector3 throwVector = selectedObject.transform.position - previousGrabPosition;
            float speed = throwVector.magnitude / Time.deltaTime;
            Vector3 throwVelocity = speed * throwVector.normalized;
            rb.velocity = throwVelocity;

            selectedObject.transform.position = new Vector3(worldPosition.x, GrabHeight, worldPosition.z);


            throwVector = selectedObject.transform.position - previousGrabPosition;
            speed = throwVector.magnitude / Time.deltaTime;

            throwVelocity = speed * throwVector.normalized;

            if (speed <= MaxVelocity)
            {
                rb.velocity = throwVelocity;
            }

            if (speed <= MinThrowVelocity)
            {
                rb.velocity = Vector3.zero;
            }


            selectedObject.GetComponent<Rigidbody>().useGravity = true;

            selectedObject.GetComponent<LineRenderer>().enabled = false;

            selectedObject = null;
            Cursor.visible = true;
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

            AddLineRay();
            UpdateLine();
        }
    }

    private void UpdateLine()
    {
        lrOfTrash.SetPosition(0, selectedObject.transform.position);
        lrOfTrash.SetPosition(1, new Vector3(selectedObject.transform.position.x, selectedObject.transform.position.y - 5, selectedObject.transform.position.z));
    }
    private void AddLineRay()
    {
        //Adds the line renderer to the object
        if (!selectedObject.TryGetComponent(typeof(LineRenderer), out Component component))
        {
            lrOfTrash = selectedObject.AddComponent<LineRenderer>();
            lrOfTrash.material = lrShader;

            Debug.Log("Added the linerenderer");

            lrOfTrash.useWorldSpace = true;
            //Adds the 2 points of the line so it renders
            lrOfTrash.SetPosition(0, selectedObject.transform.position);
            //Debug.Log(selectedObject.GetComponent<Renderer>().bounds.center);
            //Debug.Log(selectedObject.transform.position);
            lrOfTrash.SetPosition(1, new Vector3(selectedObject.transform.position.x, selectedObject.transform.position.y - 5, selectedObject.transform.position.z));
            lrOfTrash.startWidth = 0.1f;
            lrOfTrash.endWidth = 0.1f;

            lrOfTrash.alignment = LineAlignment.View;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(lineColor, 0.0f), new GradientColorKey(lineColor, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(1f, 1.0f) }
            );
            lrOfTrash.colorGradient = gradient;
        }
        else
        {
            lrOfTrash.enabled = true;
        }
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