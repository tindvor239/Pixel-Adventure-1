using UnityEngine;

public class EnemyController : Controller
{
    [SerializeField] float patrolDistance;
    [SerializeField] float targetRadius;
    [SerializeField] Transform target;
    [SerializeField] bool isPatrol = true;
    float moveAxis = 0;
    float Axis = 1;
    int count = 0;

    float patrolXPosition;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        patrolXPosition = StartPosition.x + patrolDistance;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(isPatrol)
        {
            Patrolling();
        }
        float distance = Vector2.Distance(transform.position, target.position);
        if(distance <= targetRadius)
        {
            isPatrol = false;
            if(transform.position.x <= target.position.x)
            {
                if (count == 0)
                {
                    moveAxis = 0;
                    count++;
                }
                moveAxis += Time.deltaTime;
                Moving(moveAxis);
            }
            else
            {
                if(count == 0)
                {
                    moveAxis = 0;
                    count++;
                }
                moveAxis -= Time.deltaTime;
                Moving(moveAxis);
            }
        }
        else
        {
            isPatrol = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, targetRadius);
    }

    void Moving(float moveAxis)
    {
        if (moveAxis >= 1f)
        {
            moveAxis = 1f;
        }
        else if (moveAxis <= -1f)
        {
            moveAxis = -1f;
        }

        float moveVelocity;
        moveVelocity = moveAxis * MoveSpeed * Time.deltaTime;
        MovingAnimation(moveVelocity);
        Turning(moveVelocity);
        Rigidbody.velocity = new Vector2(moveVelocity, Rigidbody.velocity.y);
    }

    void Patrolling()
    {
        moveAxis += Time.deltaTime * Axis;
        if(transform.position.x >= patrolXPosition)
        {
            patrolXPosition = StartPosition.x - patrolDistance;
            Axis = -1;
        }
        else
        {
            patrolXPosition = StartPosition.x + patrolDistance;
            Axis = 1;
        }
        Moving(moveAxis);
    }
}
