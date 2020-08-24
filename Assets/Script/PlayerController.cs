using UnityEngine;
using UnityEngine.UI;
public class PlayerController : Controller
{
    // Start is called before the first frame update.
    [SerializeField] Side side;
    [SerializeField] bool canBeDamage;
    [SerializeField] float currentHitDelay;
    [SerializeField] Slider healthBar;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip movingSound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip eatFruitSound;
    [SerializeField] EnemyController enemy;

    float hitDelay;
    bool isBlinking = false;
    float blinkTime = 0.2f;
    int blinkCount = 0;
    bool isBlink = true;
    float move = 0;

    private void Awake()
    {
        hitDelay = currentHitDelay;
        audioSource = GetComponent<AudioSource>();
        
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
        
        // on NOT sliding wall.
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
            // when hit enemy add force up.
            Rigidbody.AddForce(new Vector2(transform.position.x, transform.position.y + enemy.BeenHitForce), ForceMode2D.Impulse);
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
            // jump sound.
            audioSource.PlayOneShot(jumpSound);

            Rigidbody.AddForce(transform.up * JumpSpeed, ForceMode2D.Impulse);
        }
    }

    void WallJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // jump sound.
            audioSource.PlayOneShot(jumpSound);

            if(transform.eulerAngles.y >= 180)
            {
                Rigidbody.AddForce(new Vector2(transform.position.x + 40.0f, transform.position.y + 18.0f), ForceMode2D.Impulse);
            }
            else
            {
                Rigidbody.AddForce(new Vector2(transform.position.x - 40.0f, transform.position.y + 25.0f), ForceMode2D.Impulse);
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
            audioSource.PlayOneShot(eatFruitSound);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Finish")
        {
            SceneMnger.instance.gameState = SceneMnger.GameState.Finish;
        }
        else if (collision.gameObject.tag == "Bullet" && canBeDamage)
        {
            Destroy(collision.gameObject);
            // play been hit sound.
            audioSource.PlayOneShot(hitSound);
            // deal damage
            SetCanBeDamage(true);
            Stats.HP -= 5;
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
            enemy = collision.gameObject.GetComponent<EnemyController>();
            // player been hit sound
            audioSource.PlayOneShot(hitSound);
            SetCanBeDamage(true);
            Stats.HP -= enemy.Stats.Damage;
        }
    }
}
