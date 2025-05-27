using UnityEngine;

public class PlayerController : MonoBehaviour
{ 
    public float speed = 5f;         // Velocidade para correr para a direita
    public float jumpForce = 7f;     // Força do pulo
    public Transform groundCheck;    // Ponto para verificar se está no chão
    public float groundRadius = 0.2f; // Raio para checar o chão
    public LayerMask groundLayer;    // Layer do chão

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Checa se está no chão usando OverlapCircle
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        // Pular se estiver no chão e apertar espaço
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Corre sempre para a direita
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    // Para visualizar o groundCheck no editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
