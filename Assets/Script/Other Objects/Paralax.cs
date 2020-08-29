using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    [SerializeField] MeshRenderer[] backgrounds = new MeshRenderer[3];
    [SerializeField] PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            backgrounds[0].material.mainTextureOffset = new Vector2(player.gameObject.transform.position.x / 200, 0);
            backgrounds[1].material.mainTextureOffset = new Vector2(player.gameObject.transform.position.x / 400, 0);
            backgrounds[2].material.mainTextureOffset = new Vector2(player.gameObject.transform.position.x / 800, 0);
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
    }
}
