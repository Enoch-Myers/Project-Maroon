using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    public GameObject player;
    private float attackCooldown = 10f;
    private float timer = 0f;
    private bool canAttack = true;
    
    void Start()
    {

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
        //player.health -= 10;
        Debug.Log("Player is hit");
        canAttack = false;
        timer = 0f;
    }
}
