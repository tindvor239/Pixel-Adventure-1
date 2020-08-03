using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Controller : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] bool isGrounded;
    [SerializeField] Animator animator;
    private Vector2 startScale;
    private Vector2 startPosition;
    public Animator Animator
    {
        get { return animator; }
        set { animator = value; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    public float JumpSpeed
    {
        get { return jumpSpeed; }
        set { jumpSpeed = value; }
    }
    public bool IsGrounded
    {
        get { return isGrounded; }
        set { isGrounded = value; }
    }
    public Rigidbody2D Rigidbody
    {
        get { return rigidbody; }
        set { rigidbody = value; }
    }
    public Vector2 StartScale
    {
        get { return startScale; }
        set { startScale = value; }
    }

    public Vector2 StartPosition
    {
        get { return startPosition; }
        set { startPosition = value; }
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        startScale = transform.localScale;
        animator = GetComponent<Animator>();
        StartPosition = transform.position;
    }
    public void Turning(float move)
    {
        if (rigidbody.velocity.x <= -0.01f)
        {
            transform.localScale = new Vector2(-startScale.x, transform.localScale.y);
        }
        else if (rigidbody.velocity.x >= 0.01f)
        {
            transform.localScale = new Vector2(startScale.x, transform.localScale.y);
        }
    }

    public void MovingAnimation(float move)
    {
        if(isGrounded)
        {
            animator.SetFloat("speed", Mathf.Abs(move));
        }
    }


    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            IsGrounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            IsGrounded = false;
        }
    }

}
