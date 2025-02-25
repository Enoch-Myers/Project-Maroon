using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    void Start()
    {
        pauseMenuUI.SetActive(false);

        // Loop through all child objects under the "Options" parent
        foreach (Transform child in pauseMenuUI.transform.Find("Options"))
        {
            Button button = child.GetComponent<Button>();  // Get the Button component from the child
            if (button != null)  // Check if it's a valid button
            {
                // Use a listener to handle the button click event
                button.onClick.AddListener(() => OnButtonClick(button.name));
            }
        }
    }

    void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }
    
    void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (pauseMenuUI.activeInHierarchy) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    void OnButtonClick(string buttonName)
    {
        switch (buttonName)
        {
            case "Resume":
                Resume();
                break;
            case "Restart":
                Resume();
                SceneLoader.Instance.LoadSceneAsync(SceneManager.GetActiveScene().name);
                break;
            case "Exit":
                Resume();
                SceneLoader.Instance.LoadSceneAsync("LevelSelect");
                break;
            default:
                break;
        }
    }
}
