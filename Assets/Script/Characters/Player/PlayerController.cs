using UnityEngine;
using UnityEngine.UI;
public class PlayerController : Controller
{
    // Start is called before the first frame update.
    [SerializeField] HitChecker side;
    [SerializeField] bool canBeDamage;
    [SerializeField] float currentHitDelay;
    [SerializeField] Slider healthBar;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip movingSound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip eatFruitSound;
    [SerializeField] EnemyController enemy;
    SceneController sceneController;
    // Action Behaviors.
    [SerializeField]  float wallSlidingSpeed;
    bool isWallSliding;

    sbyte score;
    float hitDelay;
    bool isBlinking = false;
    float blinkTime = 0.2f;
    int blinkCount = 0;
    bool isBlink = true;

    #region Properties
    public sbyte Score
    {
        get { return score; }
    }
    #endregion
    private void Awake()
    {
        hitDelay = currentHitDelay;
        audioSource = GetComponent<AudioSource>();
        Screen.SetResolution(1920, 1080, true, 30);

    }
    public override void Start()
    {
        sceneController = SceneController.instance;
        base.Start();
        // Check game object healthbar.
        if (GameObject.FindGameObjectWithTag("PlayerHealth") != null)
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
        if (sceneController.gameState == SceneController.GameState.Play)
        {
            base.Update();
            Move();
            #region Update values per frame
            //Update behaviors of player.
            isWallSliding = side.IsHit;

            //Update player damage behaviors
            currentHitDelay -= Time.deltaTime;
            canBeDamage = GetCanBeDamage();

            //Update player stats to UI.
            healthBar.value = Stats.HP;
            #endregion

            if (isWallSliding == false && IsGrounded) // if you are not sliding and in the ground you can normal jump.
            {
                Jump();
            }
            else if (isWallSliding && IsGrounded == false) // if you are sliding and not touching the ground then jump like clamping the wall.
            {
                WallJump();
            }
            Animator.SetBool("isWallJump", isWallSliding);
            Animator.SetBool("isJumping", !IsGrounded);
        
            if (IsGrounded) // if when you wall sliding and touch the ground it will cancel wall sliding.
            {
                // somehow disable side object for more reasonable.
                // becuz when character hit the ground and still sliding wall character will keep sliding instead switch to ground state.
                // then solution is enable gameobject and disable.
                side.gameObject.SetActive(false);
                side.IsHit = false;
            }
            else
            {
                side.gameObject.SetActive(true);
            }
            if (feet.IsHitEnemy)
            {
                // when hit enemy add force up.
                Rigidbody.AddForce(transform.up * feet.Enemy.BeenHitForce, ForceMode2D.Impulse);
                if (currentHitDelay <= 0.0f)
                {
                    if(feet.Enemy.gameObject.GetComponent<EnemyController>())
                    {
                        EnemyController enemy = feet.Enemy.gameObject.GetComponent<EnemyController>();
                        enemy.Stats.HP -= Stats.Damage;
                    }
                    currentHitDelay = hitDelay;
                }
            }

            if(isBlinking)
            {
                Rigidbody.AddForce(transform.up / 8f, ForceMode2D.Impulse);
            }

            if (isBlinking) // excecute blinking effect.
            {
                Blinking(ref blinkTime, ref blinkCount, ref isBlink);
            }
        }
    }

    #region Movement Handler
    private void Move()
    {
        float velocityX;
        float velocityY;
        if (isWallSliding == false)
        {
            Rigidbody.gravityScale = 1f;
            velocityX = Input.GetAxisRaw("Horizontal") * MoveSpeed * Time.deltaTime;
            velocityY = Rigidbody.velocity.y;
        }
        else
        {
            Rigidbody.gravityScale = 0.3f;
            velocityX = 0;
            velocityY = Mathf.Clamp(Rigidbody.velocity.y, -wallSlidingSpeed, float.MaxValue);

            // if player input diffent direction.
            if (Input.GetAxisRaw("Horizontal") > 0 && transform.eulerAngles.y >= 180 || Input.GetAxisRaw("Horizontal") < 0 && transform.eulerAngles.y <= 0)
            {
                velocityX = Input.GetAxisRaw("Horizontal") * MoveSpeed * Time.deltaTime;
                Debug.Log(string.Format("Velocity: {0}", velocityX));
            }
        }
        Rigidbody.velocity = new Vector2(velocityX, velocityY);
        MovingAnimation(velocityX);
        Turning(velocityX);
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

            Rigidbody.AddForce(new Vector2(30.0f * -Input.GetAxisRaw("Horizontal"), 10.0f), ForceMode2D.Impulse);
        }
    }
    #endregion
    #region Can Be Damage Handler
    public bool CanBeDamage
    {
        get { return canBeDamage = GetCanBeDamage(); }
    }
    bool GetCanBeDamage()
    {
        return isBlinking == false ? true : false;
    }
    public void SetCanBeDamage(bool value)
    {
        isBlinking = value;
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
    #endregion
    #region Collider Handler
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            score += 1;
            Stats.HP += 5;
            audioSource.PlayOneShot(eatFruitSound);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Finish")
        {
            SceneController.instance.gameState = SceneController.GameState.Finish;
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
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trap" && canBeDamage)
        {
            Stats.HP = 0;
        }
        else if (collision.gameObject.tag == "Enemy" && canBeDamage && feet.IsHitEnemy == false)
        {
            score += 2;
            enemy = collision.gameObject.GetComponent<EnemyController>();
            // player been hit sound
            Debug.Log(string.Format("Player BEEN hit enemy at: {0}", Time.time));
            audioSource.PlayOneShot(hitSound);
            SetCanBeDamage(true);
            Stats.HP -= enemy.Stats.Damage;
        }
    }
    #endregion
}
