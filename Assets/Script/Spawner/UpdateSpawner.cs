using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSpawner : Spawner
{
    [SerializeField] float spawnTime;

    GameObject currentSpawnObject;
    float spawnDelay;
    // Update is called once per frame
    void Update()
    {
        spawnDelay -= Time.deltaTime;
        if(spawnDelay <= 0f)
        {
            if(currentSpawnObject == null)
                currentSpawnObject = SpawnObject();
            spawnDelay = spawnTime;
        }
    }
}
