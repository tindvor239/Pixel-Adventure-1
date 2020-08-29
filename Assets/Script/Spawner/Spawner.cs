using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject spawnObject;
    [SerializeField] Vector2 spawnOffset;
    public GameObject SpawnObject()
    {
        GameObject newObject;
        newObject = Instantiate(spawnObject);
        newObject.transform.position = new Vector2(transform.position.x + spawnOffset.x, transform.position.y + spawnOffset.y);
        return newObject;
    }
}
