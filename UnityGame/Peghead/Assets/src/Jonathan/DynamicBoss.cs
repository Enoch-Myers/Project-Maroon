using UnityEngine;

// Stage 1 Boss
public class BaseBoss1
{
    public Transform player;
    public float speed = 2f;
    public int maxHealth = 100;
    public int currentHealth;

    public BaseBoss1(Transform player, float speed, int maxHealth)
    {
        this.player = player;
        this.speed = speed;
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
    }

    public virtual void UpdateLogic(Transform bossTransform, float deltaTime, string bossName)
    {
        if(player != null){
            float bossX = bossTransform.position.x;
            float playerX = player.position.x;

            if(bossX < playerX){
                bossTransform.position += Vector3.right * speed * deltaTime;
            }else if(bossX > playerX){
                bossTransform.position += Vector3.left * speed * deltaTime;
            }
        }
    }

    public virtual void TakeDamage(int damageAmount, string bossName)
    {
        currentHealth -= damageAmount;
        if(currentHealth < 0) currentHealth = 0;

        if(currentHealth == 0){
            Debug.Log("Boss defeated: " + bossName);
        }
    }
}

// Stage 2 Dynamic Boss
public class BossStage2 : BaseBoss1
{
    public BossStage2(Transform player, float speed, int maxHealth)
        : base(player, speed, maxHealth)
        { }

    public override void TakeDamage(int damageAmount, string bossName)
    {
        // Half damage
        int actualDamage = Mathf.FloorToInt(damageAmount * 0.5f);
        base.TakeDamage(actualDamage, bossName);
    }

    public override void UpdateLogic(Transform bossTransform, float deltaTime, string bossName)
    {
        // Better Pathing
        if(player != null){
            Vector3 targetPosition = new Vector3(player.position.x, bossTransform.position.y, bossTransform.position.z);
            bossTransform.position = Vector3.Lerp(bossTransform.position, targetPosition, speed * deltaTime);
        }
    }
}