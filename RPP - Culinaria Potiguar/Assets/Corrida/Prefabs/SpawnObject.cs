using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    private Rigidbody2D rb;
    public float baseSpeed = 5f; // Velocidade base do objeto
    
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        
        // Destruir após 6 segundos
        if (timer > 10)
        {
            Destroy(gameObject);
        }

        // Velocidade = velocidade base * multiplicador global
        float currentSpeed = baseSpeed * GameManager.Instance.speedMultiplier;
        rb.linearVelocity = Vector2.left * currentSpeed;
    }
}