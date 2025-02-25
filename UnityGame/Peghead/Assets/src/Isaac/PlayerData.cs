using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    public int level = 0;  // Example player data
    
    // Called when the script instance is being loaded
    void Awake()
    {
        // Check if there's already an instance, if so, destroy the new one
        if (Instance == null)
        {
            Instance = this;  // Assign the current instance
            DontDestroyOnLoad(gameObject);  // Prevent this object from being destroyed between scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instance
        }
    }
}
