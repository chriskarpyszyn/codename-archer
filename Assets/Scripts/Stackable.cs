using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stackable : MonoBehaviour
{
 
    private bool isStacked = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Stackable")
        {
            if (!isStacked)
            {
                // if this is not stacked, then add to the ObjectStack list
            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Stackable")
        {
            if (isStacked)
            {
                // if this is no longer stacked, remove from the object stack
            }
        }
    }

}
