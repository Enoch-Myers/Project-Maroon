using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneManager.LoadScene("TitleScreen");
    }
    
    public void PressPlay()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
