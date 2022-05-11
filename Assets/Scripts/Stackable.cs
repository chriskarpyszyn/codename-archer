using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stackable : MonoBehaviour
{
 
    private bool stacked = false;
    private ObjectStack objectStack;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Stackable")
        {
            if (!stacked)
            {
                //todo-ck add a delay before considering something stacked (how long something has been in contact)
                //todo-ck maybe we can check the "velocity" of the object, so do not stack if moving.

                //todo-ck what is this? 
                //You are trying to create a MonoBehaviour using the 'new' keyword.  This is not allowed.  MonoBehaviours can only be added using AddComponent(). Alternatively, your script can inherit from ScriptableObject or no base class at all

                //if the object I'm colliding with is also not stacked, we're going to create our object stack
                //because we're the first two objects being stacked
                if (!collision.gameObject.GetComponent<Stackable>().isStacked())
                {
                    Debug.Log("Not stacked");
                    objectStack = new ObjectStack();
                    //objectStack.addToStack(collision.gameObject);
                    objectStack.addToStack(this.gameObject);
                    FindObjectOfType<GameManager>().addToStackableList(objectStack);
                    
                }
                else  //if the object I'm colliding with is already stacked, I will want to get a reference to his object stack
                {
                    Debug.Log("Stacked");
                    objectStack = collision.gameObject.GetComponent<Stackable>().getObjectStack();
                    objectStack.addToStack(this.gameObject);
                }


                stacked = true;
                Debug.Log(objectStack.getCount());
            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Stackable")
        {
            if (stacked)
            {

                //look at list count, if the other object's place in the list does not match the count, then do nothing with it since
                //this is not the object being removed

                //if the list count and the place in the list matches, then remove from the list and remove reference





                    //Debug.Log("Object Stack Get Count on Remove " + objectStack.getCount());
                    //// if this is no longer stacked, remove from the object stack and remove the reference from this 
                    //// todo-ck this if statement doesn't work.
                    //if (objectStack.getCount()>1)
                    //{
                    //    //how do i stop this from running twice 

                    //    if (collision.gameObject.GetComponent<Stackable>().objectStack != null)
                    //    objectStack.removeFromStack(collision.gameObject);

                    //    objectStack.removeFromStack(this.gameObject);
                    //    this.objectStack = null;
                    //} else 
                    //{
                    //    Debug.Log("Last Two");
                    //    // if this is the last two objects being removed, then we will want to remove all references of this objectstack
                    //    // (from me, him and gamemanager)

                    //    objectStack.removeFromStack(this.gameObject);
                    //    FindObjectOfType<GameManager>().removeFromStackableList(objectStack);
                    //    objectStack = null;
                    //    //todo-ck do i need to garbage collect this instance as well
                    //}



                    //stacked = false;
                    //if (objectStack != null)
                    //{
                    //    Debug.Log(objectStack.getCount());
                    //}
                }
        }
    }

    public bool isStacked()
    {
        return this.stacked;
    }

    public ObjectStack getObjectStack()
    {
        return this.objectStack;
    }
}
