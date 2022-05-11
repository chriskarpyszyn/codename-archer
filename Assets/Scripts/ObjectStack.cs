using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStack : MonoBehaviour
{
    List<GameObject> stackedObjects = new List<GameObject>();
    private int count = 0;
    public string countString;

    public void addToStack(GameObject gameObject)
    {
        stackedObjects.Add(gameObject);
        Debug.Log("Added!");
        count++;
    }

    public void removeFromStack(GameObject gameObject)
    {
        stackedObjects.Remove(gameObject);
        Debug.Log("Removed!");
        count--;
    }

    public int getCount()
    {
        return count;
    }

    //todo- method to get the last object in the stack so we can draw a count over it
    public GameObject getLastGameObject()
    {
        return stackedObjects[count - 1];
    }


}
