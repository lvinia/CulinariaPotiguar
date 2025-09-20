using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float initialJumpForce = 10f; 
    public float holdJumpForce = 5f;     
    public float maxJumpTime = 0.3f;     

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isJumping;
    private float jumpTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, initialJumpForce);
            isJumping = true;
            jumpTime = 0f;
            isGrounded = false;
        }

        
        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTime < maxJumpTime)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, holdJumpForce);
                jumpTime += Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}