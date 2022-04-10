using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject cubePrefab;
    public GameObject spherePrefab;
    public GameObject prismPrefab;
    public int instForce = 10;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
     
        if (Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        if (Input.anyKeyDown && !IsMouseButtonClick())
        {
            int randomInt = (int)Mathf.Ceil(Random.Range(0.1f, 3f));
            Vector3 instPos = new Vector3(0, 1, 0);



            if (randomInt == 1)
            {
                GameObject cube = Instantiate(cubePrefab, instPos, Quaternion.identity);
                AddInstantiationForce(cube);
            } else if (randomInt == 2)
            {
                GameObject sphere = Instantiate(spherePrefab, instPos, Quaternion.identity);
                AddInstantiationForce(sphere);
            } else
            {
                GameObject prism = Instantiate(prismPrefab, instPos, Quaternion.identity);
                AddInstantiationForce(prism);
            }
            
        }
        
    }

    /// <summary>
    /// Check if the input given is a mouse click.
    /// </summary>
    /// <returns>True if this is a mouse click.</returns>
    private bool IsMouseButtonClick()
    {
        return (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2));
    }

    /// <summary>
    /// Add's an force at the moment of instantiation.
    /// </summary>
    /// <param name="gameObject">The Game Object to add the force to.</param>
    private void AddInstantiationForce(GameObject gameObject)
    {
        //todo-ck add code to never be zero
        float randomX = Random.Range(-2f, 2f);
        float randomZ = Random.Range(-2f, 2f);
        gameObject.GetComponent<Rigidbody>().AddForce(randomX, instForce, randomZ, ForceMode.Impulse);
    }
}
