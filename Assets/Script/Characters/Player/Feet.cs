using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : HitChecker
{
    [SerializeField] bool isHitEnemy;
    private EnemyController enemy;
    private PlayerController playerController;

    #region Properties
    public EnemyController Enemy
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
        if (collision.gameObject.tag == "Enemy" && playerController.CanBeDamage && collision.transform.position.y < transform.position.y) // player can be damage following hit delay time.
        {
            isHitEnemy = true;
            enemy = collision.GetComponent<EnemyController>();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            isHitEnemy = false;
        }
    }
    #endregion
}
