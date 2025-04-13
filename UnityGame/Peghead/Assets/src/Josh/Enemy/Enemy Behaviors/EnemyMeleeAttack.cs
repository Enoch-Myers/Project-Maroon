using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    public GameObject player;
    private float attackCooldown = 1f;
    private float timer = 0f;
    private bool canAttack = true;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    void Update()
    {
        if(!canAttack)
        {
            timer += Time.deltaTime;
            if(timer >= attackCooldown)
            {
                canAttack = true;
            } else 
            {
                return;
            }
        }

        if(Vector3.Distance(transform.position, player.transform.position) < 2f && canAttack)
        {
            DoMeleeAttack();

        }
    }

    void DoMeleeAttack()
    {
        // Debug.Log("Player is hit");
        playerHealth.TakeDamage();
        canAttack = false;
        timer = 0f;
    }
}
