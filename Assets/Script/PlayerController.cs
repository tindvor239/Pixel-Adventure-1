using UnityEngine;

public class PlayerController : Controller
{
    // Start is called before the first frame update
    [SerializeField] Side side;
    [SerializeField] Feet feet;
    [SerializeField] bool isSlideWallJumping;
    [SerializeField] bool canBeDamage;
    bool isCollide;
    bool isBlinking = false;
    float blinkTime = 0.2f;
    int blinkCount = 0;
    bool isBlink = true;
    float move = 0;
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        canBeDamage = GetCanBeDamage();
        if (side.IsWallSliding == false && IsGrounded)
        {
            Jump();
        }
        else if (side.IsWallSliding && IsGrounded)
        {
            WallJump();
        }
        Animator.SetBool("isWallJump", side.IsWallSliding);
        Animator.SetBool("isJumping", !IsGrounded);
        if (side.IsWallSliding == false)
        {
            move = Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime;
            Rigidbody.velocity = new Vector2(move, Rigidbody.velocity.y);
            Turning(move);
        }
        else
        {
            if (isSlideWallJumping == false) // when wall sliding.
            {
                if (Input.GetAxisRaw("Horizontal") != Mathf.Sign(transform.localScale.x)) // if input oposite with scale it will cancel wall sliding.
                {
                    side.IsWallSliding = false;
                }
                else
                {
                    Turning(Rigidbody.velocity.x);
                }
            }
        }

        if (feet.IsOnTerrain) // if when you wall sliding and touch the ground it will cancel wall sliding.
        {
            side.IsWallSliding = false;
        }

        if (isCollide) // is collide mean collding with enemy or trap.
        {
            Rigidbody.velocity = transform.up * 3f;
        }

        if (feet.IsOnEnemy)
        {
            Destroy(feet.Enemy);
        }

        if (isBlinking)
        {
            Blinking(ref blinkTime, ref blinkCount, ref isBlink);
        }
        MovingAnimation(move);

    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody.velocity = transform.up * JumpSpeed;
        }
    }

    void WallJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSlideWallJumping = true;
            Rigidbody.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * JumpSpeed, 8);
            IsGrounded = false;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isSlideWallJumping = false;
        }
    }

    void Blinking(ref float blinkTime, ref int blinkCount, ref bool isBlink)
    {
        blinkTime -= Time.deltaTime;
        if (blinkTime <= 0.0f && blinkCount <= 3)
        {
            isBlink = !isBlink;
            gameObject.GetComponent<SpriteRenderer>().enabled = isBlink;
            if (isBlink == false)
            {
                blinkCount++;
            }
            blinkTime = 0.2f;
        }
        if (blinkCount > 3) // reset to normal
        {
            isBlink = true;
            blinkCount = 0;
            gameObject.GetComponent<SpriteRenderer>().enabled = isBlink;
            isBlinking = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            Destroy(collision.gameObject);
        }
    }

    public bool CanBeDamage
    {
        get { return canBeDamage = GetCanBeDamage(); }
    }

    #region Can Be Damage Handler
    bool GetCanBeDamage()
    {
        if (isCollide == false && isBlinking == false)
            return true;
        else
            return false;
    }
    public void SetCanBeDamage(bool value)
    {
        isBlinking = value;
        isCollide = value;
    }
    public bool ISCollide
    {
        get { return isCollide; }
        set { isCollide = value; }
    }
    #endregion
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trap" && canBeDamage)
        {
            SetCanBeDamage(true);
        }
    }
    public override void OnCollisionExit2D(Collision2D collision)
    {
        base.OnCollisionExit2D(collision);
        if (collision.gameObject.tag == "Trap")
        {
            isCollide = false;// fix on enemy too.
        }
    }

}
