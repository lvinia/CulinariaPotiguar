using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;


public class BackgroundManager : MonoBehaviour
{
   [System.Serializable]
   public class FaseData
   {
       public string nome;       // ex: "Urbana"
       public GameObject prefab; // prefab da fase
       public string poolKey;    // opcional; se vazio usa prefab.name
       public string PoolKeyOrGet() => string.IsNullOrEmpty(poolKey) ? (prefab ? prefab.name : nome) : poolKey;
   }


   [Header("Fases e Elementos")]
   public List<FaseData> fases;
   public GameObject prefabMuro;
   public GameObject prefabBar;


   [Header("Configurações")]
   public float tamanhoMinimo = 15f;       // largura mínima da fase
   public float tamanhoMaximo = 25f;       // largura máxima da fase
   public float velocidadeScroll = 5f;     // unidades por segundo
   public float distanciaParaRemover = 40f; // quando objetos atrás da câmera devem voltar ao pool
   public int numeroFasesParaIniciar = 5;  // gera algumas fases no início


   private float posicaoXAtual = 0f;
   private Transform cameraTransform;
   private List<GameObject> objetosAtivos = new List<GameObject>();
   private Dictionary<string, ObjectPool<GameObject>> pools = new Dictionary<string, ObjectPool<GameObject>>();


   private FaseData ultimaFase = null; // última fase gerada, para evitar repetição imediata


   void Awake()
   {
       cameraTransform = Camera.main.transform;


       // Cria pools das fases
       foreach (var fase in fases)
       {
           if (fase.prefab == null) continue;
           string key = fase.PoolKeyOrGet();
           if (!pools.ContainsKey(key))
               pools[key] = CriarPool(fase.prefab, key);
       }


       // Pool muro
       if (prefabMuro)
       {
           string keyMuro = string.IsNullOrEmpty(prefabMuro.name) ? "Muro" : prefabMuro.name;
           if (!pools.ContainsKey(keyMuro))
               pools[keyMuro] = CriarPool(prefabMuro, keyMuro);
       }


       // Pool bar
       if (prefabBar)
       {
           string keyBar = string.IsNullOrEmpty(prefabBar.name) ? "Bar" : prefabBar.name;
           if (!pools.ContainsKey(keyBar))
               pools[keyBar] = CriarPool(prefabBar, keyBar);
       }
   }


   void Start()
   {
       // gera algumas fases iniciais
       for (int i = 0; i < numeroFasesParaIniciar; i++)
       {
           GerarNovaFase();
       }
   }


   // componente para identificar pool do objeto
   private class PoolIdentity : MonoBehaviour { public string poolKey; }


   private ObjectPool<GameObject> CriarPool(GameObject prefab, string poolKey)
   {
       return new ObjectPool<GameObject>(
           createFunc: () =>
           {
               GameObject obj = Instantiate(prefab);
               PoolIdentity id = obj.GetComponent<PoolIdentity>();
               if (id == null) id = obj.AddComponent<PoolIdentity>();
               id.poolKey = poolKey;
               obj.name = poolKey;
               obj.SetActive(false);
               return obj;
           },
           actionOnGet: (obj) => obj.SetActive(true),
           actionOnRelease: (obj) => obj.SetActive(false),
           actionOnDestroy: (obj) => Destroy(obj),
           collectionCheck: false,
           defaultCapacity: 5,
           maxSize: 50
       );
   }


   void Update()
   {
       float deslocamento = velocidadeScroll * Time.deltaTime;


       // move objetos ativos
       foreach (var obj in objetosAtivos)
       {
           if (obj != null && obj.activeSelf)
               obj.transform.position -= new Vector3(deslocamento, 0f, 0f);
       }


       // remove objetos que saíram da tela
       for (int i = objetosAtivos.Count - 1; i >= 0; i--)
       {
           GameObject obj = objetosAtivos[i];
           if (obj == null) { objetosAtivos.RemoveAt(i); continue; }


           if (obj.activeSelf && obj.transform.position.x < cameraTransform.position.x - distanciaParaRemover)
           {
               PoolIdentity id = obj.GetComponent<PoolIdentity>();
               if (id != null && pools.ContainsKey(id.poolKey))
                   pools[id.poolKey].Release(obj);
               else
                   obj.SetActive(false);


               objetosAtivos.RemoveAt(i);
           }
       }


       // garante que sempre haja fases suficientes à frente da câmera
       while (posicaoXAtual < cameraTransform.position.x + Camera.main.orthographicSize * Camera.main.aspect + 20f)
       {
           GerarNovaFase();
       }
   }


   private void GerarNovaFase()
   {
       // escolhe fase aleatória, diferente da última
       FaseData faseEscolhida;
       do
       {
           faseEscolhida = fases[Random.Range(0, fases.Count)];
       } while (fases.Count > 1 && faseEscolhida == ultimaFase);


       CriarFase(faseEscolhida);


       // adiciona muro
       if (prefabMuro) CriarMuro();


       ultimaFase = faseEscolhida;
   }


   private void CriarFase(FaseData faseData)
   {
       string chave = faseData.PoolKeyOrGet();
       if (!pools.ContainsKey(chave)) return;


       GameObject fase = pools[chave].Get();
       fase.transform.SetParent(transform, worldPositionStays: true);


       // largura aleatória apenas para fases
       float larguraAleatoria = Random.Range(tamanhoMinimo, tamanhoMaximo);


       SpriteRenderer sr = fase.GetComponent<SpriteRenderer>();
       if (sr != null && sr.drawMode == SpriteDrawMode.Tiled)
       {
           Vector2 size = sr.size;
           size.x = larguraAleatoria; // aumenta horizontalmente sem achatar
           sr.size = size;
       }


       // se fase urbana, adiciona bar colado na borda esquerda da fase
       if (!string.IsNullOrEmpty(faseData.nome) && faseData.nome.ToLowerInvariant().Contains("urbana") && prefabBar != null)
       {
           string barKey = string.IsNullOrEmpty(prefabBar.name) ? "Bar" : prefabBar.name;
           if (pools.ContainsKey(barKey))
           {
               GameObject bar = pools[barKey].Get();
               bar.transform.SetParent(transform, worldPositionStays: true);


               SpriteRenderer srBar = bar.GetComponent<SpriteRenderer>();
               float larguraBar = (srBar != null) ? srBar.size.x : 5f;


               // posiciona bar na borda esquerda da fase urbana
               float barPosX = posicaoXAtual + (larguraBar / 2f);
               bar.transform.position = new Vector3(barPosX, 0f, 0f);
               objetosAtivos.Add(bar);
           }
       }


       // posiciona a fase (após o bar)
       float fasePosX = posicaoXAtual + (larguraAleatoria / 2f);
       fase.transform.position = new Vector3(fasePosX, 0f, 0f);
       objetosAtivos.Add(fase);


       posicaoXAtual += larguraAleatoria;
   }


   private void CriarMuro()
   {
       if (!prefabMuro) return;


       string key = string.IsNullOrEmpty(prefabMuro.name) ? "Muro" : prefabMuro.name;
       if (!pools.ContainsKey(key)) return;


       GameObject muro = pools[key].Get();
       muro.transform.SetParent(transform, worldPositionStays: true);


       SpriteRenderer sr = muro.GetComponent<SpriteRenderer>();
       float largura = (sr != null) ? sr.size.x : 5f;


       float muroPosX = posicaoXAtual + (largura / 2f);
       muro.transform.position = new Vector3(muroPosX, 0f, 0f);
       objetosAtivos.Add(muro);


       posicaoXAtual += largura;
   }
}
