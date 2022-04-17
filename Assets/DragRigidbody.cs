using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRigidbody : MonoBehaviour
{
    /*
     * stolen from: https://sharpcoderblog.com/blog/drag-rigidbody-with-mouse-cursor-unity-3d-tutorial
     */

    public float forceAmount = 500;
    public float zForceAmount = 1000;

    Rigidbody selectedRigidbody;
    Camera targetCamera;
    private Vector3 originalScreenTargetPosition;
    private Vector3 originalRigidbodyPosition;
    private float selectionDistance;

    private bool isMouseDown = false;
    private int forwardZForce = 0;
    private int backwardZForce = 0;

    // Start is called before the first frame update
    void Start()
    {
        targetCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetCamera)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Check if we are hovering over a Rigidbody, if so, select it
            selectedRigidbody = GetRigidBodyFromMouseClick();
            isMouseDown = true;
        }

        if (Input.GetMouseButtonUp(0) && selectedRigidbody)
        {
            //Release selected Rigidbody if there is any
            selectedRigidbody = null;
            isMouseDown = false;
            resetOnMouseUp();
        }

        //Mouse ScrollWheel Input
        if (isMouseDown && Input.GetAxis("Mouse ScrollWheel") > 0f) //forward   
        {
            forwardZForce++;
        } else if (isMouseDown && Input.GetAxis("Mouse ScrollWheel") < 0f) //backward
        {
            backwardZForce++;
        }
    }

    private void resetOnMouseUp()
    {
        forwardZForce = 0;
        backwardZForce = 0;
    }

    private void FixedUpdate()
    {
        //movement stuff here
        if (selectedRigidbody)
        {
            //Debug.Log(">>> 1 : " + selectionDistance);
            //Debug.Log(">>> 2 : " + selectedRigidbody.position.z);

            Vector3 mousePositionOffset =
                targetCamera.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance)) - originalScreenTargetPosition;


            Vector3 dragVectorForce = new Vector3(originalRigidbodyPosition.x+mousePositionOffset.x-selectedRigidbody.transform.position.x,
                originalRigidbodyPosition.y + mousePositionOffset.y - selectedRigidbody.transform.position.y,
                0);
            selectedRigidbody.velocity = dragVectorForce * forceAmount * Time.deltaTime;

            if (forwardZForce>0)
            {
                Vector3 zForce = new Vector3(0, 0, 1);
                selectedRigidbody.AddForce(zForce * zForceAmount * Time.deltaTime, ForceMode.Impulse);
                forwardZForce--;
            }
            if (backwardZForce>0)
            {
                Vector3 zForce = new Vector3(0, 0, -1);
                selectedRigidbody.AddForce(zForce * zForceAmount * Time.deltaTime, ForceMode.Impulse);
                backwardZForce--;
            }

            //Debug.DrawLine(originalRigidbodyPosition + mousePositionOffset, selectedRigidbody.transform.position, Color.red);
        }
    }

    private Rigidbody GetRigidBodyFromMouseClick()
    {
        RaycastHit hitInfo = new RaycastHit();
        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(ray, out hitInfo);

        if (hit)
        {
            if (hitInfo.collider.gameObject.GetComponent<Rigidbody>())
            {
                //todo-ck check tag of gameobject


                selectionDistance = Vector3.Distance(ray.origin, hitInfo.point);
                
                originalScreenTargetPosition = targetCamera.ScreenToWorldPoint(
                    new Vector3(
                        Input.mousePosition.x,
                        Input.mousePosition.y,
                        selectionDistance
                    ));

                originalRigidbodyPosition = hitInfo.collider.transform.position;

                return hitInfo.collider.gameObject.GetComponent<Rigidbody>();
            }  
            
        }

        return null;
    }
}
