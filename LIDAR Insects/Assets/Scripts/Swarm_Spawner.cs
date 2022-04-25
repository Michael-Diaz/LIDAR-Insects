using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm_Spawner : MonoBehaviour {

    public Swarm_SingleMovement prefab;
    public float spawnRadius = 10;
    public int spawnCount = 10;

    void Awake () {
        for (int i = 0; i < spawnCount; i++) {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            Swarm_SingleMovement boid = Instantiate (prefab);
            boid.transform.position = pos;
            boid.transform.forward = Random.insideUnitSphere;
        }
    }
}