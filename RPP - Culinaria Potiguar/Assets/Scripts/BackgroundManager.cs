using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

[System.Serializable]
public class FaseData
{
    public string nome;
    public GameObject prefab;
}

public class BackgroundManager : MonoBehaviour
{
    [Header("Fases e Prefabs")]
    public List<FaseData> fases;
    public GameObject muroPrefab;
    public GameObject barPrefab;

    [Header("Camera e Movimento")]
    public Transform cameraTransform;
    public float velocidadeScroll = 2f;

    [Header("Escala Aleatória (apenas largura)")]
    public float minWidthMultiplier = 1.1f;
    public float maxWidthMultiplier = 1.5f;

    [Header("Remoção e Pool")]
    public float distanciaParaRemover = 40f;

    private Dictionary<string, ObjectPool<GameObject>> pools = new();
    private List<GameObject> ativos = new();
    private float posicaoXAtual = 0f;
    private FaseData ultimaFaseGerada;

    void Start()
    {
        // Cria pools para as fases
        foreach (var fase in fases)
        {
            pools[fase.nome] = new ObjectPool<GameObject>(
                () => Instantiate(fase.prefab),
                obj => obj.SetActive(true),
                obj => obj.SetActive(false),
                obj => Destroy(obj),
                true, 3, 10
            );
        }

        // Cria pool do muro
        pools["muro"] = new ObjectPool<GameObject>(
            () => Instantiate(muroPrefab),
            obj => obj.SetActive(true),
            obj => obj.SetActive(false),
            obj => Destroy(obj),
            true, 2, 6
        );

        // Cria pool do bar
        pools["bar"] = new ObjectPool<GameObject>(
            () => Instantiate(barPrefab),
            obj => obj.SetActive(true),
            obj => obj.SetActive(false),
            obj => Destroy(obj),
            true, 1, 3
        );

        // Gera a primeira fase imediatamente visível
        for (int i = 0; i < 4; i++)
            GerarNovaFase();
    }

    void Update()
    {
        float deslocamento = velocidadeScroll * Time.deltaTime;

        // Move o fundo
        for (int i = ativos.Count - 1; i >= 0; i--)
        {
            GameObject obj = ativos[i];
            if (obj != null)
            {
                obj.transform.position -= new Vector3(deslocamento, 0f, 0f);

                // Remove o que saiu da tela
                if (obj.transform.position.x < cameraTransform.position.x - distanciaParaRemover)
                {
                    string chave = obj.name.Replace("(Clone)", "").Trim();
                    if (pools.ContainsKey(chave))
                        pools[chave].Release(obj);
                    else
                        obj.SetActive(false);

                    ativos.RemoveAt(i);
                }
            }
        }

        // Sempre manter fases à frente da câmera
        float limite = cameraTransform.position.x + Camera.main.orthographicSize * Camera.main.aspect + 30f;
        while (posicaoXAtual < limite)
        {
            GerarNovaFase();
        }
    }

    void GerarNovaFase()
    {
        if (fases.Count == 0) return;

        // Escolher uma fase diferente da última
        FaseData fase = fases[Random.Range(0, fases.Count)];
        while (fase == ultimaFaseGerada && fases.Count > 1)
            fase = fases[Random.Range(0, fases.Count)];
        ultimaFaseGerada = fase;

        // Criar a fase
        GameObject novaFase = pools[fase.nome].Get();
        novaFase.name = fase.nome;
        novaFase.transform.position = new Vector3(posicaoXAtual, 0f, 0f);

        // Ajustar largura (puxando como o Rect Tool, sem achatar)
        float multiplicador = Random.Range(minWidthMultiplier, maxWidthMultiplier);
        Vector3 escala = novaFase.transform.localScale;
        novaFase.transform.localScale = new Vector3(escala.x * multiplicador, escala.y, escala.z);

        ativos.Add(novaFase);

        // Pega largura real visível
        SpriteRenderer sr = novaFase.GetComponent<SpriteRenderer>();
        float larguraFase = (sr != null ? sr.bounds.size.x : 10f);
        posicaoXAtual += larguraFase;

        // Se for urbana, coloca o BAR no final da fase
        if (fase.nome.ToLower().Contains("urbana"))
        {
            GameObject bar = pools["bar"].Get();
            bar.name = "bar";
            bar.transform.position = new Vector3(posicaoXAtual, 0f, 0f);
            ativos.Add(bar);

            SpriteRenderer srBar = bar.GetComponent<SpriteRenderer>();
            if (srBar != null)
                posicaoXAtual += srBar.bounds.size.x;
        }

        // Adiciona muro entre as fases
        GameObject muro = pools["muro"].Get();
        muro.name = "muro";
        muro.transform.position = new Vector3(posicaoXAtual, 0f, 0f);
        ativos.Add(muro);

        SpriteRenderer srMuro = muro.GetComponent<SpriteRenderer>();
        if (srMuro != null)
            posicaoXAtual += srMuro.bounds.size.x;
    }
}