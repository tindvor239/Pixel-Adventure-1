using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    [SerializeField] bool isOnEnemy;
    private bool isGrounded = false;
    [SerializeField] float distance;
    private GameObject enemy;
    private Controller characterController;
    private PlayerController playerController;

    private void Start()
    {
        characterController = GetComponentInParent<Controller>();
        if(GetComponentInParent<PlayerController>() != null)
        {
            playerController = GetComponentInParent<PlayerController>();
        }
    }

    private void Update()
    {
        isGrounded = OnTerrain();

        if(playerController != null)
        {
            //isOnEnemy = OnEnemy();
        }
    }

    public GameObject Enemy
    {
        get { return enemy; }
        set { enemy = value; }
    }
    public bool IsOnEnemy
    {
        get { return isOnEnemy; }
        set { isOnEnemy = value; }
    }

    public bool IsGrounded
    {
        get { return isGrounded; }
    }

    bool OnTerrain()
    {
        Debug.DrawRay(transform.position, -transform.up, Color.red, distance);
        if (Physics2D.Raycast(transform.position, -transform.up, distance, LayerMask.GetMask("Terrain")))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && playerController.CanBeDamage)
        {
            isOnEnemy = true;
            enemy = collision.gameObject;
        }
    }
    /*
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && playerController.CanBeDamage)
        {
            isOnEnemy = false;
        }
    }*/
}
