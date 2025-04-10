using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private GameObject dieWinScreenPrefab, statsScreenPrefab, hudScreenPrefab, pauseScreenPrefab;
    private bool didWinLevel = false;

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
        PlayerHealth.OnPlayerDied += OnPlayerDied;
        LoadSceneSpecificObjects();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadSceneSpecificObjects();
    }

    public bool CheckInLevel()
    {
        string scenePath = SceneManager.GetActiveScene().path;
        return scenePath.StartsWith("Assets/Scenes/Levels/");
    }

    public void WinLevel()
    {
        if (!CheckInLevel() || didWinLevel) return;
        didWinLevel = true;
        StartCoroutine(WinLevelHandler());
    }

    private void OnPlayerDied()
    {
        StartCoroutine(OnPlayerDiedHandler());
    }

    private IEnumerator OnPlayerDiedHandler()
    {
        Time.timeScale = 0;

        DieWinScreen dieWinScreen = FindFirstObjectByType<DieWinScreen>();
        dieWinScreen.ShowLose();

        yield return new WaitForSeconds(1.5f);

        dieWinScreen.gameObject.SetActive(false);

        SceneLoader.Instance.LoadSceneAsync("LevelSelect");
    }

    private IEnumerator WinLevelHandler()
    {
        Time.timeScale = 0;

        DieWinScreen dieWinScreen = FindFirstObjectByType<DieWinScreen>();
        dieWinScreen.ShowWin();

        yield return new WaitForSeconds(1.5f);

        dieWinScreen.gameObject.SetActive(false);
        StatsScreen statsScreen = FindFirstObjectByType<StatsScreen>();
        statsScreen.ShowLevelResults();

        yield return new WaitForSeconds(5f);

        SceneLoader.Instance.LoadSceneAsync("LevelSelect");
    }

    private void LoadSceneSpecificObjects()
    {
        if (CheckInLevel()) // We're in a level
        {
            Instantiate(dieWinScreenPrefab);
            Instantiate(hudScreenPrefab);
            Instantiate(statsScreenPrefab);
            Instantiate(pauseScreenPrefab);
        }
    }
}
