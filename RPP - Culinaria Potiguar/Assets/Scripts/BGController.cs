using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class BGController : MonoBehaviour
{

    public List<Sprite> backgrounds;
    
    private SpriteRenderer spriteRenderer;
    public float baseSpeed = 0f; // Velocidade base do objeto
    public float speedToSizeMultiplier = 1f;
    private Vector2 baseSize;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseSize = spriteRenderer.size;
    }

    void Update()
    {
       // Velocidade = velocidade base * multiplicador global
        float currentSpeed = speedToSizeMultiplier * (baseSpeed + GameManager.Instance.speedBase) * GameManager.Instance.speedMultiplier;
        spriteRenderer.size = new Vector2(spriteRenderer.size.x + currentSpeed, baseSize.y);
    }
}
