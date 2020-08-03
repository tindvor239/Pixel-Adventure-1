using UnityEngine;

public class PlayerController : Controller
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime;
        Rigidbody.velocity = new Vector2(move, Rigidbody.velocity.y);
        Turning(move);
        MovingAnimation(move);
        Jump();
        Animator.SetBool("isJumping", !IsGrounded);
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            Rigidbody.AddForce(transform.up * JumpSpeed);
            IsGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Item")
        {
            Destroy(collider.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

}
