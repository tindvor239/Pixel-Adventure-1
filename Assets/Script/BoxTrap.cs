using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTrap : MonoBehaviour
{
    [SerializeField] float xOffset;
    [SerializeField] float y;

    Rigidbody2D rigidbody;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0.0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(xOffset, y, 1));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(transform.position.y > collision.transform.position.y && Mathf.Abs(transform.position.x - collision.transform.position.x) <= xOffset)
            {
                if(collision.gameObject.GetComponent<PlayerController>().CanBeDamage)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rigidbody.gravityScale = 1.0f;
        }
    }
}
