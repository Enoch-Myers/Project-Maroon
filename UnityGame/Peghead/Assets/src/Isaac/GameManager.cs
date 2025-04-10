using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Scene-Based Prefabs")]
    public GameObject dieWinScreenPrefab;
    public GameObject levelHUDPrefab;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Persist through scene loads

        // Subscribe to scene load event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // Also call once for the scene loaded at startup
        LoadSceneSpecificObjects(SceneManager.GetActiveScene());
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadSceneSpecificObjects(scene);
    }

    private void LoadSceneSpecificObjects(Scene scene)
    {
        string scenePath = scene.path;
        Debug.Log($"GameManager: Loading scene-specific UI for {scenePath}");

        // Only spawn UI if we're in a level
        if (scenePath.StartsWith("Assets/Scenes/Levels/"))
        {
            if (FindFirstObjectByType<DieWinScreen>() == null && dieWinScreenPrefab != null)
                Instantiate(dieWinScreenPrefab);

            if (FindFirstObjectByType<PlayerLivesHUD>() == null && levelHUDPrefab != null)
                Instantiate(levelHUDPrefab);
        }
    }
}
