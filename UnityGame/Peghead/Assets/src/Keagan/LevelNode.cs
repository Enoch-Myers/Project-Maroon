using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelNode : MonoBehaviour
{
    public LevelNode up = null, down = null, left = null, right = null; //pointers to neighbors in a specific direction
    public string levelID;
    public bool isSelected = false;

    private SpriteRenderer spriteRenderer;
    private Color defaultColor = Color.red;
    public Color selectedColor = Color.blue; //when highlighted

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }

    public void SelectNode()
    {
        isSelected = true;
        spriteRenderer.color = selectedColor;
    }

    public void DeselectNode()
    {
        isSelected = false;
        spriteRenderer.color = defaultColor;
    }

    public void ActivateNode()
    {
        Debug.Log("Starting Level: " + levelID);
        StartCoroutine(FadeToBlackAndLoadLevel());
    }

    private IEnumerator FadeToBlackAndLoadLevel()
    {
        //insert transition call on this line
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(levelID);
    }
}
