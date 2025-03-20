using UnityEngine;
public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        // Prevents the GameObject this script is attached to from being destroyed when loading a new scene
        DontDestroyOnLoad(gameObject);
    }
}