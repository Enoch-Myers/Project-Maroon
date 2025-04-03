using System.Collections;
using UnityEngine;

public class MeleeEnemy : EnemyAI
{
    public float attackRange = 1f;
    public Transform attackPoint;
    public float attackRadius = 0.5f;
    public LayerMask playerLayer;

    protected override void Attack()
    {
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        if (isAttacking) yield break;
        isAttacking = true;

        yield return new WaitForSeconds(0.5f); // Attack delay

        Collider2D hitPlayer = Physics2D.OverlapBox(attackPoint.position, new Vector2(attackRadius, attackRadius), 0, playerLayer);
        if (hitPlayer != null)
        {
            Debug.Log("Player hit!");
        }

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
}
