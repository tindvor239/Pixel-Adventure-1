using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : HitChecker
{
    [SerializeField] bool isHitEnemy;
    private GameObject enemy;
    private PlayerController playerController;

    #region Properties
    public GameObject Enemy
    {
        get { return enemy; }
    }
    public bool IsHitEnemy
    {
        get { return isHitEnemy; }
    }
    #endregion
    private void Start()
    {
        // to get player behavior
        if(GetComponentInParent<PlayerController>() != null)
        {
            playerController = GetComponentInParent<PlayerController>();
        }
    }

    #region Enemy Hit Detect
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && playerController.CanBeDamage) // player can be damage following hit delay time.
        {
            Debug.Log(string.Format("Player hit enemy at: {0}", Time.time));
            isHitEnemy = true;
            enemy = collision.gameObject;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && playerController.CanBeDamage)
        {
            isHitEnemy = false;
        }
    }
    #endregion
}
