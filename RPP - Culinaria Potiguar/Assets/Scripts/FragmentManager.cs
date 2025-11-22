using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FragmentManager : MonoBehaviour
{
    public static FragmentManager Instance;
    public Text fragmentText;
    private int fragmentCount = 0;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("FragmentManager criado");
        }
        else
        {
            Debug.Log("FragmentManager duplicado destruído");
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        FindFragmentText();
        UpdateUI();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Cena carregada: " + scene.name);
        
        if (scene.name != "Final")
        {
            ResetFragments();
            
            // Reseta o spawner também
            CollectibleSpawner spawner = FindObjectOfType<CollectibleSpawner>();
            if (spawner != null)
            {
                spawner.ResetSpawner(); 
                Debug.Log("Spawner resetado");
            }
            
            // IMPORTANTE: Procura o texto novamente
            FindFragmentText();
            UpdateUI();
        }
    }
    
    private void FindFragmentText()
    {
        // Tenta encontrar pela tag
        GameObject textObj = GameObject.FindWithTag("FragmentText");
        if (textObj != null)
        {
            fragmentText = textObj.GetComponent<Text>();
            Debug.Log("FragmentText encontrado!");
        }
        else
        {
            Debug.LogError("Objeto com tag 'FragmentText' não encontrado na cena!");
        }
    }
    
    public void AddFragment()
    {
        fragmentCount++;
        Debug.Log("Fragmento coletado! Total: " + fragmentCount);
        UpdateUI();
        
        if (fragmentCount >= 10)
        {
            SceneManager.LoadScene("Final");
        }
    }
    
    public void ResetFragments()
    {
        fragmentCount = 0;
        Debug.Log("Fragmentos resetados para 0");
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        if (fragmentText != null)
        {
            fragmentText.text = " " + fragmentCount.ToString();
            Debug.Log("UI atualizada: " + fragmentCount);
        }
        else
        {
            Debug.LogWarning("fragmentText está NULL! Tentando encontrar novamente...");
            FindFragmentText();
            if (fragmentText != null)
            {
                fragmentText.text = " " + fragmentCount.ToString();
            }
        }
    }
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}