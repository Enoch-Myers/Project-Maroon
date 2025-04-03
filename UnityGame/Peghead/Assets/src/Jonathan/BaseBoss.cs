/*using UnityEngine;

//Parent Class other bosses inherit from
public class BaseBoss : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public int maxHealth = 100;
    public int currentHealth;

    public virtual void Start(){
        currentHealth = maxHealth;
    }

    public virtual void Update(){
        if(player != null){
            if(transform.position.x < player.position.x){
                transform.position += Vector3.right * speed * Time.deltaTime;
            }else if(transform.position.x > player.position.x){
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
        }
    }

    public virtual void TakeDamage(int damageAmount){
        currentHealth -= damageAmount;
        if(currentHealth < 0) currentHealth = 0;
        
        if (currentHealth == 0) Debug.Log("Boss defeated: " + gameObject.name);
    }
}

//First stage of boss
public class BStage1 : BaseBoss
{
    public override void TakeDamage(int damageAmount){
        //Normal Damage
        base.TakeDamage(damageAmount);
        
        if(currentHealth <= 0){
            //Start stage 2
            public gameObject BStage2;
            if(BStage2 != null){
                Instantiate(BStage2, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
    }
    public override void Update(){
        base.Update();
        //Additional Logic Later
    }
}

//Second stage of boss
public class BStage2 : BaseBoss
{
    //Half damage
    public override void TakeDamage(int damageAmount){
        int actualDamage = Mathf.FloorToInt(damageAmount * 0.5f);
        base.TakeDamage(actualDamage);
    }

    public override void Update(){
        base.Update();
        //Additional Logic Later
    }
}*/