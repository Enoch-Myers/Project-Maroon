using UnityEngine;
using UnityEngine.UI;

public class DieWinScreen : MonoBehaviour
{
    public Image backgroundImage;    // Assign in Inspector
    public Text messageText;         // Assign in Inspector

    private void Start()
    {
        gameObject.SetActive(false); // Hide initially
    }

    private void Show(string message, Color textColor)
    {
        if (backgroundImage != null && messageText != null)
        {
            backgroundImage.color = new Color(0f, 0f, 0f, 0.8f); // semi-transparent black
            messageText.text = message;
            messageText.color = textColor;
            gameObject.SetActive(true);
        }
    }

    public void ShowWin()
    {
        Show("YOU WIN", Color.yellow);
    }

    public void ShowLose()
    {
        Show("YOU DIED", Color.red);
    }
}
