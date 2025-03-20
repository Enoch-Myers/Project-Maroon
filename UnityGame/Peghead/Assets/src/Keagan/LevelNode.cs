using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelNode : MonoBehaviour
{
    public LevelNode up = null, down = null, left = null, right = null; //pointers to neighbors in a specific direction
    public string levelID;
    public bool isSelected = false;
    public bool isLocked = true;
    
    private SpriteRenderer spriteRenderer;
    private Color defaultColor = Color.blue;
    private Color lockedColor = Color.red;
    private Color selectedColor = Color.green;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //defaultColor = spriteRenderer.color;
        spriteRenderer.color = lockedColor;
        if (!isLocked) {
            spriteRenderer.color = defaultColor;
        }
        if (isSelected) {
            spriteRenderer.color = selectedColor;
        }
    }

    public void SelectNode()
    {
        isSelected = true;
        spriteRenderer.color = selectedColor;
    }

    public void DeselectNode()
    {
        isSelected = false;
        if (isLocked){
            spriteRenderer.color = lockedColor;
        }else{
            spriteRenderer.color = defaultColor;
        }
    }

    public void ActivateNode()
    {
        if (!isLocked){
            Debug.Log("Starting Level: " + levelID);
            // StartCoroutine(TransitionLevel()); //weird unity interface functions require this being a new function
            SceneLoader.Instance.LoadSceneAsync(levelID);
        }else{
            Debug.Log("Attempted to start locked level: " + levelID);
        }
        
    }

    private IEnumerator TransitionLevel() //this is where levels are loaded
    {
        //insert transition call on this line
        yield return new WaitForSeconds(1);
        //SceneManager.LoadScene(levelID);
    }
}
