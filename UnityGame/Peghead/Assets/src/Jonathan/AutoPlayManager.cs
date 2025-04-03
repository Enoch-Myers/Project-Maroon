using UnityEngine;

public class AutoPlayManager : MonoBehaviour
{
    [Header("Inactivity Settings")]
    public float inactivityThreshold = 10f;

    [Header("Player Object")]
    public GameObject playerObject;

    private player_movement playerController;
    private DemoMode autoPlayController;

    private float lastActivityTime;
    private bool isAutoPlaying = false;

    void Start()
    {
        playerController = playerObject.GetComponent<player_movement>();
        autoPlayController = playerObject.GetComponent<DemoMode>();

        playerController.enabled = true;
        autoPlayController.enabled = false;

        lastActivityTime = Time.time;
    }

    void Update()
    {
        if (isAutoPlaying) return;

        if(Input.anyKeyDown ||
            Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01f ||
            Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.01f)
        {
            lastActivityTime = Time.time;
        }

        if(Time.time - lastActivityTime >= inactivityThreshold){
            StartAutoPlay();
        }
    }

    private void StartAutoPlay()
    {
        isAutoPlaying = true;
        Debug.Log("Starting DemoMode.");

        playerController.enabled = false;
        autoPlayController.enabled = true;
    }
}
