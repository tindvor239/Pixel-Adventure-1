using UnityEngine;

public class StartSpawner : Spawner
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            SpawnObject();
        }
    }
}
