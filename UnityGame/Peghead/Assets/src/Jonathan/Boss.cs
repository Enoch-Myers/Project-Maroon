using UnityEngine;

// Static Base Boss class
public class BaseBoss
{
    public Transform player;
    public float speed = 2f;

    public BaseBoss(Transform player, int speed)
    {
        this.player = player;
        this.speed = speed;
    }

    // Basic movement move horizontally toward the player.
    public virtual void UpdateBoss(Transform bossTransform, float deltaTime)
    {
        if(player != null){
            if(bossTransform.position.x < player.position.x){
                bossTransform.position += Vector3.right * speed * deltaTime;
            }else if(bossTransform.position.x > player.position.x){
                bossTransform.position += Vector3.left * speed * deltaTime;
            }
        }
    }
}

// Dynamic Stage 2 boss: Better pathing behavior
public class BStage2 : BaseBoss
{
    public BStage2(Transform player, int speed)
        : base(player, speed)
        { }


    public override void UpdateBoss(Transform bossTransform, float deltaTime)
    {
        
        if(player != null){
            Vector3 targetPos = new Vector3(player.position.x, bossTransform.position.y, bossTransform.position.z);
            bossTransform.position = Vector3.Lerp(bossTransform.position, targetPos, speed * deltaTime);
        }
    }
}