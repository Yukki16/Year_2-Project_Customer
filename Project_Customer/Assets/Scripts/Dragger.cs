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

    private void Start()
    {
        lastMousePos = Input.mousePosition;
    }

    void EnableOutline(GameObject trashObject)
    {
        if (trashObject == null)
            return;

        if (!trashObject.CompareTag("draggable"))
            return;
        
        trashObject.gameObject.GetComponent<Outline>().OutlineWidth = 3;
        
        
    }

    private void Update()
    {

        lastMousePos = Input.mousePosition;

        RaycastHit hit = CastRay();
        if (hit.collider != null)
        EnableOutline(hit.collider.gameObject);

        if (Input.GetMouseButtonDown(0))
        {
            if(selectedObject == null)
            {
                if(hit.collider != null)
                {
                    if (!hit.collider.CompareTag("draggable"))
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
            else
            {
                rb.velocity = MaxVelocity * throwVector.normalized;
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
        selectedObject.GetComponent<LineRenderer>().SetPosition(0, selectedObject.transform.position);
        selectedObject.GetComponent<LineRenderer>().SetPosition(1, new Vector3(selectedObject.transform.position.x, selectedObject.transform.position.y - 5, selectedObject.transform.position.z));
    }
    private void AddLineRay()
    {
        //Adds the line renderer to the object
        if (!selectedObject.TryGetComponent(typeof(LineRenderer), out Component component))
        {
            selectedObject.AddComponent<LineRenderer>();
            Debug.Log("Added the linerenderer");

            selectedObject.GetComponent<LineRenderer>().useWorldSpace = true;
            //Adds the 2 points of the line so it renders
            selectedObject.GetComponent<LineRenderer>().SetPosition(0, selectedObject.transform.position);
            //Debug.Log(selectedObject.GetComponent<Renderer>().bounds.center);
            //Debug.Log(selectedObject.transform.position);
            selectedObject.GetComponent<LineRenderer>().SetPosition(1, new Vector3(selectedObject.transform.position.x, selectedObject.transform.position.y - 5, selectedObject.transform.position.z));
            selectedObject.GetComponent<LineRenderer>().startWidth = 0.1f;
            selectedObject.GetComponent<LineRenderer>().endWidth = 0.1f;

            selectedObject.GetComponent<LineRenderer>().alignment = LineAlignment.View;
        }
        else
        {
            selectedObject.GetComponent<LineRenderer>().enabled = true;
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

