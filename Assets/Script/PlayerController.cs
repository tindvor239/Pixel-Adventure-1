using UnityEngine;
using UnityEngine.UI;
public class PlayerController : Controller
{
    // Start is called before the first frame update.
    [SerializeField] Side side;
    [SerializeField] bool canBeDamage;
    [SerializeField] float currentHitDelay;
    [SerializeField] Slider healthBar;
    float hitDelay;
    bool isBlinking = false;
    float blinkTime = 0.2f;
    int blinkCount = 0;
    bool isBlink = true;

    private void Awake()
    {
        hitDelay = currentHitDelay;
        print(hitDelay);
    }
    public override void Start()
    {
        base.Start();

        if(GameObject.FindGameObjectWithTag("PlayerHealth") != null)
        {
            GameObject healthBarObj = GameObject.FindGameObjectWithTag("PlayerHealth");
            healthBar = healthBarObj.GetComponent<Slider>();
            healthBar.maxValue = Stats.MaxHp;
        }
        else
        {
            print("Are you sure that u already add healthbar???");
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        float move = 0;
        currentHitDelay -= Time.deltaTime;
        canBeDamage = GetCanBeDamage();
        healthBar.value = Stats.HP;

        if (side.IsWallSliding == false && IsGrounded) // if you are not sliding and in the ground you can normal jump.
        {
            Jump();
        }
        else if (side.IsWallSliding && IsGrounded == false) // if you are sliding and not touching the ground then jump like clamping the wall.
        {
            WallJump();
        }
        Animator.SetBool("isWallJump", side.IsWallSliding);
        Animator.SetBool("isJumping", !IsGrounded);
        
        if (side.IsWallSliding == false)
        {
            Rigidbody.gravityScale = 1f;
            move = Move();
        }
        else
        {
            Rigidbody.gravityScale = 0.3f;
            if(Input.GetAxisRaw("Horizontal") > 0 && transform.eulerAngles.y >= 180 || Input.GetAxisRaw("Horizontal") < 0 && transform.eulerAngles.y <= 0)
            {
                move = Move();
            }
        }

        if (IsGrounded) // if when you wall sliding and touch the ground it will cancel wall sliding.
        {
            // somehow disable side script for more reasonable.
            // becuz when character hit the ground and jump to slide wall you must move character collider outside the wall and move character back the wall again to slide.
            // then solution is enable gameobject and disable.
            side.gameObject.SetActive(false);
            side.IsWallSliding = false;
        }
        else
        {
            side.gameObject.SetActive(true);
        }

        if (feet.IsOnEnemy)
        {
            Rigidbody.AddForce(new Vector2(transform.position.x, transform.position.y + 35.0f), ForceMode2D.Impulse);
            if (currentHitDelay <= 0.0f)
            {
                if(feet.Enemy.gameObject.GetComponent<EnemyController>())
                {
                    EnemyController enemy = feet.Enemy.gameObject.GetComponent<EnemyController>();
                    enemy.Stats.HP -= Stats.Damage;
                }
                currentHitDelay = hitDelay;
                feet.IsOnEnemy = false;
            }
        }

        if(isBlinking)
        {
            Rigidbody.AddForce(transform.up / 5f, ForceMode2D.Impulse);
        }

        if (isBlinking) // excecute blinking effect.
        {
            Blinking(ref blinkTime, ref blinkCount, ref isBlink);
        }
        MovingAnimation(move);
    }

    private float Move()
    {
        float move = Input.GetAxisRaw("Horizontal") * MoveSpeed * Time.deltaTime;
        Rigidbody.velocity = new Vector2(move, Rigidbody.velocity.y);
        Turning(move);
        return move;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody.AddForce(transform.up * JumpSpeed, ForceMode2D.Impulse);
        }
    }

    void WallJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(transform.eulerAngles.y >= 180)
            {
                Rigidbody.AddForce(new Vector2(transform.position.x + 50.0f, transform.position.y + 25.0f), ForceMode2D.Impulse);
            }
            else
            {
                Rigidbody.AddForce(new Vector2(transform.position.x - 50.0f, transform.position.y + 25.0f), ForceMode2D.Impulse);
            }
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
        else if (collision.gameObject.tag == "Finish")
        {
            SceneMnger.instance.gameState = SceneMnger.GameState.Finish;
        }
    }
    
    public bool CanBeDamage
    {
        get { return canBeDamage = GetCanBeDamage(); }
    }
    #region Can Be Damage Handler
    bool GetCanBeDamage()
    {
        if (isBlinking == false)
            return true;
        else
            return false;
    }
    public void SetCanBeDamage(bool value)
    {
        isBlinking = value;
    }
    #endregion
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trap" && canBeDamage)
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Enemy" && canBeDamage)
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            SetCanBeDamage(true);
            Stats.HP -= enemy.Stats.Damage;
        }
    }
}
