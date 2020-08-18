using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.FindGameObjectWithTag("Player") == null)
        {
            GameObject newPlayer;
            newPlayer = Instantiate(player);
            print("spawned");
            newPlayer.transform.position = new Vector2(transform.position.x + 2f, transform.position.y + 2f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
