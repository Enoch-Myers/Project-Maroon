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
        print(scene.name + " loaded");
        if (scene.name != "TitleScreen") {
            cutoutMask.enabled = false; // Fixes a bug preventing the circle from rendering on a new scene loading
            cutoutMask.enabled = true;
            StartCoroutine(EndCircleWipeTransition());
        }
    }

    IEnumerator EndCircleWipeTransition()
    {
        transition.SetTrigger("End");
        
        yield return new WaitForSeconds(transitionTime);

        // transition.ResetTrigger("End");
    }

    // Starts a circle wipe transition from one scene to the next
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        print("Start circle wipe transition");
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        // transition.ResetTrigger("Start");

        SceneManager.LoadScene(sceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
