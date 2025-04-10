using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Animator transition;
    public Mask cutoutMask;
    public float transitionTime = 1f;

    public static SceneLoader Instance { get; private set; }

    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isTransitioning = false;

        if (scene.name != "TitleScreen") {
            // Fixes a bug preventing the circle from rendering on a new scene loading
            cutoutMask.enabled = false;
            cutoutMask.enabled = true;
            StartCoroutine(EndCircleWipeTransition());
        }
    }

    IEnumerator EndCircleWipeTransition()
    {
        transition.SetTrigger("End");

        yield return new WaitForSeconds(transitionTime);
        
        // Optional: reset the trigger if needed
        // transition.ResetTrigger("End");
    }

    // Starts a circle wipe transition from one scene to the next
    public void LoadSceneAsync(string sceneName)
    {
        if (isTransitioning) return;

        isTransitioning = true;
        StartCoroutine(LoadScene(sceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        // Optional: reset the trigger if needed
        // transition.ResetTrigger("Start");

        SceneManager.LoadScene(sceneName);
    }

    // Update is unused but kept for future use
    void Update()
    {
        
    }
}
