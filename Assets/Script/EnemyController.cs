using UnityEngine;

public class EnemyController : Controller
{
    [SerializeField] Transform target;
    [SerializeField] Transform player;
    [SerializeField] float distance;
    [SerializeField] Transform[] destinations = new Transform[2];
    [SerializeField] bool isPatrol = true;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        foreach(Transform destination in destinations)
        {
            destination.parent = null;
        }
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distance);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchTarget();
        if(isPatrol)
        {
            Patrolling();
        }
        else
        {
            if(target == null)
            {
                isPatrol = true;
            }
            else
            {
                Following();
            }
        }
        
    }

    void Moving(float moveAxis)
    {
        MovingAnimation(moveAxis * MoveSpeed * Time.deltaTime);
        Turning(moveAxis * MoveSpeed * Time.deltaTime);
        Rigidbody.velocity = new Vector2(moveAxis * MoveSpeed * Time.deltaTime, Rigidbody.velocity.y);
    }

    void Patrolling()
    {
        if(target == null)
        {
            target = target = destinations[Random.Range(0, 1)];
        }
        Vector2 facing = target.position - transform.position;
        float moveAxis = Mathf.Sign(facing.x);
        if(target == destinations[0] && transform.position.x <= target.position.x)
        {
            target = destinations[1];
        }
        else if(target == destinations[1] && transform.position.x >= target.position.x)
        {
            target = destinations[0];
        }
        Moving(moveAxis);
    }

    void Following()
    {
        Vector2 facing = target.position - transform.position;
        float moveAxis = Mathf.Sign(facing.x);
        Moving(moveAxis);
    }

    void SwitchTarget()
    {
        if(player != null)
        {
            if(Vector2.Distance(transform.position, player.position) <= distance)
            {
                isPatrol = false;
                target = player;
            }
            else
            {
                isPatrol = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print(collision.gameObject.name);
            Destroy(collision.gameObject);
        }
    }

}
