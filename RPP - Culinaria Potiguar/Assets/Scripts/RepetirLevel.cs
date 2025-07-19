using UnityEngine;

public class RepetirLevel : MonoBehaviour
{
    public Transform[] blocos;
    public float tamanhoDoBloco = 20f;

    private Transform jogador;

    private void Start()
    {
        jogador = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    private void Update()
    {
        foreach (Transform bloco in blocos)
        {
            if (bloco.position.x + tamanhoDoBloco < jogador.position.x)
            {
                float posMaisADireita = bloco.position.x;
                foreach (Transform b in blocos)
                {
                    if (b.position.x > posMaisADireita)
                    {
                        posMaisADireita = b.position.x;
                    }
                }
                bloco.position = new Vector3(posMaisADireita + tamanhoDoBloco, bloco.position.y, bloco.position.z);
            }
        }
    }
}
