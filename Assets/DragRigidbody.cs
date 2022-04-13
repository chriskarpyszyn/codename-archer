using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRigidbody : MonoBehaviour
{
    /*
     * stolen from: https://sharpcoderblog.com/blog/drag-rigidbody-with-mouse-cursor-unity-3d-tutorial
     */

    public float forceAmount = 500;

    Rigidbody selectedRigidbody;
    Camera targetCamera;
    Vector3 originalScreenTargetPosition;
    Vector3 originalRigidbodyPosition;
    float selectionDistance;


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
        }

        if (Input.GetMouseButtonUp(0) && selectedRigidbody)
        {
            //Release selected Rigidbody if there is any
            selectedRigidbody = null;
        }
    }

    private void FixedUpdate()
    {
        //movement stuff here
        if (selectedRigidbody)
        {
            Vector3 mousePositionOffset = 
                targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance)) - originalScreenTargetPosition;
            selectedRigidbody.velocity = (originalRigidbodyPosition + mousePositionOffset - selectedRigidbody.transform.position) * forceAmount * Time.deltaTime;
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
