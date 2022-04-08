using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject cubePrefab;
    public GameObject spherePrefab;
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
            int randomInt = (int)Mathf.Ceil(Random.Range(0.1f, 2f));
            Vector3 instPos = new Vector3(0, 1, 0);

            //todo-ck add code to never be zero
            float randomX = Random.Range(-2f, 2f);
            float randomZ = Random.Range(-2f, 2f);

            if (randomInt == 1)
            {
                GameObject cube = Instantiate(cubePrefab, instPos, Quaternion.identity);
                cube.GetComponent<Rigidbody>().AddForce(randomX, instForce, randomZ, ForceMode.Impulse);
            } else
            {
                GameObject sphere = Instantiate(spherePrefab, instPos, Quaternion.identity);
                sphere.GetComponent<Rigidbody>().AddForce(randomX, instForce, randomZ, ForceMode.Impulse);
            }
            
        }
        
    }

    private bool IsMouseButtonClick()
    {
        return (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2));
    }
}
