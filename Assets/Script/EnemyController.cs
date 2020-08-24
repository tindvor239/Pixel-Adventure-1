using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyController : Controller
{
    [SerializeField] Transform target;
    [SerializeField] private Transform player;
    [SerializeField] Vector2 viewRange;
    [SerializeField] Transform[] destinations = new Transform[2];
    [SerializeField] float beenHitForce;
    bool isPatrol = true;
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
        Gizmos.DrawWireCube(transform.position, viewRange);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        SwitchTarget();
        if (player == null)
        {
            if(SceneMnger.instance.Player != null)
                player = SceneMnger.instance.Player.transform;
        }
        if (isPatrol)
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
            target = destinations[Random.Range(0, 1)];
        }
        else if(target.tag == "Player")
        {
            target = destinations[Random.Range(0, 1)];
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
            float distanceX = Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(player.position.x, 0));
            float distanceY = Vector2.Distance(new Vector2(0, transform.position.y), new Vector2(0, player.position.y));
            if (distanceX < viewRange.x / 2 && distanceY <= viewRange.y / 2)
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

    #region Properties
    public Transform Player
    {
        get { return player; }
    }

    public float BeenHitForce
    {
        get { return beenHitForce; }
    }
    #endregion
}
