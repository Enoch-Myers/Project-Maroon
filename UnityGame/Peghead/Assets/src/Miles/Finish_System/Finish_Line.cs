using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish_Line : MonoBehaviour
{
    public string nextSceneName;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.WinLevel(); // on player collision player wins level and gets pushed to entered scene
        }
    }
}
