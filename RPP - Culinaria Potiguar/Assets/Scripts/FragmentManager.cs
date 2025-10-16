using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FragmentManager : MonoBehaviour
{
    public static FragmentManager Instance; // Singleton para acessar de outros scripts
    public Text fragmentText; // Referência ao texto da UI
    private int fragmentCount = 0;

    private void Awake()
    {
        // Garante que só exista um FragmentManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persiste entre cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
        SceneManager.sceneLoaded += OnSceneLoaded; // Quando cena carregar, atualiza UI
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Se a cena for o jogo, tenta achar o texto de novo (caso ele seja destruído)
        if (scene.name != "Final")
        {
            if (fragmentText == null)
                fragmentText = GameObject.FindWithTag("FragmentText")?.GetComponent<Text>();
            UpdateUI();
        }
    }

    public void AddFragment()
    {
        fragmentCount++;
        UpdateUI();

        if (fragmentCount >= 10)
        {
            fragmentCount = 0;
            SceneManager.LoadScene("Final");
        }
    }

    public void ResetFragments()
    {
        fragmentCount = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (fragmentText != null)
            fragmentText.text = "Fragmentos: " + fragmentCount.ToString();
    }
}