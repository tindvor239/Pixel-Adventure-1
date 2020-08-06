using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    private bool isOnTerrain;
    private bool isOnEnemy;
    private GameObject enemy;
    private PlayerController player;

    private void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    public GameObject Enemy
    {
        get { return enemy; }
        set { enemy = value; }
    }
    public bool IsOnTerrain
    {
        get { return isOnTerrain; }
        set { isOnTerrain = value; }
    }

    public bool IsOnEnemy
    {
        get { return isOnEnemy; }
        set { isOnEnemy = value; }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isOnTerrain = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && player.CanBeDamage)
        {
            player.ISCollide = true;
            isOnEnemy = true;
            enemy = collision.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isOnTerrain = false;
            enemy = null;
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            isOnEnemy = false;
            player.ISCollide = false;
        }
    }
}
