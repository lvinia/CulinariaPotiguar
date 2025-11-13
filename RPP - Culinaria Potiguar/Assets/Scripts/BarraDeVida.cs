using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    public Slider sliderVidasRestantes;
    public PlayerLife playerLife; // Mudou aqui
    
    [SerializeField]
    private int vidasRestantes = 0;
    
    void Start()
    {
        if (playerLife != null)
        {
            if (sliderVidasRestantes != null)
            {
                sliderVidasRestantes.minValue = 0;
                sliderVidasRestantes.maxValue = playerLife.vidas; // Acessa a vida inicial
            }
        }
    }
    
    void Update()
    {
        if (sliderVidasRestantes != null && playerLife != null)
        {
            vidasRestantes = playerLife.vidas; // Acessa diretamente a variável pública
            sliderVidasRestantes.value = vidasRestantes;
        }
    }
}