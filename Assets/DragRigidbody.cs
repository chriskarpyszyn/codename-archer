using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRigidbody : MonoBehaviour
{
    /*
     * stolen from: https://sharpcoderblog.com/blog/drag-rigidbody-with-mouse-cursor-unity-3d-tutorial
     */

    public float dragingForce = 500;
    public float throwForce = 1000;
    public float scrollwheelForce = 0.5f;

    Rigidbody selectedRigidbody;
    Camera targetCamera;
    private Vector3 originalScreenTargetPosition;
    private Vector3 originalRigidbodyPosition;
    private float selectionDistance;

    private bool isMouseDown = false;
    private bool throwObject = false;
    private float scrollWheelZOffset = 0f;

    private float sphereRadius = 3f;
    private float maxDistance = 0.5f;
    private GameObject currentHitObject;

    private float zOffset = 0;

    private Vector3 origin;
    private Vector3 direction;
    private float currentHitDistance;

    // Start is called before the first frame update
    void Start()
    {
        targetCamera = GetComponent<Camera>();

        //code to ignore raycast
        //todo-ck maybe pass objects by ref to ignore other objects as well
        GameObject groundGameObject = GameObject.Find("Ground");
        GameObject tubeGameObject = GameObject.Find("Cylinder");
        groundGameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        tubeGameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
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

        if (isMouseDown && Input.GetMouseButtonDown(1))
        {
            throwObject = true;
        }

        //Mouse ScrollWheel Input
        if (isMouseDown && Input.GetAxis("Mouse ScrollWheel") > 0f) //forward   
        {
            //scrollWheelZOffset += -scrollwheelForce * Mathf.Sign(Input.getAxis("Mouse ScrollWheel"))
            scrollWheelZOffset = scrollWheelZOffset + scrollwheelForce;
        } else if (isMouseDown && Input.GetAxis("Mouse ScrollWheel") < 0f) //backward
        {
            scrollWheelZOffset = scrollWheelZOffset - scrollwheelForce;
        }
    }

    private void resetOnMouseUp()
    {
        scrollWheelZOffset = 0;
        zOffset = 0;

    }

    private void FixedUpdate()
    {
        //movement stuff here
        if (selectedRigidbody)
        {
            //Debug.Log(">>> 1 : " + selectionDistance);
            //Debug.Log(">>> 2 : " + selectedRigidbody.position.z);


            //ray cast part deux
            /////////////////////////////////////
            origin = selectedRigidbody.position;
            direction = -transform.up;
            RaycastHit hit;
            if (Physics.SphereCast(origin, sphereRadius, direction, out hit, maxDistance))
            {
                currentHitObject = hit.transform.gameObject;
                currentHitDistance = hit.distance;
                zOffset = (currentHitObject.transform.position.z - selectedRigidbody.transform.position.z);
            } else
            {
                currentHitDistance = maxDistance;
                currentHitObject = null;
                zOffset = 0;
            }

            //ray casting end
            /////////////////////////////

            Vector3 mousePositionOffset =
                targetCamera.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance)) - originalScreenTargetPosition;

            

            //todo-ck tweak values
            //todo-ck update var names
            if (currentHitObject)
            {
                if (zOffset < 0 && zOffset > 0.1f)
                {
                    scrollWheelZOffset -= 0.0001f;
                }
                else if (zOffset < 0)
                {
                    scrollWheelZOffset -= 0.1f;
                }
                else if (zOffset > 0 && zOffset < 0.1f)
                {
                    scrollWheelZOffset += 0.0001f;
                }
                else if (zOffset > 0)
                {
                    scrollWheelZOffset += 0.1f;
                }

                Debug.DrawLine(currentHitObject.transform.position, selectedRigidbody.transform.position);
            } else
            {
               
            }
            

            Vector3 dragVectorForce = new Vector3
                (
                originalRigidbodyPosition.x + mousePositionOffset.x  - selectedRigidbody.transform.position.x,
                originalRigidbodyPosition.y + mousePositionOffset.y  - selectedRigidbody.transform.position.y,
                originalRigidbodyPosition.z + scrollWheelZOffset - selectedRigidbody.transform.position.z
                );



            selectedRigidbody.velocity = dragVectorForce * dragingForce * Time.deltaTime;


            
            Debug.Log("zoffset " + zOffset);

        }

        if (throwObject)
        { 
            selectedRigidbody.AddForce(new Vector3(0, 0, 1) * throwForce * Time.deltaTime, ForceMode.Impulse);
            throwObject = false;
            selectedRigidbody = null;
            resetOnMouseUp();
        }
    }

    private void OnDrawGizmos()
    {
        if (selectedRigidbody)
        {
            Gizmos.color = Color.red;
            Debug.DrawLine(origin, origin + direction * currentHitDistance);
            Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
            //Debug.Log("Sphere Radius: " + sphereRadius);
            
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
