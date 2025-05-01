using UnityEngine;

public class player_attack_controller : MonoBehaviour
{

    public Projectile_behavior ProjectilePrefab;
    public Transform launchOffset;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("player shot");
            Instantiate(ProjectilePrefab, launchOffset.position, transform.rotation);
        }
    }
}
