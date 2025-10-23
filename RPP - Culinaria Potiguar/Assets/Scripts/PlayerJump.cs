using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float initialJumpForce = 10f; 
    public float holdJumpForce = 5f;     
    public float maxJumpTime = 0.3f;     
    
    private Rigidbody2D rb;
    private Animator animator;  // NOVO
    private bool isGrounded;
    private bool isJumping;
    private float jumpTime;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();  // NOVO
    }
    
    void Update()
    {
        // Inicia o pulo
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, initialJumpForce);
            isJumping = true;
            jumpTime = 0f;
            isGrounded = false;
            
            animator.SetBool("EstaPulando", true);  // NOVO - Ativa animação de pulo
        }
        
        // Mantém o pulo pressionado
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
        
        // Solta o botão de pulo
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
            animator.SetBool("EstaPulando", false);  // NOVO - Desativa animação de pulo
        }
    }
}