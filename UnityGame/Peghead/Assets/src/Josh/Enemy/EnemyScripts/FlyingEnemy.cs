using System.Collections;
using UnityEngine;

public class FlyingEnemy : EnemyAI
{
    public Transform pointA;
    public Transform pointB;
    public float verticalChaseRange = 3f;

    public float patrolSpeed = 2f; // Speed for patrolling
    public float chaseSpeed = 3f;  // Speed for chasing the player

    private bool movingToB = true;

    protected override void Start()
    {
        base.Start();
        transform.position = pointA.position; // Start at point A
    }

    protected override void Update()
    {
        if (isDead) return;

        if (PlayerDetected())
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    protected override void Patrol()
    {
        Transform target = movingToB ? pointB : pointA;
        transform.position = Vector2.MoveTowards(transform.position, target.position, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            movingToB = !movingToB;
        }
    }

    protected override void ChasePlayer()
    {
        if (player == null) return;

        // Move toward the player while keeping a certain height difference
        Vector2 targetPosition = new Vector2(player.position.x, player.position.y + verticalChaseRange);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, chaseSpeed * Time.deltaTime);
    }

    protected override void Attack()
    {
        Debug.Log("Flying enemy attacks! (Override this method for ranged attack logic if needed)");
    }
}
