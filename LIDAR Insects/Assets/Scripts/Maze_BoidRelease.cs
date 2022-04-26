using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_BoidRelease : MonoBehaviour
{
    public GameObject targetPrefab;
    public GameObject swarmPrefab;

    private bool isTriggered = false;

    // Start is called before the first frame update
    void OnTriggerEnter()
    {
        if (!isTriggered)
        {
            Instantiate(targetPrefab, new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity);
            Instantiate(swarmPrefab, new Vector3(-1.0f, 1.0f, -1.0f), Quaternion.identity);
            isTriggered = true;
        }
    }
}
