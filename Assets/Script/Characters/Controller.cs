using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Feet))]
[RequireComponent(typeof(CharacterStats))]
public class Controller : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] bool isGrounded;
    public Feet feet;
    private Animator animator;
    private CharacterStats stats;

    public Animator Animator
    {
        get { return animator; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
    }
    public float JumpSpeed
    {
        get { return jumpSpeed; }
    }
    public bool IsGrounded
    {
        get { return isGrounded; }
    }
    public Rigidbody2D Rigidbody
    {
        get { return rigidbody; }
    }
    public CharacterStats Stats
    {
        get { return stats; }
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        feet = GetComponentInChildren<Feet>();
        animator = GetComponent<Animator>();
        stats = GetComponent<CharacterStats>();
    }

    public virtual void Update()
    {
        isGrounded = feet.IsHit;
    }
    public void Turning(float move)
    {
        if (move != 0)
        {
            transform.right = new Vector2(rigidbody.velocity.x, 0);
        }
    }

    public void MovingAnimation(float move)
    {
        if(isGrounded)
        {
            animator.SetFloat("speed", Mathf.Abs(move));
        }
    }

}
