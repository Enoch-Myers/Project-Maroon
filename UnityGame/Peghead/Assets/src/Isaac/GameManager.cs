using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private GameObject dieWinScreenPrefab, statsScreenPrefab, hudScreenPrefab, pauseScreenPrefab;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Persist through scene loads

        dieWinScreenPrefab = Resources.Load<GameObject>("Prefabs/DieWinScreen");
        hudScreenPrefab = Resources.Load<GameObject>("Prefabs/HUD");
        statsScreenPrefab = Resources.Load<GameObject>("Prefabs/StatsScreen");
        pauseScreenPrefab = Resources.Load<GameObject>("Prefabs/PauseScreen");

        // Subscribe to scene load event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
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
 
        if (scenePath.StartsWith("Assets/Scenes/Levels/")) // We're in a level
        {
            Instantiate(dieWinScreenPrefab);
            Instantiate(hudScreenPrefab);
            Instantiate(statsScreenPrefab);
            Instantiate(pauseScreenPrefab);
        }
    }
}
