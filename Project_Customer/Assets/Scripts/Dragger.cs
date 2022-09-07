using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragger : MonoBehaviour 
{
    private GameObject selectedObject;

    private Vector3 lastMousePos;

    Vector3 previousGrabPosition;

<<<<<<< Updated upstream
=======
    public float MaxVelocity;

>>>>>>> Stashed changes
    private bool isReleased;

    float forceMultiplier = 3;

    private void Start()
    {
        lastMousePos = Input.mousePosition;
    }

    private void Update()
    {

        lastMousePos = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            if(selectedObject == null)
            {
                RaycastHit hit = CastRay();

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
<<<<<<< Updated upstream
            selectedObject.transform.position = new Vector3(worldPosition.x, 0f, worldPosition.z);

            Vector3 throwVector = selectedObject.transform.position - previousGrabPosition;
            float speed = throwVector.magnitude / Time.deltaTime;
            Vector3 throwVelocity = speed * throwVector.normalized;
            rb.velocity = throwVelocity;
=======
            selectedObject.transform.position = new Vector3(worldPosition.x, 1, worldPosition.z);

            Vector3 throwVector = selectedObject.transform.position - previousGrabPosition;
            float speed = throwVector.magnitude / Time.deltaTime;
           
            Vector3 throwVelocity = speed * throwVector.normalized;
               
            if (speed <= MaxVelocity)
            {
                rb.velocity = throwVelocity;
            }
            else
            {
                rb.velocity = MaxVelocity * throwVector.normalized;
            }
         
>>>>>>> Stashed changes
            selectedObject.GetComponent<Rigidbody>().useGravity = true;

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
<<<<<<< Updated upstream
            selectedObject.transform.position = new Vector3(worldPosition.x, 0.25f, worldPosition.z);
=======
            selectedObject.transform.position = new Vector3(worldPosition.x, 1, worldPosition.z);
>>>>>>> Stashed changes
            selectedObject.GetComponent<Rigidbody>().useGravity = false;
        }
    }

<<<<<<< Updated upstream

    void FireObject(Vector3 Force)
    {
       

        Debug.Log(Input.GetAxis("Mouse X"));

        selectedObject.GetComponent<Rigidbody>().AddForce(new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * forceMultiplier);
   
    }

=======
>>>>>>> Stashed changes
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

