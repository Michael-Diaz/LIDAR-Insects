using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_OrbitLock : MonoBehaviour
{
    GameObject player;
    Transform playerVectors;

    Vector3 ringPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Spider_PH");
        playerVectors = player.GetComponent<Transform>();

        ringPos = new Vector3(0.0f, 1.5f, -2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        ringPos = new Vector3((Mathf.Sin(playerVectors.eulerAngles.y * Mathf.Deg2Rad) * -2) + playerVectors.position.x, 1.5f, (Mathf.Cos(playerVectors.eulerAngles.y * Mathf.Deg2Rad) * -2) + playerVectors.position.z);

        gameObject.transform.position = ringPos;
        gameObject.transform.localRotation = Quaternion.Euler(15, playerVectors.eulerAngles.y, 0);
    }
}
